using FluentValidation;

namespace MongoDBAssesmentDomain.Entity
{
    public class Users
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UsersValidator : AbstractValidator<Users>
    {
        public UsersValidator()
        {
            RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("The User Name is required.")
            .MinimumLength(5)
            .WithMessage("The User Name must be at least 6 characters long.")
            .Must(str => !str.Contains(" "))
            .WithMessage("The User Name cannot contain any spaces.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Please enter Email")
                .EmailAddress().WithMessage("Please enter a valid Email");

            RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Birth date cannot be empty")
            .Must(date => date <= DateTime.Now)
            .WithMessage("Birth date cannot be in the future");

            RuleFor(x => x.DateOfBirth)
                .Must(date => (DateTime.Now - date).TotalDays >= 365 * 18)
                .WithMessage("You must be 18 or older to use this service");
        }
    }
}
