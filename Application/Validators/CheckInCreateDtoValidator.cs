using Application.DTOs.CheckInDtos;
using FluentValidation;

namespace Application.Validators
{
    public class CheckInCreateDtoValidator : AbstractValidator<CheckInCreateDto>
    {
        public CheckInCreateDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than zero.");
        }
    }
}
