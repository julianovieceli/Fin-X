using AutoMapper;
using Fin_X.Domain.Interfaces;
using Fin_X.Dto;
using Microsoft.Extensions.Logging;
using Personal.Common.Domain;
using Personal.Common.Domain.Validators;
using Personal.Common.Services;

namespace Fin_X.Application.Services
{
    public class PatientService : BaseService, IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(ILogger<PatientService> logger, IPatientRepository patientRepository, IMapper dataMapper) : base(logger, dataMapper)
        {
            this._patientRepository = patientRepository;
        }



        public async Task<Result> RegisterAsync(RegisterPatientDto patient)
        {

            return Result.Success;

            //try
            //{
            //    ArgumentNullException.ThrowIfNull(registerClient, "registerClient");

            //    var validatorResult = _clientValidator.Validate(registerClient);
            //    if (!validatorResult.IsValid)
            //    {
            //        return Result.Failure("400", validatorResult.Errors.FirstOrDefault().ErrorMessage);//Erro q usuario ja existe com este documento.
            //    }

            //    if (await _clientRepository.GetCountByDocto(registerClient.Docto) == 0)
            //    {

            //        Client client = new Client(registerClient.Name, registerClient.Docto, registerClient.Age);

            //        if (!await _clientRepository.Register(client))
            //            return Result.Failure("999");


            //        await _rabbitMqMessagingClientStrategyService.SendMessage(client);
            //        await _azureMessagingClientService.SendMessage(client);
            //        await _aWSMessagingClientService.SendMessage(client);


            //        return Result.Success;

            //    }

            //    return Result.Failure("400", "Ja existe cliente com este documento");//Erro q usuario ja existe com este documento.
            //}
            //catch (Exception e)
            //{
            //    return Result.Failure("999", e.Message, System.Net.HttpStatusCode.InternalServerError);
            //}

        }


        public async Task<Result> Get(string docto)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(docto, "docto");

                if (!DocumentValidator.IsCpfCnpjValid(docto))
                    return Result.Failure("400", "Documento inválido");

                var patient = await _patientRepository.GetByDocto(docto);

                if (patient is null)
                    return Result.Failure("404", "Paciente não encontrado"); //erro nao encontrado
                else
                {
                    var patientDto = _dataMapper.Map<ResponsePatientDto>(patient);

                    return Result<ResponsePatientDto>.Success(patientDto);
                }

            }
            catch (ArgumentNullException exArg)
            {
                return Result.Failure("400", "Documento obrigatório");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar um paciente");
                return Result.Failure("666", "Erro ao consultar um Paciente");
            }
        }



    }
}
