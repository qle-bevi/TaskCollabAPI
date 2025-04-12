using FluentValidation;

namespace TaskCollab.Application.Features.Auth.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("L'e-mail est requis.")
            .EmailAddress().WithMessage("Veuillez fournir une adresse e-mail valide.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Le mot de passe est requis.");

        RuleFor(v => v.TenantSlug)
            .NotEmpty().WithMessage("L'identifiant de l'organisation est requis.");
    }
}