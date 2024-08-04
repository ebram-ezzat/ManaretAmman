using AutoMapper;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mapper
{
    public class UserTypeDataResolver : IValueResolver<EvaluationSurveySetup, GetEvaluationSurveySetup, List<UserTypeEvaluationSurveySetup>>
    {
        public List<UserTypeEvaluationSurveySetup> Resolve(EvaluationSurveySetup source, GetEvaluationSurveySetup destination, List<UserTypeEvaluationSurveySetup> destMember, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source.UsertypeData)
                ? new List<UserTypeEvaluationSurveySetup>()
                : JsonSerializer.Deserialize<List<UserTypeEvaluationSurveySetup>>(source.UsertypeData);
        }
    }

    public class DepartmentIdsResolver : IValueResolver<EvaluationSurveySetup, GetEvaluationSurveySetup, List<int>>
    {
        public List<int> Resolve(EvaluationSurveySetup source, GetEvaluationSurveySetup destination, List<int> destMember, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source.DepartmentIds)
                ? new List<int>()
                : source.DepartmentIds.Split(',').Select(int.Parse).ToList();
        }
    }

    public class EmployeelevelIdsResolver : IValueResolver<EvaluationSurveySetup, GetEvaluationSurveySetup, List<int>>
    {
        public List<int> Resolve(EvaluationSurveySetup source, GetEvaluationSurveySetup destination, List<int> destMember, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source.EmployeelevelIds)
                ? new List<int>()
                : source.EmployeelevelIds.Split(',').Select(int.Parse).ToList();
        }
    }

    public class UsertypeDataStringResolver : IValueResolver<SaveEvaluationSurveySetup, EvaluationSurveySetup, string>
    {
        public string Resolve(SaveEvaluationSurveySetup source, EvaluationSurveySetup destination, string destMember, ResolutionContext context)
        {
            return source.UsertypeData == null
                ? string.Empty
                : JsonSerializer.Serialize(source.UsertypeData);
        }
    }

    public class DepartmentIdsStringResolver : IValueResolver<GetEvaluationSurveySetup, EvaluationSurveySetup, string>
    {
        public string Resolve(GetEvaluationSurveySetup source, EvaluationSurveySetup destination, string destMember, ResolutionContext context)
        {
            return source.DepartmentIds == null
                ? string.Empty
                : string.Join(",", source.DepartmentIds);
        }
    }

    public class EmployeelevelIdsStringResolver : IValueResolver<GetEvaluationSurveySetup, EvaluationSurveySetup, string>
    {
        public string Resolve(GetEvaluationSurveySetup source, EvaluationSurveySetup destination, string destMember, ResolutionContext context)
        {
            return source.EmployeelevelIds == null
                ? string.Empty
                : string.Join(",", source.EmployeelevelIds);
        }
    }

    public class UserTypeDataJsonResolver : IValueResolver<GetEvaluationSurveySetup, EvaluationSurveySetup, string>
    {
        public string Resolve(GetEvaluationSurveySetup source, EvaluationSurveySetup destination, string destMember, ResolutionContext context)
        {
            return source.UsertypeData == null
                ? string.Empty
                : JsonSerializer.Serialize(source.UsertypeData);
        }
    }

}
