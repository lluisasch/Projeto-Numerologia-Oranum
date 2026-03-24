using Oranum.Application.DTOs.Requests;
using Oranum.Domain.Exceptions;

namespace Oranum.Application.Validators;

public sealed class ReadingRequestValidator
{
    public void Validate(NameReadingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Trim().Length < 2)
        {
            throw new DomainValidationException("Informe um nome com pelo menos 2 caracteres.");
        }
    }

    public void Validate(BirthDateReadingRequest request)
    {
        Validate(new NameReadingRequest(request.FullName));

        if (request.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            throw new DomainValidationException("A data de nascimento nao pode estar no futuro.");
        }
    }

    public void Validate(CompatibilityReadingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Person1Name) || request.Person1Name.Trim().Length < 2)
        {
            throw new DomainValidationException("Informe o primeiro nome com pelo menos 2 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Person2Name) || request.Person2Name.Trim().Length < 2)
        {
            throw new DomainValidationException("Informe o segundo nome com pelo menos 2 caracteres.");
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        if (request.Person1BirthDate.HasValue && request.Person1BirthDate.Value > today)
        {
            throw new DomainValidationException("A primeira data de nascimento nao pode estar no futuro.");
        }

        if (request.Person2BirthDate.HasValue && request.Person2BirthDate.Value > today)
        {
            throw new DomainValidationException("A segunda data de nascimento nao pode estar no futuro.");
        }
    }
}
