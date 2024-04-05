using DataAccessLayer.Models;
using ManaretAmman.MiddleWare;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AutoMapper;
using BusinessLogicLayer.Mapper;
using BusinessLogicLayer.Services.ProjectProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccessLayer.DTO.Permissions;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

var TypesToRegister = Assembly.Load("BusinessLogicLayer").GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsClass).ToList();
var ITypesToRegister = Assembly.Load("BusinessLogicLayer").GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsInterface).ToList();

#region Cors Origin
string defaultpolicy = "default";
builder.Services.AddCors(
    options => options.AddPolicy(
        defaultpolicy,
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    )
);
#endregion

#region Atuo Mapping
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new Mapping());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

#region DbContext

builder.Services.AddDbContext<PayrolLogOnlyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion

// Add services to the container.
#region Injection
builder.Services.AddSingleton<IProjectProvider, ProjectProvider>();

builder.Services.AddScoped<DbContext, PayrolLogOnlyContext>();

for (int i = 0; i < TypesToRegister.Count; i++)
{
    var itype = ITypesToRegister
        .Find(t => (t.Name == "I" + TypesToRegister[i].Name));
    if (itype != null)
        builder.Services.AddScoped(itype, TypesToRegister[i]);
}

#endregion

//#region Identity
//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
//    .AddEntityFrameworkStores<BapetcoContext>()
//    .AddDefaultTokenProviders();
//#endregion
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:ValidAudience"],
        ValidAudience = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(config =>
{
    config.OperationFilter<HeaderFilter>();
    config.OperationFilter<AddLanguageHeaderOperationFilter>();
    // Reference your custom CSS file
   
    //Set the comments path for the Swagger JSON and UI.

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);   
   
    #region other Assembiles
    var dataAccessAss = Assembly.Load("DataAccessLayer");
    var xmlFileDataAccess = $"{dataAccessAss.GetName().Name}.xml";
    var xmlPathDataAccess = Path.Combine(AppContext.BaseDirectory, xmlFileDataAccess);
    if(File.Exists(xmlPathDataAccess))
    {
        config.IncludeXmlComments(xmlPathDataAccess);
        //config.OperationFilter<IncludeModelPropertyDescriptionsFilter>(xmlPathDataAccess); //this is if the summery of model properties doesn't appear on API 
    }
    #endregion
    //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
    //{
    //    var xmlFile = $"{assembly.GetName().Name}.xml";
    //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //    if (File.Exists(xmlPath))
    //    {
    //        config.IncludeXmlComments(xmlPath);
    //    }
    //}

});


builder.Services.AddHttpContextAccessor();

builder.Logging.AddConsole();
builder.Logging.AddFilter(category: DbLoggerCategory.Database.Command.Name, level: LogLevel.Information);


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
{    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // c.RoutePrefix = string.Empty; // Sets Swagger UI at the app's root
        // Add your custom CSS file to the Swagger UI
        c.InjectStylesheet("/swagger-custom_css.css"); // Adjust the path if your CSS is in a subdirectory
        c.DocExpansion(DocExpansion.None); // This line collapses all sections by default

    });
}


app.UseMiddleware(typeof(GlobalExceptionHandler));

#region Cors
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
#endregion

//app.UseExceptionHandler(); 

app.UseHttpsRedirection();

app.UseStaticFiles();//this for swagger css it should be before Routing

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.UseMiddleware(typeof(ProjectMiddleware));

app.MapControllers();

app.Run();