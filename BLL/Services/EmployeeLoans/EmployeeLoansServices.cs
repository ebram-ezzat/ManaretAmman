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
using DataAccessLayer.DTO.EmployeeLoans;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Identity;
using DataAccessLayer.Models;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;

namespace BusinessLogicLayer.Services.EmployeeLoans
{
    internal class EmployeeLoansService : IEmployeeLoansService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILookupsService _lookupsService;
        private readonly IMapper _mapper;
        readonly IProjectProvider _projectProvider;
        readonly IAuthService _authService;
        readonly INotificationsService _iNotificationsService;
        readonly int _userId;
        readonly int _projecId;
        readonly DataAccessLayer.Models.PayrolLogOnlyContext _payrolLogOnlyContext;
        public EmployeeLoansService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper, IProjectProvider projectProvider, IAuthService authService, INotificationsService iNotificationsService, DataAccessLayer.Models.PayrolLogOnlyContext payrolLogOnlyContext)
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
        public async Task<EmployeeLoansOutput> Get(int id)
        {
            var Loan = _unitOfWork.EmployeeLoanRepository
                       .PQuery(e => e.EmployeeLoanID == id, include: e => e.Employee)
                       .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("data not found");


            var result = new EmployeeLoansOutput
            {
                ID = Loan.EmployeeLoanID,
                EmployeeID = Loan.Employee.EmployeeID,
                EmployeeName = Loan.Employee.EmployeeName,
                LoanDate = Loan.LoanDate.IntToDateValue(),
                LoanAmount = Loan.LoanAmount,
                ProjectID = Loan.ProjectID,
                LoantypeId=Loan.loantypeid,
                loantypeAr = Loan.loantypeid is not null ? Constants.GetEmployeeLoanDictionary[Loan.loantypeid.Value].NameAr : null,
                loantypeEn = Loan.loantypeid is not null ? Constants.GetEmployeeLoanDictionary[Loan.loantypeid.Value].NameEn : null
            };

            return result;
        }

        public async Task<dynamic> GetPage(PaginationFilter<EmployeeLoanFilter> filter)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
            //int? employeeId = _authService.IsHr(_userId);

            //var query = from e in _unitOfWork.EmployeeRepository.PQuery()
            //            join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
            //            from lt in ltGroup.DefaultIfEmpty()
            //            join el in _unitOfWork.EmployeeLoanRepository.PQuery() on e.EmployeeID equals el.EmployeeID
            //            where (lt.TableName == "Department" && lt.ColumnName == "DepartmentID") && e.ProjectID == _projecId && lt.ProjectID == _projecId && el.ProjectID == _projecId && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId == null)
            //            select new EmployeeLoan
            //            {
            //                Employee = e,
            //                EmployeeID = e.EmployeeID,
            //                ApprovalStatusID = el.ApprovalStatusID,
            //                EmployeeLoanID = el.EmployeeLoanID,
            //                loantypeid = el.loantypeid,
            //                ProjectID = el.ProjectID,
            //                LoanDate = el.LoanDate,
            //                LoanAmount=el.LoanAmount,
            //                Notes= el.Notes,
            //                StatusID=el.StatusID

            //            };


            //if (filter.FilterCriteria != null)
            //    query= ApplyFilter(query, filter.FilterCriteria);

            //var totalRecords = await query.CountAsync();

            //var Loans = await query.Skip((filter.PageIndex - 1) * filter.Offset)
            //            .Take(filter.Offset).ToListAsync();

            ////var lookups = await _lookupsService.GetLookups(Constants.EmployeeLoans, Constants.LoanTypeID);
            //var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

            //var result = Loans.Select(item => new EmployeeLoansOutput
            //{
            //    ID             = item.EmployeeLoanID,
            //    EmployeeID     = item.Employee.EmployeeID,
            //    EmployeeName   = item.Employee.EmployeeName,
            //    LoanDate       = item.LoanDate.IntToDateValue(),
            //    LoanAmount     = item.LoanAmount  ,
            //    ProjectID = item.ProjectID,
            //    LoantypeId = item.loantypeid,
            //    loantypeAr = item.loantypeid is not null? Constants.GetEmployeeLoanDictionary[item.loantypeid.Value].NameAr:null,
            //    loantypeEn = item.loantypeid is not null ? Constants.GetEmployeeLoanDictionary[item.loantypeid.Value].NameEn : null,
            //    ApprovalStatus = approvals.FirstOrDefault(e => e.ColumnValue == item.ApprovalStatusID.ToString())?.ColumnDescriptionAr,
            //    Notes=item.Notes,
            //    StatusID=item.StatusID
            //}).ToList();

            //return result.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);

            var inputParams = new Dictionary<string, object>()
            {
                
                //{"pEmployeeLoanID",filter.FilterCriteria.EmployeeLoanID },
                {"pEmployeeID",filter.FilterCriteria.EmployeeID },
                {"pProjectID",_projectProvider.GetProjectId()},
                {"pFromDate",filter.FilterCriteria.FromDate!=null?filter.FilterCriteria.FromDate.DateToIntValue():Convert.DBNull},
                {"pToDate", filter.FilterCriteria.ToDate!=null ?filter.FilterCriteria.ToDate.DateToIntValue():Convert.DBNull },
                {"pLanguageID",_projectProvider.LangId() },
                {"pLoanTypeID",1 /*filter.FilterCriteria.LoanTypeId??Convert.DBNull*/ },
                {"pFlag",1 },
                {"pLoginUserID",_projectProvider.UserId()},
                
                {"pPageNo",filter.PageIndex },
                {"pPageSize", filter.Offset},
                

            };
            var outputParams = new Dictionary<string, object>() { { "prowcount", "int" } };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<EmployeeLoanResult>("dbo.GetEmployeeLoan", inputParams, outputParams);
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

            return obj;
        }

        private static IQueryable<EmployeeLoan> ApplyFilter(IQueryable<EmployeeLoan>  query, EmployeeLoanFilter criteria)
        {

            if (criteria == null)
                return query;

            var parameter = Expression.Parameter(typeof(EmployeeLoan), "e");
            Expression combinedExpression = null;
            if (criteria.EmployeeID != null)
            {
                var employeeIdExpression = Expression.Equal(
                    Expression.Property(parameter, "EmployeeID"),
                    Expression.Constant(criteria.EmployeeID)
                );
                combinedExpression = employeeIdExpression;
            }
            if (criteria.LoanTypeId != null)
            {
                var LoanTypeIdExpression = Expression.Equal(
                    Expression.Property(parameter, "loantypeid"),
                    Expression.Constant(criteria.LoanTypeId, typeof(int?))
                );
                combinedExpression = combinedExpression == null
                   ? LoanTypeIdExpression
                   : Expression.AndAlso(combinedExpression, LoanTypeIdExpression);
            }
            if (criteria.FromDate != null)
            {
                var LoanDateFromExpression = Expression.GreaterThanOrEqual(
                    Expression.Property(parameter, "LoanDate"),
                    Expression.Constant(criteria.FromDate.DateToIntValue(), typeof(int?))
                );
                combinedExpression = combinedExpression == null
                                   ? LoanDateFromExpression
                                   : Expression.AndAlso(combinedExpression, LoanDateFromExpression);
            }
            if (criteria.ToDate != null)
            {
                var LoanDateToExpression = Expression.LessThanOrEqual(
                    Expression.Property(parameter, "LoanDate"),
                    Expression.Constant(criteria.ToDate.DateToIntValue(), typeof(int?))
                );
                combinedExpression = combinedExpression == null
                                   ? LoanDateToExpression
                                   : Expression.AndAlso(combinedExpression, LoanDateToExpression);
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<EmployeeLoan, bool>>(combinedExpression, parameter);
                query = query.Where(lambda);
            }
            return query; 
        }

        public async Task<int> Create(EmployeeLoansInput model)
        {

            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

            if (model == null)
                throw new NotFoundException("recieved data is missed");

            //var LoanDate = model.LoanDate.DateToIntValue();

            //model.LoanDate = null;

            //var employeeLoan = _mapper.Map<EmployeeLoan>(model);

            //employeeLoan.LoanDate    = LoanDate;

            //await _unitOfWork.EmployeeLoanRepository.PInsertAsync(employeeLoan);

            // await _unitOfWork.SaveAsync();
            //var insertedPKValue = employeeLoan.EmployeeLoanID;
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", model.EmployeeID },
                { "pLoanDate", model.LoanDate.DateToIntValue() },
                { "pLoanAmount", model.LoanAmount},
                { "pNotes",model.Notes},
                { "pCreatedBy",_projectProvider.UserId()},
                { "pProjectID",_projectProvider.GetProjectId()},
                { "ploantypeid",1},
                { "pIsFirstLoan",1},
                { "pIsPaid",0},
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                { "pEmployeeLoanID",model.ID },
                { "pError","int" },

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeLoan", inputParams, outputParams);
            await sendToNotification(model.EmployeeID, result);
            return (int)result;
            
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
                ApprovalPageID = 3,
                PrevilageType = _authService.GetUserType(_userId, employeeId)
        };
            await _iNotificationsService.AcceptOrRejectNotificationsAsync(model);
        }


        public async Task<int> Update(EmployeeLoansUpdate employeeLoan)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

            var Loan = _unitOfWork.EmployeeLoanRepository.Get(emp => emp.EmployeeLoanID == employeeLoan.ID)
                .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("Data Not Found");

            //Loan.LoanDate = employeeLoan.LoanDate.DateToIntValue();
            //Loan.LoanAmount = employeeLoan.LoanAmount;
            //Loan.Notes = employeeLoan.Notes;
            //Loan.loantypeid = employeeLoan.LoantypeId;

            //await _unitOfWork.EmployeeLoanRepository.UpdateAsync(Loan);

            //await _unitOfWork.SaveAsync();

            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", employeeLoan.EmployeeID },
                { "pLoanDate", employeeLoan.LoanDate.DateToIntValue() },
                { "pLoanAmount", employeeLoan.LoanAmount},
                { "pNotes",employeeLoan.Notes},
                { "pCreatedBy",_projectProvider.UserId()},
                { "pProjectID",_projectProvider.GetProjectId()},
                { "ploantypeid",1},
                { "pIsFirstLoan",1},
                { "pIsPaid",0},
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                { "pEmployeeLoanID",employeeLoan.ID },
                { "pError","int" },

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeLoan", inputParams, outputParams);
            return (int)result;

        }

        public async Task Delete( int employeeLoanId)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

            var Loan = _unitOfWork.EmployeeLoanRepository
                        .Get(e => e.EmployeeLoanID == employeeLoanId)
                        .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("Data Not Found");

            _unitOfWork.EmployeeLoanRepository.Delete(Loan);

            await _unitOfWork.SaveAsync();

        }

        private  int? GetLoanTimingInputs(DateTime? LoanDate)
        {
            return LoanDate.ConvertFromDateTimeToUnixTimestamp();
               
        }

        public async Task<int> CreateScheduledLoans(SchededuledLoansInput employees)
        {
            if(employees.TotalAmount != employees.EmployeeLoansInputs.Sum(x=>x.LoanAmount))
            {
                throw new UnauthorizedAccessException("TotalAmount not equal the LoanAmount");

            }
            var first = employees.EmployeeLoansInputs.FirstOrDefault();
            int serial = 0;
            if(first is not null)
            {
                Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", employees.EmployeeID },
                { "pLoanDate", first.LoanDate.DateToIntValue() },
                { "pLoanAmount", first.LoanAmount},
                { "pNotes",first.Notes},
                { "pCreatedBy",_projectProvider.UserId()},
                { "pProjectID",_projectProvider.GetProjectId()},
                { "ploantypeid",1},
                { "pIsFirstLoan",1},
                { "pIsPaid",first.IsPaid},
            };
                Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                {"pEmployeeLoanID",first.ID },
                {"pLoanSerial","int" },
                { "pError","int" },

            };
                var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeLoan", inputParams, outputParams);
                if (outputValues.TryGetValue("pLoanSerial", out var value))
                {
                    serial = Convert.ToInt32(value);
                  
                }
            }
            foreach(var employee in employees.EmployeeLoansInputs.Skip(1)) {
                Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", employees.EmployeeID },
                { "pLoanDate", employee.LoanDate.DateToIntValue() },
                { "pLoanAmount",employee.LoanAmount},
                { "pNotes",employee.Notes},
                { "pCreatedBy",_projectProvider.UserId()},
                { "pProjectID",_projectProvider.GetProjectId()},
                { "ploantypeid",1},
                { "pIsFirstLoan",0},
                { "pIsPaid",employee.IsPaid},
            };
                Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                {"pEmployeeLoanID",first.ID },
                {"pLoanSerial",serial},
                { "pError","int" },

            };
                var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeLoan", inputParams, outputParams);
            }
            return serial;
        }

        public async Task<int> UpdateScheduledLoans(SchededuledLoansInput employees)
        {
            if (employees.TotalAmount != employees.EmployeeLoansInputs.Sum(x => x.LoanAmount))
            {
                throw new UnauthorizedAccessException("TotalAmount not equal the LoanAmount");

            }
            foreach (var employee in employees.EmployeeLoansInputs.Skip(1))
            {
                Dictionary<string, object> inputParams = new Dictionary<string, object>
            {                
                { "pLoanDate", employee.LoanDate },
                { "pLoanAmount",employee.LoanAmount},
                { "pNotes",employee.Notes},
                { "pCreatedBy",_projectProvider.UserId()},
                { "pProjectID",_projectProvider.GetProjectId()},               
                { "pIsPaid",employee.IsPaid},
            };
                Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                {"pEmployeeLoanID",employee.ID},                
                { "pError","int" },

            };
                var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeLoan", inputParams, outputParams);
            }
            return 0;
        }

        public async Task<int> DeleteScheduledLoans(DeleteSchededuledLoansInput schededuledLoans)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
            { "pEmployeeLoanID", schededuledLoans.EmployeeLoanID },
            { "pLoanSerial", schededuledLoans.LoanSerial },
            { "pEmployeeID", schededuledLoans.EmployeeID },
            { "pProjectID", _projectProvider.GetProjectId() }

          };


            Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" }, // Assuming the output parameter "pError" is of type int
            
        };

            // Call the ExecuteStoredProcedureAsync function
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeLoan", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];


            return pErrorValue;
        }
        public async Task<dynamic> GetScheduledLoan(EmployeeLoanParameters getEmployeeLoan)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeLoanID", getEmployeeLoan.EmployeeLoanID ?? Convert.DBNull },
            { "pEmployeeID", getEmployeeLoan.EmployeeID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },  // Not nullable, no need for DB null check
            { "pFromDate", getEmployeeLoan.FromDate==null ? Convert.DBNull:getEmployeeLoan.FromDate.DateToIntValue() },
            { "pToDate", getEmployeeLoan.ToDate==null ? Convert.DBNull:getEmployeeLoan.ToDate.DateToIntValue() },
            { "pLoanTypeID", getEmployeeLoan.LoanTypeID ?? Convert.DBNull },
            { "pLanguageID", _projectProvider.LangId() }, // Not nullable, no need for DB null check
            { "pFlag", getEmployeeLoan.Flag },
            {"pCreatedBy", getEmployeeLoan.CreatedBy?? Convert.DBNull},
            {"pLoginUserID",_projectProvider.UserId()==-1?Convert.DBNull:_projectProvider.UserId() },
            {"pPageNo" ,getEmployeeLoan.PageNo},
            {"pPageSize",getEmployeeLoan.PageSize }
        };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            {"prowcount","int" }
        };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<EmployeeLoanResult>("dbo.GetEmployeeLoan", inputParams, outputParams);
            return PublicHelper.CreateResultPaginationObject(getEmployeeLoan, result, outputValues); ;

        }
    }
}
