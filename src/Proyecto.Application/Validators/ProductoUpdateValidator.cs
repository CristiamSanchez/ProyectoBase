using FluentValidation;
using Proyecto.Application.DTOs.Productos;

namespace Proyecto.Application.Validators;

public class ProductoUpdateValidator
    : AbstractValidator<ProductoUpdateDto>
{
    public ProductoUpdateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del producto es obligatorio");


        RuleFor(x => x.Precio)
            .GreaterThan(0)
            .WithMessage("El precio debe ser mayor que cero");


        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo");
    }
}