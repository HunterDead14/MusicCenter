using FluentValidation;

namespace Application.Albums.Commands;

public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
{
    public CreateAlbumCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Artist).NotEmpty().MinimumLength(1).MaximumLength(255);
        RuleFor(x => x.TracksCount).InclusiveBetween(1, 100);
        RuleFor(x => x.ReleaseDate).Must(x => x != default)
            .WithMessage("'Release Date' must be a valid date.");
        RuleFor(x => x.Genre).IsInEnum();
    }
}
