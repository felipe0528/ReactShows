using Application.Errors;
using Application.Interfaces;
using Application.User.DTOs;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
    public class Login
    {
        public class Query : IRequest<UserTokenDTO>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query> 
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }


        public class Handler : IRequestHandler<Query, UserTokenDTO>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IJwtGenerator jwtGenerator)
            {
                _signInManager = signInManager;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UserTokenDTO> Handle(Query request,
                CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var result = await _signInManager
                    .CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    // TODO: generate token
                    return new UserTokenDTO
                    {
                        Token = _jwtGenerator.CreateToken(user, roles.FirstOrDefault()),
                        Username = user.UserName
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }

        }

    }
}
