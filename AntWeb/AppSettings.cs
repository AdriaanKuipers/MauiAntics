using AntCore.Models;

namespace AntWeb;

public class AppSettings
{
    public AuthSettings Auth { get; set; }
}

public class AuthSettings
{
    public List<AntUser> Users { get; set; } = [];
    public JwtSettings Jwt { get; set; }
}

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}
