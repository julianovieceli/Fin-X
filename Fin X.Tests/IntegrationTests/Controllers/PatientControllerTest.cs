using AutoFixture;
using Fin_X.Dto;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Fin_X.Tests.IntegrationTests.Controllers
{
    [Collection("Integration Test Collection")]
    public class PatientControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Fixture _fixture;


        public PatientControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _fixture = factory.AutoFixture;
        }


        private async Task<string> GetToken()
        {
            string user = "finx";
            string password = "finx123";

               var authenticationString = $"{user}:{password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);


            var response = await _client.PostAsync("/api/auth/login", null);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            try
            {

                var result = JsonSerializer.Deserialize<ResponseLoginDto>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result.response;
            }
            catch (Exception e)
            {
                throw;

            }



        }

        [Fact]
        public async Task RegisterPatient_Unauthorized()
        {
            // Arrange
            var newPatientDto = _fixture.Create<RegisterPatientDto>();

            // Convert DTO to JSON content
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newPatientDto),
                                             System.Text.Encoding.UTF8, "application/json");

            // Act
            // 5. Send the request to your API endpoint
            var response = await _client.PostAsync("/Patient", content);

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);

        }


        [Fact]
        public async Task RegisterPatient_Sucess()
        {

            string token = await this.GetToken();

            // Arrange
            Random random = new Random();
            _fixture.Customize<RegisterPatientDto>(c => c
                .With(x => x.Docto, Constants.Docto)
                .With(x => x.PhoneNumber, Constants.Phone)
                .With(x => x.BirthDate, DateTime.Now.AddYears(-1 * random.Next(50)))  
                );

            var newPatientDto = _fixture.Create<RegisterPatientDto>();

            
        
            // Convert DTO to JSON content
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newPatientDto),
                                             System.Text.Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            // Act
            // 5. Send the request to your API endpoint
            var response = await _client.PostAsync("/Patient", content);

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);

        }



        [Fact]
        public async Task GetPatientByDocto_Sucess()
        {

            string token = await this.GetToken();

            // Arrange
            Random random = new Random();
            _fixture.Customize<RegisterPatientDto>(c => c
                .With(x => x.Docto, Constants.Docto)
                .With(x => x.PhoneNumber, Constants.Phone)
                .With(x => x.BirthDate, DateTime.Now.AddYears(-1 * random.Next(50)))
                );

            var newPatientDto = _fixture.Create<RegisterPatientDto>();



            // Convert DTO to JSON content
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newPatientDto),
                                             System.Text.Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            // Act
            // 5. Send the request to your API endpoint
            var response = await _client.PostAsync("/Patient", content);

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);


            response = await _client.GetAsync($"/Patient?docto={newPatientDto.Docto}");

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);




        }
    }



    public class ResponseLoginDto
    {
        public string response { get; set; }
    }

}
