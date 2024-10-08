﻿using AutoMapper;
using Azure;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.Notification;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.DTO.Locations;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.Identity;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using NotImplementedException = BusinessLogicLayer.Exceptions.NotImplementedException;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;

namespace BusinessLogicLayer.Services.EmployeeVacations
{
    internal class EmployeeVacationService : IEmployeeVacationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILookupsService _lookupsService;
        private readonly IMapper _mapper;
        readonly IProjectProvider _projectProvider;
        readonly IAuthService _authService;
        readonly INotificationsService _iNotificationsService;
        private readonly DataAccessLayer.Models.PayrolLogOnlyContext _payrolLogOnlyContext;
        readonly int _userId;
        readonly int _projecId;
        public EmployeeVacationService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper, IProjectProvider projectProvider, IAuthService authService
            , INotificationsService iNotificationsService, DataAccessLayer.Models.PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _unitOfWork     = unityOfWork;
            _lookupsService = lookupsService;
            _mapper         = mapper;
            _projectProvider = projectProvider;
            _authService = authService;
            _iNotificationsService = iNotificationsService;
            _userId = _projectProvider.UserId();
            _projecId = _projectProvider.GetProjectId();
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }
        public async Task<EmployeeVacationOutput> Get(int id)
        {
            var Vacation = _unitOfWork.EmployeeVacationRepository
                       .PQuery(e => e.EmployeeVacationID == id, include: e => e.Employee)
                       .FirstOrDefault();

            if (Vacation is null)
                throw new NotFoundException("data not found");

            var lookups = await _lookupsService.GetLookups(Constants.VacationType, Constants.VacationTypeId);

            var result = new EmployeeVacationOutput
            {
                ID = Vacation.EmployeeVacationID,
                EmployeeID = Vacation.EmployeeID,
                EmployeeName = Vacation.Employee.EmployeeName,
                VacationTypeID = Vacation.VacationTypeID,
                VacationType = lookups.FirstOrDefault(e => Vacation.VacationTypeID is not null
                                 && e.ID == Vacation.VacationTypeID)?.ColumnDescription,
                FromDate = Vacation.FromDate.IntToDateValue(),
                ToDate = Vacation.ToDate.IntToDateValue(),
                imagepath=Vacation.imagepath
            };
            return result;
        }

        public async Task<dynamic> GetPage(PaginationFilter<EmployeeVacationFilter> filter)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
            //int? employeeId = _authService.IsHr(_userId);

            //var query = from e in _unitOfWork.EmployeeRepository.PQuery()
            //            join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
            //            from lt in ltGroup.DefaultIfEmpty()
            //            join ev in _unitOfWork.EmployeeVacationRepository.PQuery() on e.EmployeeID equals ev.EmployeeID
            //            where (lt.TableName == "Department" && lt.ColumnName == "DepartmentID") && e.ProjectID == _projecId && lt.ProjectID == _projecId && ev.ProjectID == _projecId && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId == null)
            //            select new EmployeeVacation
            //            {
            //                Employee = e,
            //                EmployeeID = e.EmployeeID,
            //                ApprovalStatusID = ev.ApprovalStatusID,
            //                EmployeeVacationID = ev.EmployeeVacationID,
            //                VacationTypeID = ev.VacationTypeID,
            //                ProjectID = ev.ProjectID,
            //                FromDate = ev.FromDate,
            //                ToDate = ev.ToDate,
            //                DayCount=ev.DayCount,
            //                Notes=ev.Notes,
            //                StatusID=ev.StatusID,
            //                imagepath= ev.imagepath

            //            };
            ////var query = _unitOfWork.EmployeeVacationRepository.PQuery(include: e => e.Employee);   


            //if (filter.FilterCriteria != null)
            //    query= ApplyFilter(query, filter.FilterCriteria);

            //var totalRecords = await query.CountAsync();

            //var Vacation = await query.Skip((filter.PageIndex - 1) * filter.Offset)
            //        .Take(filter.Offset).ToListAsync();

           // var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);
           
            //var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

            var inputParams = new Dictionary<string, object>()
            {
                { "pProjectID",_projectProvider.GetProjectId()},                
                 {"pFromDate",filter.FilterCriteria.FromDate!=null?filter.FilterCriteria.FromDate.DateToIntValue():Convert.DBNull},
                {"pToDate", filter.FilterCriteria.ToDate!=null ?filter.FilterCriteria.ToDate.DateToIntValue():Convert.DBNull },
                {"ploginuserid",_projectProvider.UserId()},
                {"pEmployeeID",filter.FilterCriteria.EmployeeID },
                {"pVacationTypeID",  filter.FilterCriteria.VacationTypeId??Convert.DBNull},
                {"pPageNo",filter.PageIndex },
                {"pPageSize", filter.Offset}
            };
            var outputParams = new Dictionary<string, object>() { { "prowcount", "int" } };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<EmployeeVacationResult>("dbo.GetEmployeeVacation", inputParams, outputParams);


            //var result = Vacation.Select(item => new EmployeeVacationOutput 
            //{
            //    ID              = item.EmployeeVacationID,
            //    EmployeeID      = item.EmployeeID,
            //    EmployeeName    = item.Employee.EmployeeName,
            //    VacationTypeID  = item.VacationTypeID,
            //    VacationType    = lookups.FirstOrDefault(e => item.VacationTypeID is not null
            //                     && e.ID == item.VacationTypeID)?.ColumnDescription,
            //    FromDate        = item.FromDate.IntToDateValue(),
            //    ToDate          = item.ToDate.IntToDateValue() ,
            //    DayCount        = item.DayCount,
            //    Notes           = item.Notes,
            //    ApprovalStatus  = approvals.FirstOrDefault(e => e.ColumnValue == item.ApprovalStatusID.ToString())?.ColumnDescriptionAr,
            //    StatusID=item.StatusID,
            //    imagepath=item.imagepath
            //}).ToList();

            dynamic obj = new ExpandoObject();
            if (outputValues.TryGetValue("prowcount", out var totalRecordsObj) && totalRecordsObj is int totalRecords)
            {
                var totalPages = ((double)totalRecords / (double)filter.Offset);
                int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

                obj.totalPages = roundedTotalPages;
                obj.result = result;
                obj.pageIndex = filter.PageIndex;
                obj.offset = filter.Offset;
            }
          

            
            // return result.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);
            return obj;
        }

        private static IQueryable<EmployeeVacation> ApplyFilter(IQueryable<EmployeeVacation> query, EmployeeVacationFilter criteria)
        {
            if (criteria == null)
                return query;

            var parameter = Expression.Parameter(typeof(EmployeeVacation), "e");
            Expression combinedExpression = null;
            if (criteria.EmployeeID != null)
            {
                var employeeIdExpression = Expression.Equal(
                    Expression.Property(parameter, "EmployeeID"),
                    Expression.Constant(criteria.EmployeeID)
                );
                combinedExpression = employeeIdExpression;
            }

            if (criteria.VacationTypeId != null)
            {
                var VacationTypeIdExpression = Expression.Equal(
                    Expression.Property(parameter, "VacationTypeId"),
                    Expression.Constant(criteria.VacationTypeId, typeof(int?))
                );
                combinedExpression = combinedExpression == null
                    ? VacationTypeIdExpression
                    : Expression.AndAlso(combinedExpression, VacationTypeIdExpression);
            }

            if (criteria.FromDate != null )
            {
                var fromDateExpression = Expression.GreaterThanOrEqual(
                    Expression.Property(parameter, "FromDate"),
                    Expression.Constant(criteria.FromDate.DateToIntValue(), typeof(int?))
                );
                combinedExpression = combinedExpression == null
                    ? fromDateExpression
                    : Expression.AndAlso(combinedExpression, fromDateExpression);
            }

            if (criteria.ToDate != null)
            {
                var toDateExpression = Expression.LessThanOrEqual(
                    Expression.Property(parameter, "ToDate"),
                    Expression.Constant(criteria.ToDate.DateToIntValue(), typeof(int?))
                );
                combinedExpression = combinedExpression == null
                    ? toDateExpression
                    : Expression.AndAlso(combinedExpression, toDateExpression);
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<EmployeeVacation, bool>>(combinedExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        public async Task Create(EmployeeVacationInput model)
        {

            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
            if (model == null)
                throw new NotFoundException("recieved data is missed");
            if (model.FromDate.DateToIntValue() > model.ToDate.DateToIntValue())
                throw new BadRequestException("تاريخ بداية الاجازة لابد ان يكون اصغر من تاريخ نهايةالاجازة ");
            //var canAdd = _unitOfWork.EmployeeVacationRepository.Query(v=>v.ProjectID==_projecId && v.EmployeeID==model.EmployeeID  && model.ToDate.DateToIntValue()<= v.ToDate && model.FromDate.DateToIntValue()>= v.FromDate ).CountAsync();
            //if (await canAdd > 0)
            //    throw new BadRequestException("يوجد اجازة فى هذه الفترة");
            
            string checkValidation =await checkValidationOfVacation(model);//validation confilct vacations 
            if (!string.IsNullOrEmpty(checkValidation))
            {
                throw new BadRequestException(checkValidation);
            }

            DateTime? startDate = (DateTime)model.FromDate;
            DateTime? endDate   = (DateTime)model.ToDate;
            TimeSpan dayCount  = endDate.Value.Subtract(startDate.Value);
            int daysDifference = dayCount.Days;
            model.DayCount     = daysDifference+1;

            var timing = GetVacationTimingInputs(model);

            model.FromDate = null;
            model.ToDate   = null;

            var employeeVacation = _mapper.Map<EmployeeVacationInput, EmployeeVacation>(model);

            employeeVacation.FromDate     = startDate.DateToIntValue();// timing.FromDate;
            employeeVacation.ToDate       = endDate.DateToIntValue();//timing.ToDate;

            await _unitOfWork.EmployeeVacationRepository.PInsertAsync(employeeVacation);

             await _unitOfWork.SaveAsync();
            var insertedPKValue = employeeVacation.EmployeeVacationID;
            //update img path 
            if(model.File is not null)
            {
                var fileExtension = Path.GetExtension(model.File.FileName);
                var settingResult = await _lookupsService.GetSettings();
                var projectPath = settingResult.AttachementPath;
                var fileName = "01" + model.EmployeeID.ToString().PadLeft(6, '0') + insertedPKValue.ToString().PadLeft(6, '0') + fileExtension;
                var filePath = projectPath + fileName;
                //save img path to database
                employeeVacation.imagepath = filePath;
                await _unitOfWork.EmployeeVacationRepository.PUpdateAsync(employeeVacation);
                await _unitOfWork.SaveAsync();
                using (var fileStream = model.File.OpenReadStream())
                {
                    string ftpUrl = filePath;                   
                    string userName = settingResult.WindowsUserName; 
                    string password = settingResult.WindowsUserPassword;
                    bool IsComplete = PublicHelper.UploadFileToFtp(ftpUrl, userName, password, fileStream, fileName);
                }
            }
            await sendToNotification(employeeVacation.EmployeeID, insertedPKValue);
        }
        private async Task<string> checkValidationOfVacation(dynamic model)
        {            
               
                DateTime? FromDate = model.FromDate;
                DateTime? ToDate = model.ToDate;
                var inputParams = new Dictionary<string, object>
            {
                {"pEmployeeVacationID", model.ID},
                {"pEmployeeID", model.EmployeeID==0 ? null:model.EmployeeID},
                {"pVacationTypeID", model.VacationTypeID},
                {"pFromDate", FromDate==null?null: FromDate.DateToIntValue()},
                {"pToDate", ToDate==null?null:ToDate.DateToIntValue()},
                {"pProjectId",_projectProvider.GetProjectId() }
            };
                var outParams = new Dictionary<string, object>
            {

                {"pError","int" }
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.CheckEmployeeVacation", inputParams, outParams);

            //check if user not HR return -3 you have no permission
            if (outputValues.TryGetValue("pError", out var value))
            {
                if (Convert.ToInt32(value) == -5)
                {
                    throw new UnauthorizedAccessException("CannotAddVacationInClosedYear");
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
                ApprovalPageID = 1,
                PrevilageType = _authService.GetUserType(_userId, employeeId)
            };
            await _iNotificationsService.AcceptOrRejectNotificationsAsync(model);
        }
        public async Task Update(EmployeeVacationsUpdate employeeVacation)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
            var vacation = _unitOfWork.EmployeeVacationRepository.Get(emp => emp.EmployeeVacationID == employeeVacation.ID)
                .FirstOrDefault();

            if (vacation is null)
                throw new NotFoundException("Data Not Found");
            if (employeeVacation.FromDate.DateToIntValue() > employeeVacation.ToDate.DateToIntValue())
                throw new BadRequestException("تاريخ بداية الاجازة لابد ان يكون اصغر من تاريخ نهايةالاجازة ");
            
            //var canAdd = _unitOfWork.EmployeeVacationRepository.Query(v => v.ProjectID == _projecId && v.EmployeeID == employeeVacation.EmployeeID && employeeVacation.ToDate.DateToIntValue() <= v.ToDate && employeeVacation.FromDate.DateToIntValue() >= v.FromDate && v.EmployeeVacationID != employeeVacation.ID).CountAsync();
            //if (await canAdd > 0)
            //    throw new BadRequestException("يوجد اجازة فى هذه الفترة");

            string checkValidation =await checkValidationOfVacation(employeeVacation);//validation confilct vacations 
            if (!string.IsNullOrEmpty(checkValidation))
            {
                throw new BadRequestException(checkValidation);
            }

            DateTime startDate        = (DateTime)employeeVacation.FromDate;
            DateTime endDate          = (DateTime)employeeVacation.ToDate;
            TimeSpan dayCount         = endDate.Subtract(startDate);
            vacation.DayCount = dayCount.Days+1;

            var timing = GetVacationTimingInputs(employeeVacation);

            vacation.FromDate         = employeeVacation.FromDate.DateToIntValue();
            vacation.ToDate           = employeeVacation.ToDate.DateToIntValue();
            vacation.VacationTypeID = employeeVacation.VacationTypeID;
            vacation.Notes = employeeVacation.Notes;
            //update img path 
            if (employeeVacation.File is not null)
            {
                var fileExtension = Path.GetExtension(employeeVacation.File.FileName);
                var settingResult = await _lookupsService.GetSettings();
                var projectPath = settingResult.AttachementPath;
                var fileName = "01" + employeeVacation.EmployeeID.ToString().PadLeft(6, '0') + employeeVacation.ID.ToString().PadLeft(6, '0') + fileExtension;
                var filePath = projectPath + fileName;
                //save img path to database
                vacation.imagepath = filePath;
                
                using (var fileStream = employeeVacation.File.OpenReadStream())
                {
                    string ftpUrl = filePath;
                    string userName = settingResult.WindowsUserName;
                    string password = settingResult.WindowsUserPassword;
                    bool IsComplete = PublicHelper.UploadFileToFtp(ftpUrl, userName, password, fileStream, fileName);
                }
            }


            await _unitOfWork.EmployeeVacationRepository.UpdateAsync(vacation);

            await _unitOfWork.SaveAsync();

        }
    
        public async Task Delete( int employeeVacationId)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
            var Vacation = _unitOfWork.EmployeeVacationRepository
                        .Get(e => e.EmployeeVacationID == employeeVacationId)
                        .FirstOrDefault();

            if (Vacation is null)
                throw new NotFoundException("Data Not Found");

            _unitOfWork.EmployeeVacationRepository.Delete(Vacation);

            await _unitOfWork.SaveAsync();

        }

        private (int? FromDate, int? ToDate) GetVacationTimingInputs(EmployeeVacationInput model)
        {
            return (
                   FromDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp(),
                   ToDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp()
                   );
        }

        private (int? FromDate, int? ToDate) GetVacationTimingInputs(EmployeeVacationsUpdate model)
        {
            return (
                   FromDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp(),
                   ToDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp()
                   );
        }
        #region العطل الرسمية 
        public async Task<int> DeleteOfficialVacation(DeleteOfficialVacation deleteOfficialVacation)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                {"pHolidayID", deleteOfficialVacation.HolidayId},
                {"pProjectID", _projectProvider.GetProjectId()},

            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                { "pError","int" },

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteHoliday", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];


            return result;
        }

        public async Task<int> SaveOfficialVacation(OfficialVacationSaveData officialVacationSaveData)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                {"pHolidayTypeID", officialVacationSaveData.HolidayTypeID},
                {"pProjectID", _projectProvider.GetProjectId()},
                {"pNotes",officialVacationSaveData.Notes??Convert.DBNull },
                {"pCreatedBy", _projectProvider.UserId()},
                {"pFromDate",officialVacationSaveData.FromDate !=null? officialVacationSaveData.FromDate.DateToIntValue():Convert.DBNull},
                {"pToDate",officialVacationSaveData.ToDate !=null? officialVacationSaveData.ToDate.DateToIntValue():Convert.DBNull}

            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {
                {"pHolidayID",officialVacationSaveData.HolidayID.HasValue && officialVacationSaveData.HolidayID.Value>0?officialVacationSaveData.HolidayID.Value:"int" },
                {"pError","int" },

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveHoliday", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];


            return result;
        }

        public async Task<object> GetOfficialVacation(OfficialVacationGetInput officialVacationGetInput)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "pHolidayID", officialVacationGetInput.HolidayID??Convert.DBNull },
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pFromDate", officialVacationGetInput.FromDate!=null?officialVacationGetInput.FromDate.DateToIntValue():Convert.DBNull },
                { "pToDate", officialVacationGetInput.ToDate!=null?officialVacationGetInput.ToDate.DateToIntValue():Convert.DBNull },
                { "pLoginUserID", _projectProvider.UserId() },
                { "pPageNo", officialVacationGetInput.PageNo },
                { "pPageSize", officialVacationGetInput.PageSize },
           
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "prowcount","int"}
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<OfficialVacationGetOutput>("dbo.GetHoliday", inputParams, outputParams);
            return PublicHelper.CreateResultPaginationObject(officialVacationGetInput, result, outputValues);
        }
        #endregion
    }
}
