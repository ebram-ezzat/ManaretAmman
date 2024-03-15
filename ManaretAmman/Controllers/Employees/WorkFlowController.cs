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

        #region WorkFlowHeader Screen1
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
        /// <summary>
        /// Using This API For Insert Or Update
        /// </summary>
        /// <param name="insertOrUpdateWorkFlowHeader"></param>
        /// <returns></returns>
        [HttpPost("InsertWorkFlowHeader")]
        public async Task<IApiResponse> InsertWorkFlowHeader([FromBody] InsertOrUpdateWorkFlowHeader insertOrUpdateWorkFlowHeader)
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

        #region WorkFlowStep Screen2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getWorkFlowStepInput"></param>
        /// <returns></returns>
        [HttpGet("GetWorkFlowStep")]
        public async Task<IApiResponse> GetWorkFlowStep([FromQuery] GetWorkFlowStepInput getWorkFlowStepInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.GetWorkFlowStep(getWorkFlowStepInput);



            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteWorkFlowStep"></param>
        /// <returns></returns>
        [HttpDelete("DeleteWorkFlowStep")]
        public async Task<IApiResponse> DeleteWorkFlowStep([FromQuery] DeleteWorkFlowStep deleteWorkFlowStep)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.DeleteWorkFlowStep(deleteWorkFlowStep);


            return ApiResponse<int>.Success("data has been deleted succussfully", result);
        }
        /// <summary>
        /// Using This API For Insert Or Update
        /// </summary>
        /// <param name="insertOrUpdateWorkFlowStep"></param>
        /// <returns></returns>
        [HttpPost("InsertWorkFlowStep")]
        public async Task<IApiResponse> InsertWorkFlowStep([FromBody] InsertOrUpdateWorkFlowStep insertOrUpdateWorkFlowStep)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.InsertOrUpdateWorkFlowStep(insertOrUpdateWorkFlowStep);

            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        #endregion
    }
}
