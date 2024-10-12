using BusinessLogicLayer.Services.Employees;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.EmployeeContract;
using DataAccessLayer.DTO.EmployeeDeductions;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.EmployeeSalary;
using DataAccessLayer.DTO.EmployeeTransaction;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.Models;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
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
            var result = await _employeeService.SaveAttendanceByUser(saveAttendance);
            if(result==-3)
            {
                return ApiResponse.Failure(" An unexpected error on validation occurred ", new string[] { "The user does not have permission to log in from this device" });
            }
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
        /// .شاشة خدمات شوون الموظفين,
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

        #region Evaluation Survey questions

        [HttpPost("SaveEvaluationSurveyQuestions")]
        public async Task<IApiResponse> SaveEvaluationSurveyQuestions([FromBody]List<SaveEvaluationSurveyQuestions> LstQuestions)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEvaluationSurveyQuestions(LstQuestions);
            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        [HttpGet("GetEvaluationSurveyQuestions")]
        public async Task<IApiResponse> GetEvaluationSurveyQuestions([FromQuery] GetEvaluationSurveyQuestions getEvaluationSurveyQuestions)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEvaluationSurveyQuestions(getEvaluationSurveyQuestions);
            return ApiResponse<List<GetEvaluationSurveyQuestions>>.Success("data has been returned succussfully", result);
        }
        #endregion

        #region EvaluationSurveySetup اعدادت التقيم
        /// <summary>
        /// {StatusId} 1 active
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="saveOrUpdateEvaluationSurveySetup"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateEvaluationSurveySetup")]
        public async Task<IApiResponse> SaveOrUpdateEvaluationSurveySetup(SaveEvaluationSurveySetup saveOrUpdateEvaluationSurveySetup)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveOrUpdateEvaluationSurveySetup(saveOrUpdateEvaluationSurveySetup);
            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        [HttpDelete("DeleteEvaluationSurveySetup")]
        public async Task<IApiResponse> DeleteEvaluationSurveySetup([FromQuery] DeleteEvaluationSurveySetup deleteEvaluationSurveySetup)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEvaluationSurveySetup(deleteEvaluationSurveySetup);
            return ApiResponse<int>.Success("data has been delte succussfully", result);
        }

        /// <summary>
        /// {StatusId} 1 Active ,0 not ctive
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="getEvaluationSurveySetup"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetEvaluationSurveySetup")]
        public async Task<IApiResponse> GetEvaluationSurveySetup([FromQuery] GetEvaluationSurveySetup getEvaluationSurveySetup)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEvaluationSurveySetup(getEvaluationSurveySetup);
            return ApiResponse<List<GetEvaluationSurveySetup>>.Success("data has been returned succussfully", result);
        }
        #endregion
        #endregion

        #region EmployeePenalty
        /// <summary>
        /// you can send {Accept-Language} Via header request to get the correct description "ar" For Arabic and "en" For English
        /// </summary>
        /// <param name="getEmployeePenalty"></param>
        /// <returns></returns>
        [AddLanguageHeaderAttribute]
        [HttpGet("GetEmployeePenalty")]
        public async Task<IApiResponse> GetEmployeePenalty([FromQuery] GetEmployeePenalty getEmployeePenalty)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeePenalty(getEmployeePenalty);

            return ApiResponse<dynamic>.Success("data has been returned succussfully", result);
        }

        [HttpPost("SaveEmployeePenalty")]
        public async Task<IApiResponse> SaveEmployeePenalty(SaveEmployeePenalty saveEmployeePenalty)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeePenalty(saveEmployeePenalty);
            return ApiResponse.Success("data has been saved succussfully");
        }

        [HttpPost("UpdateEmployeePenalty")]
        public async Task<IApiResponse> UpdateEmployeePenalty(SaveEmployeePenalty UpdateEmployeePenalty)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.UpdateEmployeePenalty(UpdateEmployeePenalty);
            return ApiResponse.Success("data has been saved succussfully");
        }

        [HttpPost("ChangeStatusEmployeePenalty")]
        public async Task<IApiResponse> ChangeStatusEmployeePenalty(SaveEmployeePenalty UpdateEmployeePenalty)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.ChangeStatusEmployeePenalty(UpdateEmployeePenalty);
            return ApiResponse.Success("data has been saved succussfully");
        }
        #endregion

        #region  employeeShifts

        /// <summary>
        /// you can send {Accept-Language} Via header request to get the correct description "ar" For Arabic and "en" For English
        /// </summary>
        /// <param name="getEmployeeShiftCheck"></param>
        /// <returns></returns>
        [AddLanguageHeaderAttribute]
        [HttpGet("GetEmployeeShifts")]
        public async Task<IApiResponse> GetEmployeeShifts([FromQuery] GetEmployeeShifts getEmployeeShift)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeShifts(getEmployeeShift);

            return ApiResponse<dynamic>.Success("data has been returned succussfully", result);
        }

        [HttpPost("SaveEmployeeShiftCheck")]
        public async Task<IApiResponse> SaveEmployeeShiftCheck(GetEmployeeShifts saveEmployeeShifts)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeeShifts(saveEmployeeShifts);
            return ApiResponse.Success("data has been saved succussfully");
        }
        #endregion

        #region Employee Attandance Table (شاشة جدول الحضور )
        /// <summary>
        /// Employee Attandance Table (شاشة جدول الحضور )
        /// </summary>
        /// <param name="getEmployeeAttandanceShiftInput"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeAttandanceShift")]
        public async Task<IApiResponse> GetEmployeeAttandanceShift([FromQuery] GetEmployeeAttandanceShiftInput getEmployeeAttandanceShiftInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeAttandanceShift(getEmployeeAttandanceShiftInput);

            return ApiResponse<List<GetEmployeeAttandanceShiftOutput>>.Success("data has been returned succussfully", result);
        }

        [HttpDelete("DeleteEmployeeAttandanceShift")]
        public async Task<IApiResponse> DeleteEmployeeAttandanceShift([FromQuery] DeleteEmployeeAttandanceShifts deleteEmployeeAttandanceShifts)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEmployeeAttandanceShifts(deleteEmployeeAttandanceShifts);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }

        [HttpPost("SaveEmployeeAttandanceShift")]
        public async Task<IApiResponse> SaveEmployeeAttandanceShift([FromBody] SaveEmployeeAttandanceShiftInput saveEmployeeAttandanceShiftInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeeAttandanceShifts(saveEmployeeAttandanceShiftInput);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }
        #endregion

        #region Employee Transaction

        [HttpGet("GetEmployeeTransaction")]
        public async Task<IApiResponse> GetEmployeeTransaction([FromQuery] GetEmployeeTransactionInput getEmployeeTransactionInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeTransaction(getEmployeeTransactionInput);

            return ApiResponse<dynamic>.Success("data has been returned succussfully", result);
        }

        [HttpDelete("DeleteEmployeeTransaction")]
        public async Task<IApiResponse> DeleteEmployeeTransaction([FromQuery] DeleteEmployeeTransaction deleteEmployeeTransaction)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEmployeeTransaction(deleteEmployeeTransaction);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }

        [HttpPost("SaveEmployeeTransaction")]
        public async Task<IApiResponse> SaveEmployeeTransaction([FromBody] SaveEmployeeTransaction saveEmployeeTransaction)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeeTransaction(saveEmployeeTransaction);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }

        #endregion

        #region Employee Salary

        [HttpGet("GetEmployeeSalary")]
        public async Task<IApiResponse> GetEmployeeSalary([FromQuery] GetEmployeeSalaryInput getEmployeeSalaryInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeSalary(getEmployeeSalaryInput);

            return ApiResponse<List<GetEmployeeSalaryOutput>>.Success("data has been returned succussfully", result);
        }

        [HttpGet("GetEmployeeSalaryDetails")]
        public async Task<IApiResponse> GetEmployeeSalaryDetails([FromQuery] GetEmployeeSalaryInput getEmployeeSalaryInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeSalaryDetails(getEmployeeSalaryInput);

            return ApiResponse<GetEmployeeSalaryDetailsOutput>.Success("data has been returned succussfully", result);
        }

        [HttpDelete("DeleteCancelSalary")]
        public async Task<IApiResponse> DeleteCancelSalary([FromBody] DeleteCancelSalary deleteCancelSalary)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteCancelSalary(deleteCancelSalary);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }

        [HttpPost("CalculateEmployeeSalary")]
        public async Task<IApiResponse> CalculateEmployeeSalary([FromBody] CalculateEmployeeSalary calculateEmployeeSalary)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.CalculateEmployeeSalary(calculateEmployeeSalary);
            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }
        #endregion
        #region Employee Allownances (شاشة علاوات الموظفين)
        /// <summary>
        /// Employee Allownances (شاشة علاوات الموظفين)
        /// </summary>
        /// <param name="getEmployeeAllowancesInput"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeAllowancesMainScreen")]
        public async Task<IApiResponse> GetEmployeeAllowancesMainScreen([FromQuery] GetEmployeeAllowancesInput getEmployeeAllowancesInput)
        {
            if (!(getEmployeeAllowancesInput.Flag==1) )
            {

                var errors = new List<string>()
                {
                    "Flag Property should be 1 ",
                };


                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeAllowancesMainScreen(getEmployeeAllowancesInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        [AddLanguageHeader]
        [HttpGet("GetEmployeeAllowancesPopup")]
        public async Task<IApiResponse> GetEmployeeAllowancesPopup([FromQuery] GetEmployeeAllowancesInput getEmployeeAllowancesInput)
        {
            if (!(getEmployeeAllowancesInput.AllowanceID.HasValue || getEmployeeAllowancesInput.AllowanceID.Value > 0 || getEmployeeAllowancesInput.Flag == 2))
            {

                var errors = new List<string>()
                {
                    "Flag Property should be 2 and AllowanceID Property Should Has Value",
                };


                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeAllowancesPopupScreen(getEmployeeAllowancesInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }

        [HttpDelete("DeleteEmployeeAllowances")]
        public async Task<IApiResponse> DeleteEmployeeAllowances([FromQuery] DeleteEmployeeAllowances deleteEmployeeAllowances)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEmployeeAllowances(deleteEmployeeAllowances);
            if(result == -3)
            {
                var errors = new List<string>()
                {
                    "cannot delete because the employee's salary has been calculated",
                };

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }
        [HttpPost("UpdateEmployeeAllowances")]
        public async Task<IApiResponse> UpdateEmployeeAllowances([FromQuery] UpdateEmployeeAllowances updateEmployeeAllowances)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.UpdateEmployeeAllowances(updateEmployeeAllowances);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }
        [HttpPost("SaveEmployeeAllowances")]
        public async Task<IApiResponse> SaveEmployeeAllowances([FromQuery] SaveEmployeeAllowances saveEmployeeAllowances)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeeAllowances(saveEmployeeAllowances);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }
        [HttpGet("GetEmployeeAllowancesDeductionDDL")]
        public async Task<IApiResponse> GetEmployeeAllowancesDeductionDDL([FromQuery] GetAllowanceDeductionInput getAllowanceDeductionInput)
        {
           
            var result = await _employeeService.GetEmployeeAllowancesDeductionDDL(getAllowanceDeductionInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region اقتطاعات الموظفين

        /// <summary>
        /// Employee Allownances (شاشة اقتطاعات الموظفين)
        /// </summary>
        /// <param name="getEmployeeDeductionsInput"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeDeductionsMainScreen")]
        public async Task<IApiResponse> GetEmployeeDeductionsMainScreen([FromQuery] GetEmployeeDeductionsInput getEmployeeDeductionsInput)
        {
            if (!(getEmployeeDeductionsInput.Flag == 1))
            {

                var errors = new List<string>()
                {
                    "Flag Property should be 1 ",
                };


                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeDeductionsMainScreen(getEmployeeDeductionsInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }

        [AddLanguageHeader]
        [HttpGet("GetEmployeeDeductionsPopup")]
        public async Task<IApiResponse> GetEmployeeDeductionsPopup([FromQuery] GetEmployeeDeductionsInput getEmployeeDeductionsInput)
        {
            if (!(getEmployeeDeductionsInput.AllowanceID.HasValue || getEmployeeDeductionsInput.AllowanceID.Value > 0 || getEmployeeDeductionsInput.Flag == 2))
            {

                var errors = new List<string>()
                {
                    "Flag Property should be 2 and DeductionID Property Should Has Value",
                };


                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeDeductionsPopupScreen(getEmployeeDeductionsInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }

        [HttpDelete("DeleteEmployeeDeductions")]
        public async Task<IApiResponse> DeleteEmployeeDeductions([FromQuery] DeleteEmployeeDeductions deleteEmployeeDeductions)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEmployeeDeductions(deleteEmployeeDeductions);
            if (result == -3)
            {
                var errors = new List<string>()
                {
                    "cannot delete because the employee's salary has been calculated",
                };

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }

        [HttpPost("UpdateEmployeeDeductions")]
        public async Task<IApiResponse> UpdateEmployeeDeductions([FromQuery] UpdateEmployeeDeductions updateEmployeeDeductions)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.UpdateEmployeeDeductions(updateEmployeeDeductions);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }

        [HttpPost("SaveEmployeeDeductions")]
        public async Task<IApiResponse> SaveEmployeeDeductions([FromQuery] SaveEmployeeDeductions saveEmployeeDeductions)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeeDeductions(saveEmployeeDeductions);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }
        #endregion

        #region Adding Employess(شاشة اضافة موظفين )
        /// <summary>
        /// Adding Employess (شاشة اضافة الموظفين)
        /// </summary>
        /// <param name="getEmployeeInput"></param>
        /// <returns></returns>
        [HttpGet("GetEmployees")]
        public async Task<IApiResponse> GetEmployees([FromQuery] GetEmployeesInput getEmployeeInput)
        {
            
            var result = await _employeeService.GetEmployees(getEmployeeInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        [HttpPost("SaveOrUpdateEmployee")]
        public async Task<IApiResponse> SaveOrUpdateEmployee([FromBody] SaveOrUpdateEmployeeAllData saveOrUpdateEmployeeAllData)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveOrUpdateEmployee(saveOrUpdateEmployeeAllData);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<IApiResponse> DeleteEmployee([FromBody] DeleteEmployeeWithRelatedData deleteEmployeeWithRelatedData)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.DeleteEmployeeWithRelatedData(deleteEmployeeWithRelatedData);

            return ApiResponse<int>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region Employee Contracts(العقود)

        /// <summary>
        /// you can send {Accept-Language} Via header request to get the correct description "ar" For Arabic and "en" For English
        /// </summary>
        /// <param name="getEmployeeContract"></param>
        /// <returns></returns>
        [AddLanguageHeaderAttribute]
        [HttpGet("GetEmployeeContracts")]
        public async Task<IApiResponse> GetEmployeeContracts([FromQuery] GetEmployeeContracts GetEmployeeContract)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeeContracts(GetEmployeeContract);

            return ApiResponse<dynamic>.Success("data has been returned succussfully", result);
        }

        [HttpPost("SaveEmployeeContracts")]
        public async Task<IApiResponse> SaveEmployeeContracts(SaveEmployeeContracts saveEmployeeContracts)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeeContracts(saveEmployeeContracts);
            return ApiResponse.Success("data has been saved succussfully");
        }

        #endregion
        #region Employee Additional Info(معلومات اضافية للموظف)
        /// <summary>
        ///  Additional Info Employess (شاشة فرعية معلومات اضافية الموظفين)
        /// </summary>
        /// <param name="getEmployeeAdditionalInfoInput"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeesAdditionalInfo")]
        public async Task<IApiResponse> GetEmployeesAdditionalInfo([FromQuery] GetEmployeeAdditionalInfoInput getEmployeeAdditionalInfoInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeesAdditionalInfo(getEmployeeAdditionalInfoInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        [HttpPost("SaveOrUpdateEmployeeAdditionalInfo")]
        public async Task<IApiResponse> SaveOrUpdateEmployeeAdditionalInfo([FromBody] SaveOrUpdateEmployeeAdditionalInfo saveOrUpdateEmployeeAdditionalInfo)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.SaveEmployeeAdditionalInfo(saveOrUpdateEmployeeAdditionalInfo);

            return ApiResponse<int>.Success("data has been returned succussfully", result);
        }
        #endregion
    }
}
