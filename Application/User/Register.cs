using Application.Errors;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
    public class Register
    {
        public class Command : IRequest<UserTokenDTO>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, UserTokenDTO>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(DataContext context, 
                UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _context = context;
            }

            public async Task<UserTokenDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

                var user = new AppUser
                {
                    Email = request.Email,
                    UserName = request.Email
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "User").Wait();
                    return new UserTokenDTO
                    {
                        Token = _jwtGenerator.CreateToken(user, "User"),
                        Username = user.UserName
                    };
                }

                throw new Exception("Problem creating user");
            }
        }
    }
}
