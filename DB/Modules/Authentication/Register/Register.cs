using DB.Domain;
using DB.Domain.Entities;

using FluentValidation;

using MediatR;

namespace DB.Modules.Authentication.Register
{
    public class Register
    {
        public class Request : IRequest
        {
            public string UserName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";

            public string PasswordRepeat { get; set; } = "";
        }
        public class RegisterHandler : IRequestHandler<Request>
        {
            private readonly AppDbContext _dbContext;
            public RegisterHandler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var entity = new User()
                {
                    Email = request.Email,
                    Password = request.Password,
                    Role = SD.Roles.User,
                    Username = request.UserName,
                };
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public class RegisterUser : AbstractValidator<Request>
        {
            public RegisterUser(AppDbContext dbContext)
            {
                RuleFor(x => x.Email)
                     .NotEmpty().WithMessage("Email is required.")
                     .EmailAddress().WithMessage("Email is not valid.")
                     .Must(email => !dbContext.Users.Any(u => u.Email == email)).WithMessage("Email already exists.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password required");


                RuleFor(x => x.PasswordRepeat)
                    .NotEmpty().WithMessage("Repeat password required");

                RuleFor(x => x)
                    .Must(x => x.Password == x.PasswordRepeat).WithMessage("Passwords must be same");
            }
        }

    }
}
