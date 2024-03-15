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
                {"pUserID",deleteWorkFlowHeader.WorkflowHeaderID},
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
                {"pCreatedBy",getWorkFlowHeaderInput.CreatedBy?? Convert.DBNull},
                {"pModifiedBy",getWorkFlowHeaderInput.ModifiedBy??Convert.DBNull },
                {"pProjectID",_projectProvider.GetProjectId() },
                {"pModificationDate", getWorkFlowHeaderInput.ModificationDate??Convert.DBNull},
                {"pCreationDate",  getWorkFlowHeaderInput.CreationDate??Convert.DBNull}

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
                {"pProjectID",_projectProvider.GetProjectId() }
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
                {"pCanEdit", getWorkFlowHeaderStep.CanEdit ?? Convert.DBNull},
                {"pCanAdd", getWorkFlowHeaderStep.CanAdd ?? Convert.DBNull},
                {"pCanDelete", getWorkFlowHeaderStep.CanDelete ?? Convert.DBNull},
                {"pAcceptStatusID", getWorkFlowHeaderStep.AcceptStatusID ?? Convert.DBNull},
                {"pRejectStatusID", getWorkFlowHeaderStep.RejectStatusID ?? Convert.DBNull},
                {"pModificationDate", getWorkFlowHeaderStep.ModificationDate ?? Convert.DBNull},
                {"pCreationDate", getWorkFlowHeaderStep ?? Convert.DBNull},
                {"pCreatedBy", getWorkFlowHeaderStep.CreatedBy ?? Convert.DBNull},
                {"pModifiedBy", getWorkFlowHeaderStep.ModifiedBy ?? Convert.DBNull}
            };
           
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetWorkFlowStepOutput>("dbo.GetWorkFlowStepWithFilters", inputParams, null);
            return result;
        }


        #endregion

        #region WorkFlowNotification Screen3
        public async Task<int> InsertOrUpdateWorkFlowNotification(InsertOrUpdateWorkFlowNotification insertOrUpdateWorkFlowNotification)
        {
            bool isUpdating = insertOrUpdateWorkFlowNotification.WorkFlowNotificationID.HasValue && insertOrUpdateWorkFlowNotification.WorkFlowNotificationID.Value > 0;

            // Prepare input parameters
            var inputParams = new Dictionary<string, object>()
            {               
                {"pWorkFlowStepID", insertOrUpdateWorkFlowNotification.WorkFlowStepID },               
                {"pUserTypeID", insertOrUpdateWorkFlowNotification.UserTypeID },
                {"pNotificationDetail", insertOrUpdateWorkFlowNotification.NotificationDetail },
                {"pCreatedBy", isUpdating ? Convert.DBNull : _projectProvider.UserId()},
                {"pModifiedBy", isUpdating ? _projectProvider.UserId() : Convert.DBNull},
        
            };

            // Prepare output parameters
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                {"pWorkFlowNotificationID", isUpdating ? insertOrUpdateWorkFlowNotification.WorkFlowNotificationID.Value:"int"}, // Output parameter for insert (to retrieve the new ID)
                {"pError", "int"} // Assuming error code 0 for success, non-zero for errors
            };

            // Execute the stored procedure asynchronously
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertUpdateWorkFlowNotification", inputParams, outputParams);

            // Retrieve the output parameter value
            int pErrorValue = (int)outputValues["@pError"];

           

            return pErrorValue;

        }

        public async Task<int> DeleteWorkFlowNotification(DeleteWorkFlowNotification deleteWorkFlowNotification)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkFlowNotificationID",deleteWorkFlowNotification.WorkFlowNotificationID}

            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteWorkFlowNotification", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<List<GetWorkFlowNotificationOutput>> GetWorkFlowNotification(GetWorkFlowNotificationInput getWorkFlowNotification)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pWorkFlowNotificationID", getWorkFlowNotification.WorkFlowNotificationID ?? Convert.DBNull},
                {"pWorkFlowStepID", getWorkFlowNotification.WorkFlowStepID ?? Convert.DBNull},
                {"pUserTypeID", getWorkFlowNotification.UserTypeID ?? Convert.DBNull},
                {"pNotificationDetail", getWorkFlowNotification.NotificationDetail ?? Convert.DBNull},
                {"pModificationDate", getWorkFlowNotification.ModificationDate ?? Convert.DBNull},
                {"pCreationDate", getWorkFlowNotification.CreationDate ?? Convert.DBNull},
                {"pCreatedBy", getWorkFlowNotification.CreatedBy ?? Convert.DBNull},
                {"pModifiedBy", getWorkFlowNotification.ModifiedBy ?? Convert.DBNull}
            };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetWorkFlowNotificationOutput>("dbo.GetWorkFlowNotification", inputParams, null);
            return result;
        }
        #endregion
    }
}
