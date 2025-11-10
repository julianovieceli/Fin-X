using Fin_X.Tests.IntegrationTests;

// 1. Define o nome da coleção
[CollectionDefinition("Integration Test Collection")]
// 2. Associa a coleção com a sua CustomWebApplicationFactory
public class IntegrationTestCollection : ICollectionFixture<MongoDbFixture>
{
    // Esta classe não precisa de código, ela é apenas um ponto de referência.
}