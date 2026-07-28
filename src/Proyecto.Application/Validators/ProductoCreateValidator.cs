using FluentValidation;
using Proyecto.Application.DTOs.Productos;

namespace Proyecto.Application.Validators;

public class ProductoCreateValidator 
    : AbstractValidator<ProductoCreateDto>
{
    public ProductoCreateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del producto es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar 100 caracteres");


        RuleFor(x => x.Precio)
            .GreaterThan(0)
            .WithMessage("El precio debe ser mayor que cero");


        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo");
    }
}