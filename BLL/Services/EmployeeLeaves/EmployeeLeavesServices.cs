using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.Notification;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLeaves;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Identity;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;


namespace BusinessLogicLayer.Services.EmployeeLeaves;

internal class EmployeeLeavesService : IEmployeeLeavesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILookupsService _lookupsService;
    private readonly IMapper _mapper;
    readonly IProjectProvider _projectProvider;
    readonly IAuthService _authService;
    readonly INotificationsService _iNotificationsService;
    readonly DataAccessLayer.Models.PayrolLogOnlyContext _payrolLogOnlyContext;
    readonly int _userId;
    readonly int _projecId;
    public EmployeeLeavesService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper, IProjectProvider projectProvider
        , IAuthService authService, INotificationsService iNotificationsService, DataAccessLayer.Models.PayrolLogOnlyContext payrolLogOnlyContext)
    {
        _unitOfWork = unityOfWork;
        _lookupsService = lookupsService;
        _mapper = mapper;
        _projectProvider = projectProvider;
        _authService = authService;
        _iNotificationsService = iNotificationsService;
        _userId = _projectProvider.UserId();
        _projecId = _projectProvider.GetProjectId();
        _payrolLogOnlyContext = payrolLogOnlyContext;
    }
    public async Task<EmployeeLeavesOutput> Get(int id)
    {
        var leave = _unitOfWork.EmployeeLeaveRepository
                   .PQuery(e => e.EmployeeLeaveID == id, include: e => e.Employee)
                   .FirstOrDefault();

        if (leave is null)
            throw new NotFoundException("data not found");

        var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

        var result = new EmployeeLeavesOutput
        {
            ID = leave.EmployeeLeaveID,
            EmployeeID = leave.EmployeeID,
            EmployeeName = leave.Employee.EmployeeName,
            LeaveTypeID = leave.LeaveTypeID,
            LeaveType = lookups.FirstOrDefault(e => leave.LeaveTypeID is not null
                             && e.ID == leave.LeaveTypeID)?.ColumnDescription,
            LeaveDate = leave.LeaveDate.IntToDateValue(),
            FromTime = leave.FromTime.ConvertFromMinutesToTimeString(),
            ToTime = leave.ToTime.ConvertFromMinutesToTimeString(),
            imagepath = leave.imagepath

        };

        return result;
    }

    private static IQueryable<EmployeeLeaf> ApplyFilters(IQueryable<EmployeeLeaf> query, EmployeeLeaveFilter criteria)
    {
        if (criteria == null)
            return query;

        var parameter = Expression.Parameter(typeof(EmployeeLeaf), "e");
        Expression combinedExpression = null;

        if (criteria.EmployeeID != null)
        {
            var employeeIdExpression = Expression.Equal(
                Expression.Property(parameter, "EmployeeID"),
                Expression.Constant(criteria.EmployeeID)
            );
            combinedExpression = employeeIdExpression;
        }

        if (criteria.LeaveTypeID != null)
        {
            var leaveTypeIdExpression = Expression.Equal(
                Expression.Property(parameter, "LeaveTypeID"),
                Expression.Constant(criteria.LeaveTypeID, typeof(int?))
            );
            combinedExpression = combinedExpression == null
                ? leaveTypeIdExpression
                : Expression.AndAlso(combinedExpression, leaveTypeIdExpression);
        }
        if (!string.IsNullOrEmpty(criteria.FromTime))
        {
            var FromTimeExpression = Expression.Equal(
                Expression.Property(parameter, "FromTime"),
                Expression.Constant(criteria.FromTime.ConvertFromTimeStringToMinutes(), typeof(int?))
            );
            combinedExpression = combinedExpression == null
                ? FromTimeExpression
                : Expression.AndAlso(combinedExpression, FromTimeExpression);
        }
        if (!string.IsNullOrEmpty(criteria.ToTime))
        {
            var ToExpression = Expression.Equal(
                Expression.Property(parameter, "ToTime"),
                Expression.Constant(criteria.ToTime.ConvertFromTimeStringToMinutes(), typeof(int?))
            );
            combinedExpression = combinedExpression == null
                ? ToExpression
                : Expression.AndAlso(combinedExpression, ToExpression);
        }

        if (criteria.FromDate != null && criteria.ToDate != null)
        {
            var fromDateExpression = Expression.GreaterThanOrEqual(
                Expression.Property(parameter, "LeaveDate"),
                Expression.Constant(criteria.FromDate.DateToIntValue(), typeof(int?))
            );
            var toDateExpression = Expression.LessThanOrEqual(
                Expression.Property(parameter, "LeaveDate"),
                Expression.Constant(criteria.ToDate.DateToIntValue(), typeof(int?))
            );
            var dateRangeExpression = Expression.AndAlso(fromDateExpression, toDateExpression);
            combinedExpression = combinedExpression == null
                ? dateRangeExpression
                : Expression.AndAlso(combinedExpression, dateRangeExpression);
        }


        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<EmployeeLeaf, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        return query;


    }

    public async Task<PagedResponse<EmployeeLeavesOutput>> GetPage(PaginationFilter<EmployeeLeaveFilter> filter)
    {

        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId from header");
        if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
        int? employeeId = _authService.IsHr(_userId);

        var query = from e in _unitOfWork.EmployeeRepository.PQuery()
                    join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
                    from lt in ltGroup.DefaultIfEmpty()
                    join el in _unitOfWork.EmployeeLeaveRepository.PQuery() on e.EmployeeID equals el.EmployeeID
                    where (lt.TableName == "Department" && lt.ColumnName == "DepartmentID") && e.ProjectID == _projecId && lt.ProjectID == _projecId && el.ProjectID == _projecId && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId == null)
                    select new EmployeeLeaf
                    {
                        Employee = e,
                        EmployeeID = e.EmployeeID,
                        approvalstatusid = el.approvalstatusid,
                        EmployeeLeaveID = el.EmployeeLeaveID,
                        LeaveTypeID = el.LeaveTypeID,
                        ProjectID = el.ProjectID,
                        LeaveDate = el.LeaveDate,
                        FromTime = el.FromTime,
                        ToTime = el.ToTime,
                        statusid = el.statusid,
                        imagepath = el.imagepath
                    };

        var rquery = filter.FilterCriteria != null ? ApplyFilters(query, filter.FilterCriteria) : query;

        var totalRecords = await rquery.CountAsync();


        var leaves = await rquery.Skip((filter.PageIndex - 1) * filter.Offset)
                    .Take(filter.Offset).ToListAsync();

        var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

        var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

        var result = leaves.Select(item => new EmployeeLeavesOutput
        {
            ID = item.EmployeeLeaveID,
            EmployeeID = item.EmployeeID,
            EmployeeName = item.Employee.EmployeeName,
            LeaveTypeID = item.LeaveTypeID,
            ProjectID = item.ProjectID,
            LeaveType = lookups.FirstOrDefault(e => item.LeaveTypeID is not null
                             && e.ID == item.LeaveTypeID)?.ColumnDescription,
            LeaveDate = item.LeaveDate.IntToDateValue(),
            FromTime = item.FromTime.ConvertFromMinutesToTimeString(),
            ToTime = item.ToTime.ConvertFromMinutesToTimeString(),
            ApprovalStatus = approvals.FirstOrDefault(e => e.ColumnValue == item.approvalstatusid.ToString())?.ColumnDescriptionAr,
            statusid = item.statusid,
            imagepath = item.imagepath
        }).ToList();

        return result.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);
    }

    public async Task Create(EmployeeLeavesInput model)
    {

        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
        if (model == null)
            throw new NotFoundException("recieved data is missed");

        var timing = GetLeaveTimingInputs(model);
        if (timing.FromTime > timing.ToTime)
            throw new BadRequestException("وقت بدايةالمغادرة لابد ان يكون اصغر من وقت نهاية المغادرة");

        string checkValidation = await checkValidationOfLeave(model);//validation confilct Leave 
        if (!string.IsNullOrEmpty(checkValidation))
        {
            throw new BadRequestException(checkValidation);
        }
        var LeaveDate = model.LeaveDate;
        model.LeaveDate = null;
        model.FromTime = null;
        model.ToTime = null;

        var employeeLeave = _mapper.Map<EmployeeLeaf>(model);

        employeeLeave.LeaveDate = LeaveDate.DateToIntValue();
        employeeLeave.FromTime = timing.FromTime;
        employeeLeave.ToTime = timing.ToTime;

        await _unitOfWork.EmployeeLeaveRepository.PInsertAsync(employeeLeave);

        await _unitOfWork.SaveAsync();
        var insertedPKValue = employeeLeave.EmployeeLeaveID;
        //update img path 
        if (model.File is not null)
        {
            var fileExtension = Path.GetExtension(model.File.FileName);
            var settingResult = await _lookupsService.GetSettings();
            var projectPath = settingResult.AttachementPath;
            var fileName = "01" + model.EmployeeID.ToString().PadLeft(6, '0') + insertedPKValue.ToString().PadLeft(6, '0') + fileExtension;
            var filePath = projectPath + fileName;
            //save img path to database
            employeeLeave.imagepath = filePath;
            await _unitOfWork.EmployeeLeaveRepository.PUpdateAsync(employeeLeave);
            await _unitOfWork.SaveAsync();
            using (var fileStream = model.File.OpenReadStream())
            {
                string ftpUrl = filePath;
                string userName = settingResult.WindowsUserName;
                string password = settingResult.WindowsUserPassword;
                bool IsComplete = PublicHelper.UploadFileToFtp(ftpUrl, userName, password, fileStream, fileName);
            }
        }
        await sendToNotification(employeeLeave.EmployeeID, insertedPKValue);
    }
    async Task sendToNotification(int employeeId, int PKID)
    {
        AcceptOrRejectNotifcationInput model = new AcceptOrRejectNotifcationInput()
        {
            ProjectID = _projecId,
            CreatedBy = _userId,
            EmployeeId = employeeId,
            ApprovalStatusId = 0,
            SendToLog = 0,
            Id = PKID,
            ApprovalPageID = 2,
            PrevilageType = _authService.GetUserType(_userId, employeeId)
        };
        await _iNotificationsService.AcceptOrRejectNotificationsAsync(model);
    }
    private async Task<string> checkValidationOfLeave(dynamic model)
    {

        DateTime? FromDate = model.LeaveDate;
        string FromTime = model.FromTime;
        string ToTime = model.ToTime;
        var inputParams = new Dictionary<string, object>
            {
                {"pEmployeeLeaveID", model.ID},
                {"pEmployeeID", model.EmployeeID==0 ? null:model.EmployeeID},
                {"pLeaveTypeID", model.LeaveTypeID},
                {"pLeaveDate", FromDate==null?null: FromDate.DateToIntValue()},
                {"pFromTime", FromTime==null?null:FromTime.ConvertFromTimeStringToMinutes()},
                {"pToTime", ToTime==null?null:ToTime.ConvertFromTimeStringToMinutes()},
                {"pProjectId",_projectProvider.GetProjectId() }
            };
        var outParams = new Dictionary<string, object>
            {

                {"pError","int" }
            };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.CheckEmployeeLeaves", inputParams, outParams);

        
        if (outputValues.TryGetValue("pError", out var value))
        {
            if (Convert.ToInt32(value) == -5)
            {
                throw new UnauthorizedAccessException("CannotAddLeaveInClosedYear");
            }
            if (Convert.ToInt32(value) == -3)
            {
                throw new UnauthorizedAccessException("ThereIsConflictWithAnotherLeave");
            }
            if (Convert.ToInt32(value) == -6)
            {
                throw new UnauthorizedAccessException("ThereIsConflictWithAnotherVacation");
            }

        }
        return null;
    }




    public async Task Update(EmployeeLeavesUpdate employeeLeave)
    {
        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

        var leave = _unitOfWork.EmployeeLeaveRepository.Get(emp => emp.EmployeeLeaveID == employeeLeave.ID)
            .FirstOrDefault();

        if (leave is null)
            throw new NotFoundException("Data Not Found");

        string checkValidation = await checkValidationOfLeave(employeeLeave);//validation confilct Leave 
        if (!string.IsNullOrEmpty(checkValidation))
        {
            throw new BadRequestException(checkValidation);
        }

        var timing = GetLeaveTimingInputs(employeeLeave);

        leave.LeaveDate = employeeLeave.LeaveDate.DateToIntValue();// timing.LeaveDate;//
        leave.FromTime = timing.FromTime;
        leave.ToTime = timing.ToTime;
        leave.ModificationDate = DateTime.Now;
        leave.LeaveTypeID = employeeLeave.LeaveTypeID;

        leave.LeaveDate = employeeLeave.LeaveDate.DateToIntValue();// timing.LeaveDate;//
        leave.FromTime = timing.FromTime;
        leave.ToTime = timing.ToTime;
        leave.LeaveDate = employeeLeave.LeaveDate.DateToIntValue();
        leave.LeaveTypeID = employeeLeave.LeaveTypeID;


        //update img path 
        if (employeeLeave.File is not null)
        {
            var fileExtension = Path.GetExtension(employeeLeave.File.FileName);
            var settingResult = await _lookupsService.GetSettings();
            var projectPath = settingResult.AttachementPath;
            var fileName = "01" + employeeLeave.EmployeeID.ToString().PadLeft(6, '0') + employeeLeave.ID.ToString().PadLeft(6, '0') + fileExtension;
            var filePath = projectPath + fileName;
            //save img path to database
            leave.imagepath = filePath;

            using (var fileStream = employeeLeave.File.OpenReadStream())
            {
                string ftpUrl = filePath;
                string userName = settingResult.WindowsUserName;
                string password = settingResult.WindowsUserPassword;
                bool IsComplete = PublicHelper.UploadFileToFtp(ftpUrl, userName, password, fileStream, fileName);
            }
        }



        await _unitOfWork.EmployeeLeaveRepository.PUpdateAsync(leave);

        await _unitOfWork.SaveAsync();

    }

    public async Task Delete(int employeeLeaveId)
    {
        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

        var leave = _unitOfWork.EmployeeLeaveRepository
                    .Get(e => e.EmployeeLeaveID == employeeLeaveId)
                    .FirstOrDefault();

        if (leave is null)
            throw new NotFoundException("Data Not Found");

        _unitOfWork.EmployeeLeaveRepository.Delete(leave);

        await _unitOfWork.SaveAsync();

    }

    private (int? FromTime, int? ToTime, int? LeaveDate) GetLeaveTimingInputs(EmployeeLeavesInput model)
    {
        return (
               FromTime: model.FromTime.ConvertFromTimeStringToMinutes(),
               ToTime: model.ToTime.ConvertFromTimeStringToMinutes(),
               LeaveDate: model.LeaveDate.ConvertFromDateTimeToUnixTimestamp()
            );
    }

    private (int? FromTime, int? ToTime, int? LeaveDate) GetLeaveTimingInputs(EmployeeLeavesUpdate model)
    {
        return (
               FromTime: model.FromTime.ConvertFromTimeStringToMinutes(),
               ToTime: model.ToTime.ConvertFromTimeStringToMinutes(),
               LeaveDate: model.LeaveDate.ConvertFromDateTimeToUnixTimestamp()
            );
    }

}
