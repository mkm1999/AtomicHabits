using Application.UserService.Command.Register;
using Application.UserService.Query.ILoginService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AtomicHabits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly ILogin login;
        private readonly IRegisterUserService registerUserService;

        public AuthenticationController(IConfiguration configuration, ILogin login, IRegisterUserService registerUserService)
        {

            this.configuration = configuration;
            this.login = login;
            this.registerUserService = registerUserService;
        }

        /// <summary>
        /// ورود به سیستم و دریافت توکن
        /// </summary>
        /// <param name="request">اطلاعات ورود</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SignIn(RequestLoginDto request)
        {
            var result = login.Execute(request);
            if (!result.IsSuccess)
            {
                return Unauthorized(result.Message);
            }
            var Claims = new List<Claim>
            {
                new Claim("UserId", result.Data.Id.ToString()),
                new Claim("UserName", result.Data.UserName),
                new Claim(ClaimTypes.Role, result.Data.role)
            };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")));
            var Credentials = new SigningCredentials(secretKey ,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration.GetValue<string>("Jwt:Issuer"),
                configuration.GetValue<string>("Jwt:Issuer"),
                Claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: Credentials
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                message = result.Message
            });
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp(RequestRegisterUserDto request)
        {
            var result = registerUserService.Execute(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(new {
                message = result.Message,
                link = new
                {
                    href = Url.Action(nameof(SignIn),"Authentication",new {UserName = request.UserName , Password = request.Password}),
                    rel = "SignIn",
                    method = "POST",
                }
            });
        }
    }
}
