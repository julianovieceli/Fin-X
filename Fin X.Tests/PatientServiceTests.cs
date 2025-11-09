using AutoMapper;
using Fin_X.Application.Services;
using Fin_X.Domain;
using Fin_X.Domain.Interfaces;
using Fin_X.Dto;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Personal.Common.Domain;

namespace Fin_X.Tests
{
    public class PatientServiceTests
    {
        // Mocks
        private readonly Mock<ILogger<PatientService>> _mockLogger;
        private readonly Mock<IPatientRepository> _mockPatientRepository;
        private readonly Mock<IPatientHistoryRepository> _mockPatientHistoryRepository;
        private readonly Mock<IExamRepository> _mockExamRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<RegisterPatientDto>> _mockPatientValidator;
        private readonly Mock<IMemoryCache> _mockMemoryCache;

        // Service Under Test
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            
            _mockLogger = new Mock<ILogger<PatientService>>();
            _mockPatientRepository = new Mock<IPatientRepository>();
            _mockPatientHistoryRepository = new Mock<IPatientHistoryRepository>();
            _mockExamRepository = new Mock<IExamRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockPatientValidator = new Mock<IValidator<RegisterPatientDto>>();
            _mockMemoryCache = new Mock<IMemoryCache>();

            _service = new PatientService(
                _mockLogger.Object,
                _mockPatientRepository.Object,
                _mockMapper.Object,
                _mockPatientHistoryRepository.Object,
                _mockExamRepository.Object,
                _mockPatientValidator.Object,
                _mockMemoryCache.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_DocumentoInvalido()
        {
            // Arrange
            var dto = new RegisterPatientDto { Docto = "123" }; // DTO inválido
            var errorMessage = "Docto deve ter 11 dígitos.";

            var validationFailure = new ValidationFailure("Docto", errorMessage);
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            // Setup: Validador retorna falha
            _mockPatientValidator.Setup(v => v.Validate(dto))
                .Returns(validationResult); // IsValid = false

            // Act
            Result result = await _service.RegisterAsync(dto);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("400", result.ErrorCode);
            Assert.Equal(errorMessage, result.ErrorMessage);

            // Verifica se o repositório NUNCA foi chamado
            _mockPatientRepository.Verify(r => r.GetCountByDocto(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_Sucess()
        {
            // Arrange
            var dto = new RegisterPatientDto
            {
                Docto = Constants.Docto,
                Name = "Novo Teste",
                BirthDate = DateTime.Now.AddYears(-30),
                PhoneNumber = Constants.Phone
            };
            var patient = new Patient(dto.Name, dto.Docto, dto.BirthDate, dto.PhoneNumber);
            var responseDto = new ResponsePatientDto { Name = dto.Name, Docto = dto.Docto, PhoneNumber = dto.PhoneNumber };

            // Setup: Validador retorna sucesso (válido)
            _mockPatientValidator.Setup(v => v.Validate(dto))
                .Returns(new ValidationResult()); // IsValid = true

            // Setup: Repositório retorna 0, indicando que o paciente NÃO existe
            _mockPatientRepository.Setup(r => r.GetCountByDocto(dto.Docto))
                .ReturnsAsync(0);

            // Setup: Repositório retorna o objeto paciente inserido
            _mockPatientRepository.Setup(r => r.InsertAsync(It.IsAny<Patient>()))
                .ReturnsAsync(patient);

            // Setup: Mapeador retorna o DTO de resposta
            _mockMapper.Setup(m => m.Map<ResponsePatientDto>(patient))
                .Returns(responseDto);

            // Act
            var result = await _service.RegisterAsync(dto);

            // Assert
            Assert.False(result.IsFailure);

            Assert.True(((Result<ResponsePatientDto>)result).Response.Docto == dto.Docto);
            Assert.True(((Result<ResponsePatientDto>)result).Response.PhoneNumber == dto.PhoneNumber);

            // Verifica se a inserção no repositório foi chamada
            _mockPatientRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Once);
        }
    }
}