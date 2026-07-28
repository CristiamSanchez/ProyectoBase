using FluentValidation;
using Proyecto.Application.DTOs.Clientes;

namespace Proyecto.Application.Validators;

public class ClienteUpdateValidator : AbstractValidator<ClienteUpdateDto>
{
    public ClienteUpdateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio");


        RuleFor(x => x.Apellido)
            .NotEmpty()
            .WithMessage("El apellido es obligatorio");


        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("El email no es válido");
    }
}