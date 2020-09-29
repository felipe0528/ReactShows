using Application.User.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
    public class List
    {
        public class Query : IRequest<List<UserDTO>> { }


        public class Handler : IRequestHandler<Query,List<UserDTO>>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;

            public Handler(DataContext context, UserManager<AppUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<List<UserDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.ToListAsync();

                var usersDTO = users
                    .Select(x => new UserDTO 
                    {
                        Id=x.Id,
                        UserEmail=x.Email,
                        Role = ""
                    }).ToList();


                foreach (var item in usersDTO)
                {
                    var roles = await _userManager.GetRolesAsync(users.Where(x=>x.Id==item.Id).FirstOrDefault());
                    item.Role = String.Join(", ", roles.ToArray());

                }

                return usersDTO;
            }
        }
    }
}
