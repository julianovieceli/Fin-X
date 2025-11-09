using Fin_X.Application.Settings;
using Fin_X.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Personal.Common.Domain;
using System.Net;
using System.Text.Json;

namespace Fin_X.Application.Api
{
    public class BrasilApiService: IBrasilApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BrasilApiSettings _brasilApiSettings;
        private readonly ILogger<BrasilApiService> _logger;

        public BrasilApiService(IHttpClientFactory httpClientFactory,
        IOptions<BrasilApiSettings> brasilApiSettings,
        ILogger<BrasilApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _brasilApiSettings = brasilApiSettings.Value;
            _logger = logger;
        }

        public async Task<Result> GetAddress(string cep)
        {

            try
            {
                var httpClient = _httpClientFactory.CreateClient(nameof(HttpClientEnum.API_BRASIL));

                string servicePath = string.Format(_brasilApiSettings.UrlCep, cep);

                var response = await httpClient.GetAsync(servicePath);

                return await HandleResponseAsync(response);
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                throw;
            }
        }

        private async Task<Result> HandleResponseAsync(HttpResponseMessage response)
        {
            try
            {
                var responseAsString = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Sucesso ao acessar a API");

                    var result = JsonSerializer.Deserialize<ResponseSucessAddressDto?>(responseAsString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Result<ResponseSucessAddressDto>.Success(result);
                }
                else
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError("404 endereco não encontrado");
                    return Result.Failure("404", "Endereço não encontrado");
                }
                else
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogError("403 Forbiden");
                    return Result.Failure("403", "Forbiden");
                }
                else

                {
                    if (string.IsNullOrWhiteSpace(responseAsString))
                        throw new Exception("No content response");

                    var result = JsonSerializer.Deserialize<ResponseErrorAddressDto?>(responseAsString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Result.Failure(result.errors[0].service, result.errors[0].message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao processar a resposta da API");
                throw;
            }
        }
    }
}
