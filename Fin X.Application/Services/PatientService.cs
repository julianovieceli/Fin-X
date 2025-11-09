using AutoMapper;
using Fin_X.Domain;
using Fin_X.Domain.Interfaces;
using Fin_X.Dto;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Personal.Common.Domain;
using Personal.Common.Domain.Validators;
using Personal.Common.Services;

namespace Fin_X.Application.Services
{
    public class PatientService : BaseService, IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IExamRepository _examRepository;
        private readonly IPatientHistoryRepository _patientHistoryRepository;

        private readonly IMemoryCache _memoryCache; // Inject IMemoryCache


        private readonly IValidator<RegisterPatientDto> _patientValidator;


        private async Task<Exam?> GetExamByCode(string code)
        {
            Exam? exam;
            _logger.LogInformation($"Getting code: {code} from MC");
            if (!_memoryCache.TryGetValue(code, out exam))
            {
                _logger.LogInformation($"Not found in MC, getting from Mongo");

                exam = await _examRepository.GetExamByCodeAsync(code);
                if (exam is not null)
                {
                    _logger.LogInformation($"Found in Mongo. Setting in MC");

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                    _memoryCache.Set(code, exam, cacheEntryOptions);
                }
                else
                    _logger.LogError($"Not Found in MC even mongo");
            }

            return exam;
        }

        public PatientService(ILogger<PatientService> logger, IPatientRepository patientRepository, IMapper dataMapper,
            IPatientHistoryRepository patientHistoryRepository, IExamRepository examRepository,
            IValidator<RegisterPatientDto> patientValidator, IMemoryCache memoryCache) : base(logger, dataMapper)
        {
            this._patientRepository = patientRepository;
            this._patientHistoryRepository = patientHistoryRepository;
            this._examRepository = examRepository;

            _patientValidator = patientValidator;
            _memoryCache = memoryCache; 
        }



        public async Task<Result> RegisterAsync(RegisterPatientDto registerPatientDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(registerPatientDto, "registerPatientDto");

                var validatorResult = _patientValidator.Validate(registerPatientDto);
                if (!validatorResult.IsValid)
                {
                    return Result.Failure("400", validatorResult.Errors.FirstOrDefault().ErrorMessage);//Erro q usuario ja existe com este documento.
                }

                if (await _patientRepository.GetCountByDocto(registerPatientDto.Docto) == 0)
                {

                    Patient patient = new Patient(registerPatientDto.Name, registerPatientDto.Docto, registerPatientDto.BirthDate, registerPatientDto.PhoneNumber);


                    patient = await _patientRepository.InsertAsync(patient);
                    

                    var response = _dataMapper.Map<ResponsePatientDto>(patient);
                    return Result<ResponsePatientDto>.Success(response);

                }

                return Result.Failure("400", "Ja existe paciente com este documento");//Erro q usuario ja existe com este documento.
            }
            catch (Exception e)
            {
                return Result.Failure("666", e.Message);
            }

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


        public async Task<Result> Delete(string Id)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(Id, "Id");

                var list = await _patientHistoryRepository.GetHistoryByPatientId(Id);
                if (list.Count > 0)
                    return Result.Failure("400", "Paciente possui histórico de atendimentos e não pode ser deletado.");

                if (await _patientRepository.Delete(Id) > 0)
                {
                    return Result.Success;
                }

                return Result.Failure("400", "Paciente com este Id não encontrado ou ja deletado");//Erro q usuario ja existe com este documento.
            }
            catch (Exception e)
            {
                return Result.Failure("666", e.Message);
            }
        }


        public async Task<Result> GetAllPatientsAsync()
        {
            try
            {

                var patientList = await _patientRepository.GetAllPatientsAsync();

                if (patientList.Count == 0)
                    return Result.Failure("404", "Nenhum paciente encontrado");


                IList<ResponsePatientDto> list = patientList.Select(c => _dataMapper.Map<ResponsePatientDto>(c)).ToList();

                return Result<IList<ResponsePatientDto>>.Success(list);
            }
            catch
            {
                throw;
            }
        }


        public async Task<Result> RegisterPatientHistoryAsync(string user, RegisterPatientHistoryDto registerPatientHistoryDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(registerPatientHistoryDto, "registerPatientHistoryDto");

                //var validatorResult = _patientValidator.Validate(registerPatientDto);
                //if (!validatorResult.IsValid)
                //{
                //    return Result.Failure("400", validatorResult.Errors.FirstOrDefault().ErrorMessage);//Erro q usuario ja existe com este documento.
                //}

                if(!Enum.IsDefined(typeof(Domain.PlaceId), (int)registerPatientHistoryDto.PlaceId))
                    return Result.Failure("400", $"PlaceId inválido: {registerPatientHistoryDto.PlaceId}. Valid places: 1 = Clinic, 2 = Laboratory, 3 = Hospital");

                Domain.PlaceId placeId = Enum.Parse<Domain.PlaceId>(registerPatientHistoryDto.PlaceId.ToString());

                Patient patient = await _patientRepository.Get(registerPatientHistoryDto.PatientId);

                if(patient is null)
                {
                    return Result.Failure("400", $"Paciente não encontrado. PatientDocumentId: {registerPatientHistoryDto.PatientId}");
                }

                IList<Exam>? exams = new List<Exam>();

                if (registerPatientHistoryDto.Exams is not null)
                {
                    foreach (string exam in registerPatientHistoryDto.Exams)
                    {
                        var examFromMc = await this.GetExamByCode(exam);
                        if (examFromMc is null)
                        {
                            return Result.Failure("400", $"Exame não encontrado. ExamCode: {exam}");
                        }
                        exams.Add(examFromMc);
                    }
                }

                PatientHistory patientHistory = new PatientHistory(patient, user, registerPatientHistoryDto.Diagnostic, 
                    registerPatientHistoryDto.Prescription, placeId, exams);


                PatientHistory patientHistoryInserted = await _patientHistoryRepository.InsertAsync(patientHistory);
                patientHistoryInserted.Patient = patient;


                var response = _dataMapper.Map<ResponsePatientHistoryDto>(patientHistoryInserted);
                return Result<ResponsePatientHistoryDto>.Success(response);


            }
            catch (Exception e)
            {
                return Result.Failure("666", e.Message);
            }
        }


        public async Task<Result> GetHistoryByPatientId(string patientId)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(patientId);


                var patientList = await _patientHistoryRepository.GetHistoryByPatientId(patientId);


                IList<ResponsePatientHistoryDto> list = patientList.Select(c => _dataMapper.Map<ResponsePatientHistoryDto>(c)).ToList();

                return Result<IList<ResponsePatientHistoryDto>>.Success(list);
            }
            catch
            {
                throw;
            }
        }
    }
}
