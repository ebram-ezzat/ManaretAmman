using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Lookup;
using DataAccessLayer.DTO.WorkFlow;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;

namespace BusinessLogicLayer.Services.Lookups
{
    public class LookupsService : ILookupsService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IProjectProvider _projectProvider;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public LookupsService(IUnitOfWork unit, IMapper mapper, IConfiguration configuration, IProjectProvider projectProvider, PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _unit = unit;
            _mapper = mapper;
            _configuration = configuration;
            _projectProvider = projectProvider;
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }

       

        public async Task<string> GetDescription(string tableName, string columnName, int columnValue)
        {
            var lookup =  _unit.LookupsRepository
                    .Get(e => e.TableName == tableName 
                    && int.Parse(e.ColumnValue) == columnValue
                    && e.ColumnName == columnName)
                    .FirstOrDefault();

            return lookup.ColumnDescription;
        }

        public async Task<string> GetDescriptionAr(string tableName, string columnName, int columnValue)
        {
            var lookup = _unit.LookupsRepository
                    .Get(e => e.TableName == tableName
                    && int.Parse(e.ColumnValue) == columnValue
                    && e.ColumnName == columnName)
                    .FirstOrDefault();

            return lookup.ColumnDescriptionAr;
        }

        public async Task<IFormFile> GetFileAsFormFileByFtpPath(string fullPath)
        {
            var settings =await GetSettings();
            //string userName = _configuration["UploadServerCredentials:UserName"];
            //string password = _configuration["UploadServerCredentials:Password"];
            return await PublicHelper.GetFileAsFormFileByFtpPath(fullPath, settings?.WindowsUserName, settings?.WindowsUserPassword);
        }

        public async Task<object> GetFileBase64ByFtpPath(string fullPath)
        {
            var settings = await GetSettings();
            //string userName = _configuration["UploadServerCredentials:UserName"];
            //string password = _configuration["UploadServerCredentials:Password"];
            return await PublicHelper.GetFileBase64ByFtpPath(fullPath, settings?.WindowsUserName, settings?.WindowsUserPassword);
            
        }

        public async Task<IList<LookupDto>> GetLookups(string tableName, string columnName)
        {
            if (tableName == "Loans")
            {
                var loanLookup=new List<LookupDto>();
               
               await Task.Run(() => {
                   foreach (var key in Constants.GetEmployeeLoanDictionary.Keys)
                   {
                       loanLookup.Add(new LookupDto
                       {
                           TableName = tableName,
                           ColumnValue = key.ToString(),
                           ColumnDescription = Constants.GetEmployeeLoanDictionary[key].NameEn,
                           ColumnDescriptionAr = Constants.GetEmployeeLoanDictionary[key].NameAr,
                           ID = key,
                           ColumnName = "LoantypeId"
                       });
                   }
               });
                return loanLookup;    

            }
            if (columnName == null)
                columnName = string.Empty;

            var lookups =await  _unit.LookupsRepository
                    .PQuery(e => e.TableName == tableName && e.ColumnName == columnName)
                    .ToListAsync();

            return _mapper.Map<IList<LookupDto>>(lookups);
        }

