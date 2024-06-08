using BusinessLogicLayer.Services.Employees;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;
using ManaretAmman.MiddleWare;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetList")]
        public async Task<IApiResponse> GetList() 
        {
            var result = await _employeeService.GetEmployeesProc();

            return ApiResponse<List<EmployeeLookup>>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost("SaveAttendanceByUser")]
        public async Task<IApiResponse> SaveAttendanceByUser(SaveAttendance saveAttendance)
        {
            await _employeeService.SaveAttendanceByUser(saveAttendance);

            return ApiResponse.Success("data has been retrieved succussfully");
        }
        [HttpGet("Documents/GetEmployeePaper")]
        public async Task<IApiResponse> GetEmployeePaper([FromQuery] GetEmployeePaperRequest getEmployeePaperRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeePaperProc(getEmployeePaperRequest);



            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        [HttpDelete("Documents/DeleteEmployeePaper")]
        public async Task<IApiResponse> DeleteEmployeePaper([FromQuery]  DeleteEmployeePaper model)
        {
            if (model.EmployeeId <= 0 || model.DetailId <= 0)
            {

                return ApiResponse.Failure(" An unexpected error on validation occurred you should path each parameter bigger than 0",null);
            }
            var result = await _employeeService.DeleteEmployeePaperProc(model.EmployeeId, model.DetailId);



            return ApiResponse<int>.Success("data has been retrieved succussfully", result);
        }
        [HttpPost("Documents/SaveEmployeePaper")]
        public async Task<IApiResponse> SaveEmployeePaper([FromForm]SaveEmployeePaper saveEmployeePaper)
        {
            //var formCollection = await Request.ReadFormAsync();
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
           var result = await _employeeService.SaveEmployeePaperProc(saveEmployeePaper);



            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }

        [HttpGet("EmployeeProfile")]
        public async Task<IApiResponse> EmployeeProfile(int EmployeeId)
        {
            var result = await _employeeService.EmployeeProfile(EmployeeId);

            return ApiResponse<List<EmplyeeProfileVModel>>.Success("data has been retrieved succussfully", result);
        }
        #region شاشة خدمات شوون الموظفين
        /// <summary>
        /// You can use this API For Insert or Update
        /// </summary>
        /// <remarks>
        /// {StatusID} Should be sent by 1 
        /// </remarks>
        /// <param name="saveEmployeeAffairsService"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("SaveEmployeeAffairsService")]
        public async Task<IApiResponse> SaveEmployeeAffairsService(SaveEmployeeAffairsServices saveEmployeeAffairsService)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result= await _employeeService.SaveEmployeeAffairsService(saveEmployeeAffairsService);
            if(result ==-1)//error
            {
                throw new Exception("Error on Save Operation");
            }
            return ApiResponse.Success("data has been saved succussfully");
        }
        /// <summary>
        /// you can send {Accept-Language} Via header request to get the correct description "ar" For Arabic and "en" For English
        /// </summary>       
        /// <param name="getEmployeeAffairsServiceRequest"></param>
        /// <returns></returns>
        [AddLanguageHeaderAttribute]
        [HttpGet("GetEmployeeAffairsService")]
        public async Task<IApiResponse> GetEmployeeAffairsService([FromQuery]GetEmployeeAffairsServiceRequest getEmployeeAffairsServiceRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeAffairsService(getEmployeeAffairsServiceRequest);
           
            return ApiResponse<dynamic>.Success("data has been returned succussfully", result);
        }
        /// <summary>
        /// {StatusID} Should be sent by 2 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="deleteEmployeeAffairsService"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("DeleteEmployeeAffairsService")]
        public async Task<IApiResponse> DeleteEmployeeAffairsService([FromQuery] DeleteEmployeeAffairsService deleteEmployeeAffairsService)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEmployeeAffairsService(deleteEmployeeAffairsService);
            if (result == -1)//error
            {
                throw new Exception("Error on Delete Operation");
            }
            return ApiResponse.Success("data has been deleted succussfully");
        }
        #endregion

        #region Employee Evaluation
            #region EvaluationCategory
        [HttpPost("SaveOrUpdateEmployeeEvaluation")]        
        public async Task<IApiResponse> SaveOrUpdateEmployeeEvaluation(SaveOrUpdateEmployeeEvaluation saveOrUpdateEmployeeEvaluation)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveOrUpdateEmployeeEvaluation(saveOrUpdateEmployeeEvaluation);
            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        /// <summary>
        /// {StatusId} Should be sent by 1 Active (when page Load), 0 Archive ,null if you want get by {CategoryId}  
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="getEmployeeEvaluation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetEmployeeEvaluation")]
        public async Task<IApiResponse> GetEmployeeEvaluation([FromQuery]GetEmployeeEvaluation getEmployeeEvaluation)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeEvaluation(getEmployeeEvaluation);
            return ApiResponse<List<GetEmployeeEvaluation>>.Success("data has been returned succussfully", result);
        }
        #endregion
        #region Evaluation Question
        [HttpPost("SaveOrUpdateEvaluationQuestion")]
        public async Task<IApiResponse> SaveOrUpdateEvaluationQuestion(SaveOrUpdateEvaluationQuestion saveOrUpdateEvaluationQuestion)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveOrUpdateEmployeeEvaluationQuestion(saveOrUpdateEvaluationQuestion);
            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        /// <summary>
        /// {CategoryId} Get as DDL with only sent {StatusId} 1 Active To GetEmployeeEvaluation EndPoint (when page Load)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="getEvaluationQuestion"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetEvaluationQuestion")]
        public async Task<IApiResponse> GetEvaluationQuestion([FromQuery] GetEvaluationQuestion getEvaluationQuestion)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeEvaluationQuestion(getEvaluationQuestion);
            return ApiResponse<List<GetEvaluationQuestion>>.Success("data has been returned succussfully", result);
        }
        #endregion

        #region Evaluation Survey
        /// <summary>
        /// {StatusId} 0 Active ,1 not ctive
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="saveOrUpdateEvaluationSurvey"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateEvaluationSurvey")]
        public async Task<IApiResponse> SaveOrUpdateEvaluationSurvey(SaveOrUpdateEvaluationSurvey saveOrUpdateEvaluationSurvey)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveOrUpdateEvaluationSurvey(saveOrUpdateEvaluationSurvey);
            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        /// <summary>
        /// {StatusId} 0 Active ,1 not ctive
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="getEvaluationSurvey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetEvaluationSurvey")]
        public async Task<IApiResponse> GetEvaluationSurvey([FromQuery] GetEvaluationSurvey getEvaluationSurvey)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEvaluationSurvey(getEvaluationSurvey);
            return ApiResponse<List<GetEvaluationSurvey>>.Success("data has been returned succussfully", result);
        }
        [HttpDelete("DeleteEvalualtionSurvey")]
        public async Task<IApiResponse> DeleteEvaluationSurvey([FromQuery] DeleteEvalualtionSurvey deleteEvalualtionSurvey)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEvaluationSurvey(deleteEvalualtionSurvey);
            return ApiResponse<int>.Success("data has been delte succussfully", result);
        }
        #endregion
        #endregion
    }
}
