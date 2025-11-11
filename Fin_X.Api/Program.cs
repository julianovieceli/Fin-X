using Fin_X.Api.Swagger;
using Fin_X.Application;
using Fin_X.Infra.MongoDb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Personal.Common;
using Personal.Common.Dto;
using Personal.Common.Infra.MongoDb.Repository;
using Personal.Common.Utils;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Where(x => x.Value.Errors.Any())
                                         .Select(x => new { Field = x.Key, Messages = x.Value.Errors.Select(e => e.ErrorMessage) })
            .ToList();

            string messagge = String.Join(", ", errors.FirstOrDefault().Messages);

            ErrorResponseDto error = new ErrorResponseDto("400", messagge);


            return new BadRequestObjectResult(error);
        };
    })
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
      }); ;

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Fin X", Version = "v1" });

    // --- 1. JWT Bearer Token Authentication ---
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    // --- 2. Basic Authentication ---
    var basicSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization (Basic)",
        Type = SecuritySchemeType.Http,
        Scheme = "Basic",
        In = ParameterLocation.Header,
        Description = "Basic Auth: Enter username:password encoded in Base64 (e.g., Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==)"
    };
    c.AddSecurityDefinition("BasicAuthentication", basicSecurityScheme);

    // Adds JWT Bearer token security requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });

    // Adds Basic Authentication security requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BasicAuthentication" }
            },
            Array.Empty<string>()
        }
    });


    c.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddAutoMapper();
builder.Services.AddServices();
builder.Services.AddMongoDbRepositories();

builder.Services.AddValidators();

builder.Services.AddMongoDbContext(
    builder.Configuration.GetSection("MongoDbSettings:ConnectionString").Value!,
    builder.Configuration.GetSection("MongoDbSettings:Database").Value!
    );

// Add Authentication Services p gerar o token JWT
builder.Services.AddBasicAuthentication(builder.Configuration);

// Add Jwt Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddJwtTokenConfigurations(builder.Configuration);

builder.Services.AddHealthChecks();

builder.Services.AddHttpClient(builder.Configuration);

builder.Services.AddMemoryCache(); // Register IMemoryCache



builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .MinimumLevel.Information() // Nível padrão para logs da sua aplicação
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {RequestId} {Message:lj}{NewLine}{Exception}"
            )
            //.WriteTo.File("logs/api.log", rollingInterval: RollingInterval.Day)
        );



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health"); // Exposes a health check endpoint at /health

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();



app.UseSerilogRequestLogging(options =>
{
    // Loga o início e o fim da requisição
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";

    // Configura o Serilog para capturar o TraceIdentifier (o ID da requisição)
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestId", httpContext.TraceIdentifier);
        // Opcional: Adicionar o usuário logado (se houver)
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
        }
    };
});

app.MapControllers();


app.Run();


public partial class Program { }