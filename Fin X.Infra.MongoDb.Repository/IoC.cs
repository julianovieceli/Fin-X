using Fin_X.Domain.Interfaces;
using Fin_X.Infra.MongoDb.Repository.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Fin_X.Infra.MongoDb.Repository
{
    public static class Ioc
    {
        public static IServiceCollection AddMongoDbRepositories(this IServiceCollection services)
        {
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IPatientHistoryRepository, PatientHistoryRepository>();
            return services.AddScoped<IPatientRepository, PatientRepository>();

        }
    }
}
