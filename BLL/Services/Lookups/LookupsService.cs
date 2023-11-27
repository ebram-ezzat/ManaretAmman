using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Dynamic;

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
            string userName = _configuration["UploadServerCredentials:UserName"];
            string password = _configuration["UploadServerCredentials:Password"];
            return await PublicHelper.GetFileAsFormFileByFtpPath(fullPath, userName, password);
        }

        public async Task<object> GetFileBase64ByFtpPath(string fullPath)
        {
            string userName = _configuration["UploadServerCredentials:UserName"];
            string password = _configuration["UploadServerCredentials:Password"];
            return await PublicHelper.GetFileBase64ByFtpPath(fullPath, userName, password);
            
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
    }
}
