using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
    public class CurrentUser
    {
        public class Query : IRequest<UserTokenDTO> { }

        public class Handler : IRequestHandler<Query, UserTokenDTO>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
            }

            public async Task<UserTokenDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var roles = await _userManager.GetRolesAsync(user);

                return new UserTokenDTO
                {
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user, roles.FirstOrDefault())
                };
            }
        }
    }
}
