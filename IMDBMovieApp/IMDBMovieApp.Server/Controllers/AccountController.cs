using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Server.DTO;
using MovieApp.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private ApplicationDBContext _context;
        private UserManager<MovieUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountController(ApplicationDBContext context, UserManager<MovieUser> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(RegisterUserDTO userInput)
        {
            if (ModelState.IsValid)
            {
                var newUser = new MovieUser();
                newUser.UserName = userInput.Username;
                newUser.Email = userInput.Email;
              

                var result = await _userManager.CreateAsync(newUser, userInput.Password);

                if (result.Succeeded) 
                {
                    return StatusCode(201, $"New User {newUser.UserName} has been created");
                }
   
                
                
            }
            return BadRequest("User can't be created");
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDTO loginInput)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(loginInput.Username);

                    if (user == null || !await _userManager.CheckPasswordAsync(user, loginInput.Password))
                    {
                        throw new Exception("Invalid login attempt!");
                    }
                    else
                    {
                        var signingCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(
                                System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                                SecurityAlgorithms.HmacSha256
                                );

                        var claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.AddRange((await _userManager.GetRolesAsync(user)).Select(r => new Claim(ClaimTypes.Role, r)));

                        var jwtObject = new JwtSecurityToken(
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(60),
                            signingCredentials: signingCredentials);

                        var jwtString = new JwtSecurityTokenHandler()
                            .WriteToken(jwtObject);

                        return StatusCode(StatusCodes.Status200OK, jwtString);
                    }
                }
                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);
                }
            }catch(Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status = StatusCodes.Status401Unauthorized;

                return StatusCode(StatusCodes.Status401Unauthorized, exceptionDetails);
            }
        }

    }
}
