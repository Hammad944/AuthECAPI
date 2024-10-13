using AuthECapi.Controllers;
using AuthECapi.Extenstions;
using AuthECapi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .AddAppConfig(builder.Configuration)
    .AddIdentityHandlerAndStores()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app
    .ConfigureSwaggerExplorer()
#region Config. CORS
    .ConfigureCORS(builder.Configuration)
#endregion
    .AddIdenityAuthMiddlewares();



app.MapControllers();

app
    .MapGroup("/api")
    .MapIdentityApi<AppUser>();
app
    .MapGroup("/api")
    .MapIdentityUserEndpoints();


app.Run();
