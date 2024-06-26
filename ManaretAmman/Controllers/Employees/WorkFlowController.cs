﻿using BusinessLogicLayer.Services.WorkFlow;
using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.DTO.WorkFlow;
using LanguageExt.Common;
using ManaretAmman.MiddleWare;
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
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English
        /// </summary>       
        /// <param name="getWorkFlowHeaderInput"></param>
        /// <returns></returns>
        [AddLanguageHeaderAttribute]
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
            if (result == 1)//error 
            {
                throw new Exception("delete Failed because you should delete first workflow steps");
            }

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
        /// you should send {Accept-Language} Via header request to get the correct description "ar" For Arabic and "en" For English
        /// </summary>       
        /// <param name="getWorkFlowStepInput"></param>
        /// <returns></returns>
        [AddLanguageHeaderAttribute]
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
            if (result == 1)
            {
                throw new Exception("delete Failed because you should delete first notification");
            }

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

        #region WorkFlowNotificationsSteps Screen3
        /// <summary>
        /// you should send {Accept-Language} Via header request to get the correct description "ar" For Arabic and "en" For English
        /// </summary>
        /// <param name="getWorkFlowNotificationStepInput"></param>
        /// <returns></returns>
        [HttpGet("GetWorkFlowNotificationStep")]
        public async Task<IApiResponse> GetWorkFlowNotificationStep([FromQuery] GetWorkFlowNotificationStepInput getWorkFlowNotificationStepInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.GetWorkFlowNotificationStep(getWorkFlowNotificationStepInput);



            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteWorkFlowNotificationStep"></param>
        /// <returns></returns>
        [HttpDelete("DeleteWorkFlowNotificationStep")]
        public async Task<IApiResponse> DeleteWorkFlowNotificationStep([FromQuery] DeleteWorkFlowNotificationStep deleteWorkFlowNotificationStep)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.DeleteWorkFlowNotificationStep(deleteWorkFlowNotificationStep);


            return ApiResponse<int>.Success("data has been deleted succussfully", result);
        }
        /// <summary>
        /// Using This API For Insert Or Update
        /// </summary>
        /// <param name="insertOrUpdateWorkFlowNotificationStep"></param>
        /// <returns></returns>
        [HttpPost("InsertWorkFlowNotificationStep")]
        public async Task<IApiResponse> InsertWorkFlowNotificationStep([FromBody] InsertOrUpdateWorkFlowNotificationStep insertOrUpdateWorkFlowNotificationStep)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.InsertOrUpdateWorkFlowNotificationStep(insertOrUpdateWorkFlowNotificationStep);

            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        #endregion

        #region WorkFlowNotification Seperate Screen
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getWorkFlowNotificationInput"></param>
        /// <returns></returns>
        [HttpGet("GetWorkFlowNotification")]
        public async Task<IApiResponse> GetWorkFlowNotification([FromQuery] GetWorkFlowNotificationInput getWorkFlowNotificationInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.GetWorkFlowNotification(getWorkFlowNotificationInput);



            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteWorkFlowNotification"></param>
        /// <returns></returns>
        [HttpDelete("DeleteWorkFlowNotification")]
        public async Task<IApiResponse> DeleteWorkFlowNotification([FromQuery] DeleteWorkFlowNotification deleteWorkFlowNotification)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.DeleteWorkFlowNotification(deleteWorkFlowNotification);


            return ApiResponse<int>.Success("data has been deleted succussfully", result);
        }
        /// <summary>
        /// Using This API For Insert and the update process removing the old record and adding the new one
        /// </summary>
        /// <param name="insertWorkFlowNotification"></param>
        /// <returns></returns>
        [HttpPost("InsertWorkFlowNotification")]
        public async Task<IApiResponse> InsertWorkFlowNotification([FromBody] InsertWorkFlowNotification insertWorkFlowNotification)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _iWorkFlow.InsertWorkFlowNotification(insertWorkFlowNotification);

            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        #endregion
    }
}
