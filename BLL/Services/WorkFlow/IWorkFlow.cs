using DataAccessLayer.DTO.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.WorkFlow
{
    public interface IWorkFlow
    {
        public Task<int> InsertOrUpdateWorkFlowHeader(InsertOrUpdateWorkFlowHeader insertOrUpdateWorkFlowHeader);
        public Task<int> DeleteWorkFlowHeader(DeleteWorkFlowHeader deleteWorkFlowHeader);
        public Task<List<GetWorkFlowHeaderOutput>> GetWorkFlowHeader(GetWorkFlowHeaderInput getWorkFlowHeaderInput);

        public Task<int> InsertOrUpdateWorkFlowStep(InsertOrUpdateWorkFlowStep insertOrUpdateWorkFlowStep);
        public Task<int> DeleteWorkFlowStep(DeleteWorkFlowStep deleteWorkFlowStep);
        public Task<List<GetWorkFlowStepOutput>> GetWorkFlowStep(GetWorkFlowStepInput getWorkFlowHeaderStep);
    }
}
