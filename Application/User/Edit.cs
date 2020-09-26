using Application.Errors;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
    public class Edit
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
            public string Email { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
            }
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

                user.UserName = request.Email;
                user.Email = request.Email;

                // Persiste the changes
                var result = await _userManager.UpdateAsync(user);

                var success = result.Succeeded;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
