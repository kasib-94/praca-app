using DB.Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Authentication.Login
{
    public class Login
    {
        public class Request : IRequest<bool>
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";

        }
        private class Handler : IRequestHandler<Request, bool>
        {
            private readonly AppDbContext _dbContext;
            private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
            public Handler(AppDbContext dbContext, Blazored.LocalStorage.ILocalStorageService localStorage)
            {
                _dbContext = dbContext;
                _localStorage = localStorage;
            }
            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {

                var item = await _dbContext.Users
                    .AsNoTracking()
                    .FirstAsync(x => x.Username == request.Username && x.Password == request.Password);
                if (item != null)
                {
                    await _localStorage.SetItemAsync<DB.Models.AuthModel>("auth_token", new Models.AuthModel()
                    {
                        Email = item.Email,
                        Id = item.Id,
                        Rola = item.Role,
                        Username = item.Username
                    });
                    return true;
                }
                return false;
            }
        }

        public class RegisterUser : AbstractValidator<Request>
        {
            public RegisterUser(AppDbContext dbContext)
            {

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password required");

                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username required");

            }
        }
    }
}
