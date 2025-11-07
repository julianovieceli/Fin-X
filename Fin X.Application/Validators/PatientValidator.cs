using Fin_X.Dto;
using FluentValidation;
using Personal.Common.Domain.Validators;

namespace Porter.Application.Validators
{
    public class PatientValidator: AbstractValidator<RegisterPatientDto>
    {
        public PatientValidator()
        {
            RuleFor(patient => patient.Name).NotNull().NotEmpty().
                Custom(
                    (name, context) =>
                    {
                        if (string.IsNullOrWhiteSpace(name) || name.Length < 5)
                        {
                            throw new ArgumentException("Name deve conter ao menos 5 caracteres.", nameof(name));
                        }
                    });

            RuleFor(patient => patient.Docto).NotNull().NotEmpty().
                Custom(
                    (docto, context) =>
                    {
                        if (!DocumentValidator.IsCpfCnpjValid(docto))
                        {
                            context.AddFailure("Documento é obrigatório e deve conter 11 dígitos (CPF) ou 14 dígitos (CNPJ).");
                        }
                    });

            RuleFor(patient => patient.BirthDate).NotNull().NotEmpty().
            Custom(
                (birthDate, context) =>
                {
                    if (birthDate.Date >= DateTime.Now.Date)
                        throw new Exception("BirthDate deve ser menor que a atual!");
                });

   


        }
    }
}
