using EsperancaSolidaria.Identity.API.Converters;
using EsperancaSolidaria.Identity.API.Filters;
using EsperancaSolidaria.Identity.API.Middleware;
using EsperancaSolidaria.Identity.API.Token;
using EsperancaSolidaria.Identity.Application;
using EsperancaSolidaria.Identity.Domain.Security.Tokens;
using EsperancaSolidaria.Identity.Infrastructure;
using EsperancaSolidaria.Identity.Infrastructure.DataAccess;
using EsperancaSolidaria.Identity.Infrastructure.Extensions;
using EsperancaSolidaria.Identity.Infrastructure.Migrations;
using Microsoft.OpenApi;
using Prometheus;

const string AUTHENTICATION_SCHEME = "Bearer";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

// Prometheus metrics
builder.Services.AddMetrics();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<IdsFilter>();

    options.AddSecurityDefinition(AUTHENTICATION_SCHEME, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = AUTHENTICATION_SCHEME,
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(AUTHENTICATION_SCHEME, document)] = []
    });
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks();
builder.Services.AddHealthChecks().AddDbContextCheck<EsperancaSolidariaIdentityDbContext>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Prometheus metrics 
app.MapMetrics();

app.MapHealthChecks("/health");

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigrations.Migrate(connectionString, serviceScope.ServiceProvider);
}