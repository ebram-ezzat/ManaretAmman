using BusinessLogicLayer.Services.WorkFlow;
using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.DTO.WorkFlow;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowController : ControllerBase
    {
        private IWorkFlow _iWorkFlow { get; set; }
        public WorkFlowController(IWorkFlow workFlow)
        {
            _iWorkFlow = workFlow;
        }

        #region WorkFlowHeader Screen
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getWorkFlowHeaderInput"></param>
        /// <returns></returns>
        [HttpGet("GetWorkFlowHeader")]
        public async Task<IApiResponse> GetWorkFlowHeader([FromQuery] GetWorkFlowHeaderInput getWorkFlowHeaderInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.GetWorkFlowHeader(getWorkFlowHeaderInput);



            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteWorkFlowHeader"></param>
        /// <returns></returns>
        [HttpDelete("DeleteWorkFlowHeader")]
        public async Task<IApiResponse> DeleteWorkFlowHeader([FromQuery] DeleteWorkFlowHeader deleteWorkFlowHeader)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.DeleteWorkFlowHeader(deleteWorkFlowHeader);


            return ApiResponse<int>.Success("data has been deleted succussfully", result);
        }

        [HttpPost("InsertOrUpdateWorkFlowHeader")]
        public async Task<IApiResponse> InsertOrUpdateWorkFlowHeader([FromBody] InsertOrUpdateWorkFlowHeader insertOrUpdateWorkFlowHeader)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.InsertOrUpdateWorkFlowHeader(insertOrUpdateWorkFlowHeader);

            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        #endregion
    }
}
