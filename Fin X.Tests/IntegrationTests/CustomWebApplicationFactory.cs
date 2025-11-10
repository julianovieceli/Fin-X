using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Personal.Common.Infra.MongoDb.Repository;
using Personal.Common.Infra.MongoDb.Repository.Interfaces;

namespace Fin_X.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram>
     : Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<TProgram>, IClassFixture<MongoDbFixture> where TProgram : class
    {
        private readonly MongoDbFixture _mongoDbFixture;
        public Fixture AutoFixture { get; private set; }

        public CustomWebApplicationFactory(MongoDbFixture mongoDbFixture)
        {
            _mongoDbFixture = mongoDbFixture;
            AutoFixture = new Fixture();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Removendo service "quente"
                 var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoDbcontext));
                if (descriptor != null)
                 services.Remove(descriptor);


                // 2. Register the IMongoDatabase/IMongoClient using the Mongo2Go connection string
                services.AddSingleton<IMongoDbcontext>(sp =>
                {
                    return new MongoDbContext(_mongoDbFixture.ConnectionString, _mongoDbFixture.DatabaseName);
                });

                

            });

            
        }
    }
}
