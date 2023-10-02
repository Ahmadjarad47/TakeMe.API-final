using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeMe.Core.DTOs;
using TakeMe.Core.Entities;
using TakeMe.Core.Helper;
using TakeMe.Core.Interfaces;
using TakeMe.DTOs;
using TakeMe.Error;
using TakeMe.InferStructuer.Data;

namespace TakeMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUsers> userManager;
        private readonly SignInManager<AppUsers> signInManager;
        private readonly ITokenService token;

        public AccountController(
            IEmailService emailService,
           ApplicationDbContext context, UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager, ITokenService token)
        {
            this.emailService = emailService;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.token = token;
        }
        [HttpGet("usersAll")]
        public async Task<ActionResult> getusers()
        {
           
            return Ok(await context.AppUsers.AsNoTracking().ToListAsync());
        }
        [HttpPut("Login")]
        public async Task<IActionResult> Login(AppUsersDTO usersDTO)
        {
            AppUsers user = await context.AppUsers.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email == usersDTO.Email);
            // user is null ?
            if (user is null) return Unauthorized(new
                BaseComonentResponse(401, "this email not found !"));
            // hassing password
            if (!PasswordHasher.VerifyPassword(usersDTO.password, user.PasswordHash))
            {
                return BadRequest(new BaseComonentResponse(400,"password not matched !"));
            }

            user.refreshToken = token.CreateRefreshToken();
            user.refreshTokenTime = DateTime.Now.AddDays(10);
             context.AppUsers.Update(user);
            await context.SaveChangesAsync();
            return Ok(new TokenAPIDTO(AccessToken: token.GetAndCreateToken(token: user), user.UserName, user.Email, 200));
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Registered(RegisterDTO registerDTO)
        {
            if (string.IsNullOrEmpty(registerDTO.Email) || await checkEmail(registerDTO.Email))
            {
                return Unauthorized(new BaseComonentResponse(400, "the email is already registred"));
            }
            AppUsers app = new AppUsers
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                NormalizedUserName = registerDTO.Email.ToUpper(),
                NormalizedEmail = registerDTO.Email.ToUpper(),
                PasswordHash=PasswordHasher.HashPassword(registerDTO.password),
            };
            await token.AddUserAsync(app);
           /* await context.AppUsers.AddAsync(app);
            await context.SaveChangesAsync();*/
            return Ok(new TokenAPIDTO(AccessToken: token.GetAndCreateToken(token: app), app.UserName, app.Email,200));
        }

        [HttpPost("checkEmail")]
        private async Task<bool> checkEmail(string email)
        {
            var check = await context.AppUsers.AsNoTracking().AnyAsync(e => e.Email == email);
            return check;
        }
        [HttpPost("send-reset-email")]
        public async Task<IActionResult> SendEmail(sendEmailDTO emailDTO)
        {
            AppUsers user = await context.AppUsers.
                FirstOrDefaultAsync(x => x.Email == emailDTO.email);
            if (user is null)
            {
                return NotFound(new BaseComonentResponse(404, "Email not Found"));
            }
            string randomtoken = DateTime.Now.ToFileTimeUtc().ToString();

            var emailTokne = randomtoken.Substring(9, 6);
            user.ResetPasswordToken = emailTokne.ToString();
            user.ResetPasswordTokenExpier = DateTime.Now.AddMinutes(15);
            string from = "ahmad222jarad@gmail.com";
            EmailModelDTO emaliModel = new EmailModelDTO(emailDTO.email, "Reset Password", EmailBody.EmailStringBody(emailTokne));
            emailService.sendEmail(emaliModel);
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Email Sent"
            });
        }
        [HttpPost("reset-paswword")]
        public async Task<IActionResult> resetPassword(checkDTO dto)
        {
            AppUsers user = await context.AppUsers.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email == dto.email);
            if (user is null)
            {
                return NotFound(new BaseComonentResponse(404, $"this Email is't registered"));
            }
            if (user.ResetPasswordTokenExpier <= DateTime.Now || dto.code != user.ResetPasswordToken)
            {
                return Unauthorized(new BaseComonentResponse(401, "you code is expier"));
            }
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await userManager.ResetPasswordAsync(user, token, dto.password);
            if (result.Succeeded is false)
            {
                return BadRequest(new BaseComonentResponse(400, "something went wrong !"));
            }
            user.ResetPasswordToken = string.Empty;

            await context.SaveChangesAsync();

            return Ok(new BaseComonentResponse(200, "password change successfly"));

        }
        [HttpPost("check-verify-code")]
        public async Task<bool> checkcode(checkDTO check)
        {
            AppUsers user = await context.AppUsers.AsNoTracking()
                .Where(x => x.Email == check.email).FirstOrDefaultAsync();
            if (user.ResetPasswordTokenExpier >= DateTime.Now || check.code == user.ResetPasswordToken)
            {
                return true;
            }
            return true;
        }


        [HttpPost("refresh-token")]
        /*   public async Task<IActionResult> GenerateRefreshToken(TokenAPIDTO token)
           {
               if (token is null)
               {
                   return BadRequest(new BaseComonentResponse(400, "Token Invalid"));

               }
               string accessToken = token.AccessToken;
               string refresh = token.RefreshToken;
               var prncip = this.token.GetPrincipalFromRefreshToken(accessToken);
               string userName = prncip.Identity.Name;

               AppUsers user = await context.AppUsers.AsNoTracking()
                   .FirstOrDefaultAsync(u => u.UserName == userName);

               if (user is null || token.RefreshToken != refresh || user.refreshTokenTime <= DateTime.Now)
                   return BadRequest(new BaseComonentResponse(400, "Invalid Reqruest"));



               string newAccessToken = this.token.GetAndCreateToken(user);
               string newRefresh = this.token.CreateRefreshToken();
               user.refreshToken = newRefresh;
               await context.SaveChangesAsync();

               return Ok(new TokenAPIDTO(accessToken, refresh));



           }*/
        [HttpPut("update-account")]
        public async Task<ActionResult> UpdateDetais(UpdateAccountDTO dTO)
        {
            var check = await context.AppUsers.AsNoTracking().FirstOrDefaultAsync(m=>m.Email==dTO.Email);
            if (check is null)
            {
                return BadRequest(new BaseComonentResponse(400, $"Invalid Reqruest this Email {dTO.Email} not Registerd"));
            }
            if (!PasswordHasher.VerifyPassword(dTO.password, check.PasswordHash))
            {
                return BadRequest(new BaseComonentResponse(400, "password not matched !"));
            }
            if (await CheckUserName(dTO.UserName))
            {
                return BadRequest(new BaseComonentResponse(400, $"Invalid Reqruest this userName {dTO.UserName} is already Registerd"));
            }
            AppUsers checks = check;
                checks = new()
                {
                    Email = dTO.Email,
                    PhoneNumber = dTO.PhoneNumber,
                    UserName = dTO.UserName,
                    CollageName= dTO.CollageName,
                    NormalizedEmail=dTO.Email.ToUpper(),
                    NormalizedUserName=dTO.UserName.ToUpper(),  
                    PasswordHash=PasswordHasher.HashPassword(dTO.password)
                };
            token.DeleteUser(check.Id);
            await context.AddAsync(checks);
            await context.SaveChangesAsync();
                return Ok(new BaseComonentResponse(200));
        }
        [HttpPost("CheckUserName")]
        private async Task<bool> CheckUserName(string userName)
        {
            var check = await context.AppUsers.AsNoTracking().AnyAsync(e => e.UserName == userName);
            return check;
        }
    }
}
