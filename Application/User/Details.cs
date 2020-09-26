using Application.Errors;
using Application.User.DTOs;
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
    public class Details
    {
        public class Query : IRequest<UserDTO>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UserDTO>
        {
            private readonly UserManager<AppUser> _userManager;
            public Handler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<UserDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.Id);
                    
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { AppUser = "Not found" });

                var roles = await _userManager.GetRolesAsync(user);

                return new UserDTO
                {
                    Id = user.Id,
                    UserEmail = user.Email,
                    Role = String.Join(", ", roles.ToArray())
                };
            }
        }
    }
}
