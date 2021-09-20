using Demo.API.Domain.Model;
using Demo.API.Domain.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Demo.Common.Logging;
using System;

namespace Demo.API.Domain.Controllers
{
    [EnableCors("Policy1")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ICustomLog _logger;
        private readonly UserService _userService;

        public UserController(ICustomLogFactory logger, UserService userService)
        {
            _logger = logger.CreateLogger<ICustomLogFactory>();
            _userService = userService;
        }


        [HttpPost("signup")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Cadastro([FromBody] User user)
        {
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                user = _userService.Insert(user);

                response = Ok(user);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex); ;
                response = StatusCode(500, ex.Message);
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint \'UQ__Users__A9D105346D5EB3D4\'"))
                {
                    response = StatusCode(500, "Já existe cadastro para esse Email");
                }
            }

            return response;
        }

        [HttpPost("signin")]
        [ProducesResponseType(typeof(Login), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] Login login)
        {
            ObjectResult response;
            User user;

            try
            {
                user = new User();
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                user = _userService.Login(login);

                response = Ok(user);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex); ;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }




    }
}
