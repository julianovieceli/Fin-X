using Mongo2Go;

namespace Fin_X.Tests.IntegrationTests
{
    public class MongoDbFixture : IDisposable
    {
        private readonly MongoDbRunner _runner;

        public string ConnectionString { get; }

        // The database name used for the test
        public string DatabaseName { get; } = "FinX_TestDb";

        public MongoDbFixture()
        {
            _runner = MongoDbRunner.Start();

            // 2. Build the connection string
            ConnectionString = _runner.ConnectionString;
        }

        // 3. Clean up and stop the MongoDB process after tests complete
        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
