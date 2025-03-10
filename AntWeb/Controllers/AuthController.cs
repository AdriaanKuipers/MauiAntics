using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using AntCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AntWeb.Controllers;

public class AuthController(IOptionsSnapshot<AppSettings> settingsSnapshot, ILogger<AuthController> logger) : Controller
{
    private readonly IOptionsSnapshot<AppSettings> _settingsSnapshot = settingsSnapshot;
    private readonly ILogger<AuthController> _logger = logger;
    private readonly JwtSettings _jwtSettings = settingsSnapshot.Value.Auth.Jwt;

    [HttpGet]
    [AllowAnonymous]
    public ActionResult Login([FromQuery] string ReturnUrl)
    {
        return View(new LoginViewModel() { ReturnUrl = ReturnUrl });
    }


    [HttpPost]
    [AllowAnonymous]
    public ActionResult Login([FromForm] LoginViewModel loginViewModel)
    {
        AntUser user;
        try
        {
            user = Authenticate(loginViewModel);
        }
        catch
        {
            return Unauthorized();
        }

        var claims = GetUserClaims(user);

        var identity = new ClaimsIdentity(claims, "Ant", ClaimTypes.Name, ClaimTypes.Role);
        var principal = new ClaimsPrincipal(identity);

        HttpContext.SignInAsync(principal);

        return LocalRedirect(loginViewModel.ReturnUrl ?? "/");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task MobileLogin()
    {
        var auth = await Request.HttpContext.AuthenticateAsync();

        // Ensure successful authentication
        if (!auth.Succeeded || auth?.Principal == null || !auth.Principal.Identities.Any(id => id.IsAuthenticated))
        {
            await Request.HttpContext.ChallengeAsync();
        }
        else
        {
            var claims = auth.Principal.Identities.FirstOrDefault()?.Claims;

            var token = CreateToken(claims);

            // callback parameters
            var parameters = new Dictionary<string, string>
            {
                { "access_token", token }
            };

            // Build the result url
            var valueParameters = parameters.Where(kvp => !string.IsNullOrEmpty(kvp.Value));
            var parameterStrings = valueParameters.Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}");
            var url = "ant://#" + string.Join("&", parameterStrings);

            // Redirect to final url
            Request.HttpContext.Response.Redirect(url);
        }
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<AntUser> Me()
    {
        var user = _settingsSnapshot.Value.Auth.Users.FirstOrDefault(x => x.Name == Request.HttpContext.User?.Identity?.Name);
        if (user == null)
        {
            return NotFound();
        }

        return new AntUser() { Name = user.Name, Roles = user.Roles };
    }

    private AntUser Authenticate(LoginViewModel loginViewModel)
    {
        var settings = _settingsSnapshot.Value;
        var user = settings.Auth.Users.FirstOrDefault(x => x.Name == loginViewModel.User && x.Password == loginViewModel.Password);

        if (user == null)
        {
            _logger.LogWarning("Failed login attempt for {User} - {Password}", loginViewModel.User, loginViewModel.Password);
            throw new InvalidCredentialException();
        }

        _logger.LogInformation("Login successful for {User}", loginViewModel.User);

        return user;
    }

    private string CreateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: credential,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static List<Claim> GetUserClaims(AntUser user)
    {
        List<Claim> claims = [new(ClaimTypes.Name, user.Name)];
        foreach (var role in user.Roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }
        return claims;
    }
}

public class LoginViewModel
{
    public string User { get; set; }
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
}