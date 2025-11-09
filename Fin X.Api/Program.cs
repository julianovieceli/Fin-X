using Microsoft.AspNetCore.Mvc;
using Personal.Common.Dto;
using Personal.Common.Utils;
using Fin_X.Application;
using Fin_X.Infra.MongoDb.Repository;
using Personal.Common.Infra.MongoDb.Repository;
using Personal.Common;

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

builder.Services.AddSwaggerGen();

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

builder.Services.AddMemoryCache(); // Register IMemoryCache


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

app.MapControllers();

app.Run();
