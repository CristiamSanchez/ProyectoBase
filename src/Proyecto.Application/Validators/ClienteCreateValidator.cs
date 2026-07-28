using FluentValidation;
using Proyecto.Application.DTOs.Clientes;

namespace Proyecto.Application.Validators;

public class ClienteCreateValidator : AbstractValidator<ClienteCreateDto>
{
    public ClienteCreateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(50)
            .WithMessage("El nombre no puede superar 50 caracteres");


        RuleFor(x => x.Apellido)
            .NotEmpty()
            .WithMessage("El apellido es obligatorio")
            .MaximumLength(50);


        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El email es obligatorio")
            .EmailAddress()
            .WithMessage("El email no tiene un formato válido");
    }
}