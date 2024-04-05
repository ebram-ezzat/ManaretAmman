using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.DTO.WorkFlow;
using DataAccessLayer.Models;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.WorkFlow
{
    public class WorkFlow : IWorkFlow
    {
        private PayrolLogOnlyContext _payrolLogOnlyContext;
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
        public WorkFlow(PayrolLogOnlyContext payrolLogOnlyContext, IProjectProvider projectProvider, ILookupsService lookupsService)
        {
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _projectProvider = projectProvider;
            _lookupsService = lookupsService;
        }
        #region WorkFlowHeader Screen1
        public async Task<int> InsertOrUpdateWorkFlowHeader(InsertOrUpdateWorkFlowHeader insertOrUpdateWorkFlowHeader)
        {
            bool isUpdating = insertOrUpdateWorkFlowHeader.WorkflowHeaderID.HasValue && insertOrUpdateWorkFlowHeader.WorkflowHeaderID > 0;

            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkflowTypeID",insertOrUpdateWorkFlowHeader.WorkflowTypeID},
                {"pTypeID",insertOrUpdateWorkFlowHeader.TypeID},
                {"pCreatedBy",isUpdating? Convert.DBNull:_projectProvider.UserId()},              
                {"pModifiedBy",isUpdating?_projectProvider.UserId():Convert.DBNull},
                {"pProjectID",_projectProvider.GetProjectId() }
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                {"pWorkflowHeaderID",isUpdating? insertOrUpdateWorkFlowHeader.WorkflowHeaderID.Value:"int"},//Input output direction
                { "pError","int" }
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertOrUpdateWorkFlowHeader", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<int> DeleteWorkFlowHeader(DeleteWorkFlowHeader deleteWorkFlowHeader)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkflowHeaderID",deleteWorkFlowHeader.WorkflowHeaderID},
                {"pProjectID",_projectProvider.GetProjectId() }
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteWorkFlowHeader", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<List<GetWorkFlowHeaderOutput>> GetWorkFlowHeader(GetWorkFlowHeaderInput getWorkFlowHeaderInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkflowHeaderID",getWorkFlowHeaderInput.WorkflowHeaderID??Convert.DBNull},
                {"pWorkflowTypeID",getWorkFlowHeaderInput.WorkflowTypeID??Convert.DBNull},
                {"pTypeID",getWorkFlowHeaderInput.TypeID?? Convert.DBNull},
                {"pCreatedBy",Convert.DBNull},
                {"pModifiedBy",Convert.DBNull },
                {"pProjectID",_projectProvider.GetProjectId() },
                {"pModificationDate",Convert.DBNull},
                {"pCreationDate", Convert.DBNull},
                {"pLangID", _projectProvider.LangId()}

            };
          
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetWorkFlowHeaderOutput>("dbo.GetWorkFlowHeader", inputParams, null);
            return result;
        }


        #endregion

        #region WorkFlowStep Screen2
        public async Task<int> InsertOrUpdateWorkFlowStep(InsertOrUpdateWorkFlowStep insertOrUpdateWorkFlowStep)
        {
            bool isUpdating = insertOrUpdateWorkFlowStep.WorkFlowStepID.HasValue && insertOrUpdateWorkFlowStep.WorkFlowStepID > 0;

            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkflowHeaderID",insertOrUpdateWorkFlowStep.WorkflowHeaderID},
                {"pUserTypeID",insertOrUpdateWorkFlowStep.UserTypeID},
                {"pCanEdit",insertOrUpdateWorkFlowStep.CanEdit},
                {"pCanAdd",insertOrUpdateWorkFlowStep.CanAdd},
                {"pCanDelete",insertOrUpdateWorkFlowStep.CanDelete},
                {"pAcceptStatusID",insertOrUpdateWorkFlowStep.AcceptStatusID},
                {"pRejectStatusID",insertOrUpdateWorkFlowStep.RejectStatusID},
                {"pCreatedBy",isUpdating? Convert.DBNull:_projectProvider.UserId()},
                {"pModifiedBy",isUpdating?_projectProvider.UserId():Convert.DBNull},
               // {"pProjectID",_projectProvider.GetProjectId() }
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                {"pWorkFlowStepID",isUpdating? insertOrUpdateWorkFlowStep.WorkFlowStepID.Value:"int"},//Input output direction
                { "pError","int" }
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertUpdateWorkFlowStep", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<int> DeleteWorkFlowStep(DeleteWorkFlowStep deleteWorkFlowStep)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkFlowStepID",deleteWorkFlowStep.WorkFlowStepID}
                
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteWorkFlowStep", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<List<GetWorkFlowStepOutput>> GetWorkFlowStep(GetWorkFlowStepInput getWorkFlowHeaderStep)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkFlowStepID", getWorkFlowHeaderStep.WorkFlowStepID ?? Convert.DBNull},
                {"pWorkflowHeaderID", getWorkFlowHeaderStep.WorkflowHeaderID ?? Convert.DBNull},
                {"pUserTypeID", getWorkFlowHeaderStep.UserTypeID ?? Convert.DBNull},
                {"pProjectID",_projectProvider.GetProjectId()},
                {"pLangID",_projectProvider.LangId()},
                {"pCanEdit", getWorkFlowHeaderStep.CanEdit ?? Convert.DBNull},
                {"pCanAdd", getWorkFlowHeaderStep.CanAdd ?? Convert.DBNull},
                {"pCanDelete", getWorkFlowHeaderStep.CanDelete ?? Convert.DBNull},
                {"pAcceptStatusID", getWorkFlowHeaderStep.AcceptStatusID ?? Convert.DBNull},
                {"pRejectStatusID", getWorkFlowHeaderStep.RejectStatusID ?? Convert.DBNull},
                {"pModificationDate", Convert.DBNull},
                {"pCreationDate",Convert.DBNull},
                {"pCreatedBy", Convert.DBNull},
                {"pModifiedBy",  Convert.DBNull}
            };
           
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetWorkFlowStepOutput>("dbo.GetWorkFlowStepWithFilters", inputParams, null);
            return result;
        }


        #endregion

        #region WorkFlowNotificationSteps Screen3
        public async Task<int> InsertOrUpdateWorkFlowNotificationStep(InsertOrUpdateWorkFlowNotificationStep insertOrUpdateWorkFlowNotificationStep)
        {
            bool isUpdating = insertOrUpdateWorkFlowNotificationStep.WorkFlowNotificationID.HasValue && insertOrUpdateWorkFlowNotificationStep.WorkFlowNotificationID.Value > 0;

            // Prepare input parameters
            var inputParams = new Dictionary<string, object>()
            {               
                {"pWorkFlowStepID", insertOrUpdateWorkFlowNotificationStep.WorkFlowStepID },               
                {"pUserTypeID", insertOrUpdateWorkFlowNotificationStep.UserTypeID },
                {"pNotificationDetail", insertOrUpdateWorkFlowNotificationStep.NotificationDetail??Convert.DBNull},
                {"pNotificationDetailAr", insertOrUpdateWorkFlowNotificationStep.NotificationDetailAr??Convert.DBNull},
                {"pCreatedBy", isUpdating ? Convert.DBNull : _projectProvider.UserId()},
                {"pModifiedBy", isUpdating ? _projectProvider.UserId() : Convert.DBNull},
        
            };

            // Prepare output parameters
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                {"pWorkFlowNotificationID", isUpdating ? insertOrUpdateWorkFlowNotificationStep.WorkFlowNotificationID.Value:"int"}, // Output parameter for insert (to retrieve the new ID)
                {"pError", "int"} // Assuming error code 0 for success, non-zero for errors
            };

            // Execute the stored procedure asynchronously
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertUpdateWorkFlowNotificationStep", inputParams, outputParams);

            // Retrieve the output parameter value
            int pErrorValue = (int)outputValues["pError"];

           

            return pErrorValue;

        }

        public async Task<int> DeleteWorkFlowNotificationStep(DeleteWorkFlowNotificationStep deleteWorkFlowNotificationStep)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkFlowNotificationID",deleteWorkFlowNotificationStep.WorkFlowNotificationID}

            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteWorkFlowNotificationStep", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<List<GetWorkFlowNotificationStepOutput>> GetWorkFlowNotificationStep(GetWorkFlowNotificationStepInput getWorkFlowNotificationStepInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkFlowNotificationID", getWorkFlowNotificationStepInput.WorkFlowNotificationID ?? Convert.DBNull},
                {"pWorkFlowStepID", getWorkFlowNotificationStepInput.WorkFlowStepID ?? Convert.DBNull},
                {"pUserTypeID", getWorkFlowNotificationStepInput.UserTypeID ?? Convert.DBNull},
                {"pNotificationDetail", getWorkFlowNotificationStepInput.NotificationDetail ?? Convert.DBNull},
                {"pNotificationDetailAr", getWorkFlowNotificationStepInput.NotificationDetailAr ?? Convert.DBNull},
                {"pLangID",_projectProvider.LangId() },
                {"pProjectID",_projectProvider.GetProjectId() },
                {"pCreatedBy",  Convert.DBNull},
                {"pModifiedBy", Convert.DBNull},
                {"pCreationDate",  Convert.DBNull},
                {"pModificationDate", Convert.DBNull},
               
                
            };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetWorkFlowNotificationStepOutput>("dbo.GetWorkFlowNotificationStep", inputParams, null);
            return result;
        }


        #endregion

        #region WorkFlowNotification Seperate Screen
        public async Task<int> InsertWorkFlowNotification(InsertWorkFlowNotification insertWorkFlowNotification)
        {
            // Prepare input parameters
            var inputParams = new Dictionary<string, object>()
            {
                {"pNotificationSetupID", insertWorkFlowNotification.NotificationSetupID },
                {"pUserTypeID", insertWorkFlowNotification.UserTypeID },
                {"pMessageFormatAr", insertWorkFlowNotification.MessageFormatAr??Convert.DBNull},
                {"pMessageFormatEn", insertWorkFlowNotification.MessageFormatEn??Convert.DBNull},
                {"pIsSMS", insertWorkFlowNotification.IsSMS??Convert.DBNull},
                {"pIsEmail", insertWorkFlowNotification.IsEmail??Convert.DBNull },
                {"pIsWhatsapp", insertWorkFlowNotification.IsWhatsapp??Convert.DBNull},
                {"pIsSystem", insertWorkFlowNotification.IsSystem??Convert.DBNull},
                {"pCreatedBy",  _projectProvider.UserId()}

            };

            // Prepare output parameters
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {               
                {"pError", "int"} // Assuming error code 0 for success, non-zero for errors
            };

            // Execute the stored procedure asynchronously
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertNotificationUserType", inputParams, outputParams);

            // Retrieve the output parameter value
            int pErrorValue = (int)outputValues["pError"];



            return pErrorValue;

        }

        public async Task<int> DeleteWorkFlowNotification(DeleteWorkFlowNotification deleteWorkFlowNotification)
        {
            // Prepare input parameters
            var inputParams = new Dictionary<string, object>()
            {
                {"pNotificationSetupID", deleteWorkFlowNotification.NotificationSetupID },
                {"pUserTypeID", deleteWorkFlowNotification.UserTypeID },
               

            };

            // Prepare output parameters
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                {"pError", "int"} // Assuming error code 0 for success, non-zero for errors
            };

            // Execute the stored procedure asynchronously
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteNotificationUserType", inputParams, outputParams);

            // Retrieve the output parameter value
            int pErrorValue = (int)outputValues["pError"];



            return pErrorValue;

        }

        public async Task<List<GetWorkFlowNotificationOutput>> GetWorkFlowNotification(GetWorkFlowNotificationInput getWorkFlowNotificationInput)
        {
            // Prepare input parameters
            var inputParams = new Dictionary<string, object>()
            {
                {"pNotificationSetupID", getWorkFlowNotificationInput.NotificationSetupID },
                {"pUserTypeID", getWorkFlowNotificationInput.UserTypeID },
                {"pFlag",getWorkFlowNotificationInput.Flag},
                {"pLanguageID",getWorkFlowNotificationInput.LanguageID}

            };

           
            // Execute the stored procedure asynchronously
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetWorkFlowNotificationOutput>("dbo.GetNotificationUserType", inputParams, null);

            
            return result;

        }
        #endregion
    }
}
