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

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<UserDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users
                    //.Where(x => x.Email != "admin@reactshows.com")
                    .Select(x => new UserDTO 
                    {
                        Id=x.Id,
                        UserEmail=x.Email,
                        Role = "User"
                    }).ToListAsync();

                return users;
            }
        }
    }
}
