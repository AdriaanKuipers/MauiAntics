namespace AntCore.Models;

public class AntUser
{
    public string Name { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; } = [];
}