        public async Task<GetSettingsResult> GetSettings(int Falg = 1)
        {
            var parameters = new Dictionary<string, object>
             {                
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pFlag" , Falg }
             };
            var (settingsResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetSettingsResult>("dbo.GetSettings", parameters, null);

            return settingsResponse.FirstOrDefault();
        }

        public async Task<List<EmployeeShiftDTO>> GetShifts()
        {
            var parameters = new Dictionary<string, object>
             {
                { "pShiftID" , null },
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pSearch" , null }
             };
            var (employeePapersResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<EmployeeShiftDTO>("[dbo].[GetShifts]", parameters, null);

            return employeePapersResponse;
        }

        public async Task<int> InsertLookup(InsertLookup insertLookup)
        {
            bool isUpdating = insertLookup.ID.HasValue && insertLookup.ID.Value > 0;

            var inputParams = new Dictionary<string, object>
            {
                {"pTableName",insertLookup.TableName },
                {"pColumnName",insertLookup.ColumnName },
                {"pColumnValue",insertLookup.ColumnValue },
                {"pColumnDescription",insertLookup.ColumnDescription },
                {"pColumnDescriptionAR",insertLookup.ColumnDescriptionAR },
                {"pOrderBy",insertLookup.OrderBy },
                {"pProjectID",insertLookup.ProjectID },
                {"pBalance", Convert.DBNull},
                {"pDefaultValue", Convert.DBNull},
                {"pParentID", Convert.DBNull},
                {"pEmployeeID", Convert.DBNull},
                {"pCalWithHoliday", Convert.DBNull},
                {"pIsHealthVacation", Convert.DBNull},
                {"pIsInjuryVacation", Convert.DBNull},
                {"pApprovalProcessID", Convert.DBNull},
                {"pFirstPeriod", Convert.DBNull},
                {"pSecondPeriod", Convert.DBNull},
                {"pThirdPeriod", Convert.DBNull},
                {"pFourthPeriod", Convert.DBNull},
                {"pFifthPeriod", Convert.DBNull},
                {"pPenaltyCategoryID2", Convert.DBNull},
                {"pPenaltyCategoryID3", Convert.DBNull},
                {"pPenaltyCategoryID4", Convert.DBNull},
                {"pPenaltyCategoryID5", Convert.DBNull},
                {"pIsWithoutSalaryVacation", Convert.DBNull},
                {"pIsPersonalVacation", Convert.DBNull},
                {"pWithoutSalaryVacationValue", Convert.DBNull},
                {"pisOtherVacation", Convert.DBNull}
            };
            var outParams = new Dictionary<string, object>
            {
                {"pID",isUpdating?insertLookup.ID:"int"},
                {"pError","int"}
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertLookupTable", inputParams, outParams);

            // Retrieve the output parameter value
            int pErrorValue = (int)outputValues["pError"];



            return pErrorValue;
        }
        public async Task<int> DeleteLookup(DeleteLookup deleteLookup)
        {
            var inputParams = new Dictionary<string, object>
            {
                {"pID",deleteLookup.ID },
                {"pProjectID",deleteLookup.ProjectID }
            };
            var outParams = new Dictionary<string, object>
            {
               
                {"pError","int"}
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteLookupTable", inputParams, outParams);

            // Retrieve the output parameter value
            int pErrorValue = (int)outputValues["pError"];



            return pErrorValue;
        }

        public async Task<List<GetTableNamesWithColumnNames>> GetTableNamesColumnNamesByProjectId(GetTableAndColumnOfProject getTableAndColumnOfProject)
        {
            var inputParams = new Dictionary<string, object>
            {
              
                {"pProjectID",getTableAndColumnOfProject.ProjectID }
            };
           
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetTableNamesWithColumnNames>("dbo.GetTableNameAndColumnName", inputParams, null);
            



            return result;
        }

        public async Task<List<GetLookupTableDataOutput>> GetLookupData(GetLookupTableDataInput getLookupTableDataInput)
        {
            var inputParam = new Dictionary<string, object>
            {
                {"pID",getLookupTableDataInput.ID??Convert.DBNull },
                {"pProjectID",getLookupTableDataInput.ProjectID??Convert.DBNull},
                {"pParentID", getLookupTableDataInput.ParentID??Convert.DBNull},
                {"pTableName", getLookupTableDataInput.TableName??Convert.DBNull},
                {"pColumnName", getLookupTableDataInput.ColumnName??Convert.DBNull},
                {"pLanguageID", _projectProvider.LangId()},
                {"pApprovalPageID", Convert.DBNull},

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetLookupTableDataOutput>("dbo.GetLookupTableData", inputParam, null);
            return result; 
        }

        public async Task<GetMobileVersionOutput> GetMobileVersion()
        {
            return _unit.MobileVersionRepository
                    .Get()
                    .Select(x=>new GetMobileVersionOutput() 
                    {
                        DurationUntilAlertAgaint = x.DurationUntilAlertAgaint,
                        MinAppVersion = x.MinAppVersion,
                        ShowIgnore = x.ShowIgnore,
                        ShowLater = x.ShowLater,
                    })
                    .FirstOrDefault();
        }
    }
}
