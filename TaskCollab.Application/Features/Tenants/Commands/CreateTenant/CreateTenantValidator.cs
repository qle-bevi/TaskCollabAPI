using FluentValidation;
using TaskCollab.Application.Interfaces;
using TaskCollab.Domain.Interfaces;

namespace TaskCollab.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IIdentityService _identityService;

    public CreateTenantValidator(ITenantRepository tenantRepository, IIdentityService identityService)
    {
        _tenantRepository = tenantRepository;
        _identityService = identityService;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Le nom est requis.")
            .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères.");

        RuleFor(v => v.Slug)
            .NotEmpty().WithMessage("Le slug est requis.")
            .MaximumLength(50).WithMessage("Le slug ne peut pas dépasser 50 caractères.")
            .Matches(@"^[a-z0-9-]+$").WithMessage("Le slug ne peut contenir que des lettres minuscules, des chiffres et des tirets.")
            .MustAsync(SlugNotExist).WithMessage("Ce slug est déjà utilisé.");

        RuleFor(v => v.OwnerEmail)
            .NotEmpty().WithMessage("L'e-mail du propriétaire est requis.")
            .EmailAddress().WithMessage("Veuillez fournir une adresse e-mail valide.")
            .MustAsync(EmailNotExist).WithMessage("Cette adresse e-mail est déjà utilisée.");

        RuleFor(v => v.OwnerFirstName)
            .NotEmpty().WithMessage("Le prénom du propriétaire est requis.")
            .MaximumLength(50).WithMessage("Le prénom ne peut pas dépasser 50 caractères.");

        RuleFor(v => v.OwnerLastName)
            .NotEmpty().WithMessage("Le nom de famille du propriétaire est requis.")
            .MaximumLength(50).WithMessage("Le nom de famille ne peut pas dépasser 50 caractères.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Le mot de passe est requis.")
            .MinimumLength(8).WithMessage("Le mot de passe doit comporter au moins 8 caractères.")
            .Matches(@"[A-Z]+").WithMessage("Le mot de passe doit contenir au moins une majuscule.")
            .Matches(@"[a-z]+").WithMessage("Le mot de passe doit contenir au moins une minuscule.")
            .Matches(@"[0-9]+").WithMessage("Le mot de passe doit contenir au moins un chiffre.");
    }

    private async Task<bool> SlugNotExist(string slug, CancellationToken cancellationToken)
    {
        return !await _tenantRepository.ExistsBySlugAsync(slug);
    }

    private async Task<bool> EmailNotExist(string email, CancellationToken cancellationToken)
    {
        return !await _identityService.UserExistsAsync(email);
    }
}