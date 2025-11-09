using AutoMapper;
using Fin_X.Application.Api;
using Fin_X.Application.Services;
using Fin_X.Application.Settings;
using Fin_X.Dto;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Porter.Application.Mapping;
using Porter.Application.Validators;

namespace Fin_X.Application
{
    public static class IoC
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PatientProfile>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBrasilApiService, BrasilApiService>();
            return services.AddScoped<IPatientService, PatientService>();
        }


        public static IServiceCollection AddValidators(this IServiceCollection services)
        {

            
            return services.AddScoped<IValidator<RegisterPatientDto>, PatientValidator>();
        }


        public static IServiceCollection AddHttpClient(this IServiceCollection services
     , IConfiguration configuration)
        {
            services.Configure<BrasilApiSettings>(configuration.GetSection("BrasilApiSettings"));

            BrasilApiSettings brasilApiSettings = new BrasilApiSettings();
            configuration.GetSection("BrasilApiSettings").Bind(brasilApiSettings);


            services.AddHttpClient(nameof(HttpClientEnum.API_BRASIL), httpClient =>
            {
                httpClient.BaseAddress = new Uri(brasilApiSettings.BaseUrl);
            });

            return services;
        }
    }
}
