using Application.User;
using Application.User.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTO>> Login(Login.Query query)
        {
            try
            {
                _logger.LogInformation($"LOGIN: {query.Email} TRYING TO JOIN");
                var user = await Mediator.Send(query);
                _logger.LogInformation($"LOGIN: {query.Email} SUCCESS");
                return user;
            }
            catch (Exception e )
            {
                _logger.LogInformation($"LOGIN:{query.Email} ERROR: {e.Message}");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserTokenDTO>> Register(Register.Command command)
        {
            try
            {
                _logger.LogInformation($"REGISTER: {command.Email} TRYING TO register ");
                var user = await Mediator.Send(command);
                _logger.LogInformation($"REGISTER: {command.Email} SUCCEDED");
                return user;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"REGISTER: {command.Email} ERROR: {e.Message}");

                throw;
            }
        }

        [HttpGet("current")]
        public async Task<ActionResult<UserTokenDTO>> CurrentUser()
        {
            try
            {
                _logger.LogInformation($"GETCURRENT: from token");
                var user = await Mediator.Send(new CurrentUser.Query());

                _logger.LogInformation($"GETCURRENT: {user.Username} SUCCEDED");
                return user;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"GETCURRENT:  ERROR: {e.Message}");

                throw;
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UserDTO>> Details(string id)
        {
            try
            {
                _logger.LogInformation($"DETAILS: {id} TRYING TO GET DETAILS");
                return await Mediator.Send(new Details.Query { Id = id });
            }
            catch (Exception e)
            {
                _logger.LogInformation($"DETAILS: {id} ERROR: {e.Message}");

                throw;
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Unit>> Edit(string id, Edit.Command command)
        {
            try
            {
                _logger.LogInformation($"EDIT: {id} TRYING TO EDIT");
                return await Mediator.Send(command);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"EDIT: {id} ERROR: {e.Message}");

                throw;
            }
            command.Id = id;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            try
            {
                _logger.LogInformation($"DELETE: {id} TRYING TO DELETE");
                return await Mediator.Send(new Delete.Command { Id = id });

            }
            catch (Exception e)
            {
                _logger.LogInformation($"DELETE: {id} ERROR: {e.Message}");

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<UserDTO>>> List()
        {
            try
            {
                _logger.LogInformation($"USERLIST:  TRYING TO GET USERS");
                var user= await Mediator.Send(new List.Query());

                _logger.LogInformation($"USERLIST:  SUCCEDED");
                return user;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"USERLIST: ERROR: {e.Message}");

                throw;
            }
        }
    }
}
