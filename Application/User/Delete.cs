using Application.Errors;
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
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly UserManager<AppUser> _userManager;
            public Handler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.Id);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { AppUser = "Not found" });

                //Gets list of Roles associated with current user
                var rolesForUser = await _userManager.GetRolesAsync(user);

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }

                //Delete User
                var success = await _userManager.DeleteAsync(user); ;

                if (success.Succeeded) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
