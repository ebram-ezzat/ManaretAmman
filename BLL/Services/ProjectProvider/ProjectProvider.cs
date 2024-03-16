using LanguageExt.ClassInstances.Pred;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services.ProjectProvider;

public class ProjectProvider : IProjectProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public int GetProjectId()
    {
        var projectId = _httpContextAccessor.HttpContext.Request.Headers["ProjectId"].ToString();

        return int.Parse(projectId);
    }

    public int LangId()
    {
        string langId = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString();
        if (string.IsNullOrEmpty(langId)) return 1; //default is 1 (Arabic Lang);
        int returnedId = langId.Contains("en")? 2 : 1; //2 is English
        return returnedId;
    }

    public int UserId()
    {
        string userId = _httpContextAccessor.HttpContext.Request.Headers["UserId"].ToString();
        if (string.IsNullOrEmpty(userId)) return -1;
        return int.Parse(userId);
    }
}
