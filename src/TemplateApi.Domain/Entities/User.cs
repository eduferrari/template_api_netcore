using TemplateApi.Domain.Common;

namespace TemplateApi.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Pass { get; private set; } = string.Empty;
    public bool Active { get; private set; } = true;

    protected User() { }

    public static User Create(string name, string email, string pass)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(pass);

        return new User
        {
            Name = name,
            Email = email,
            Pass = pass,
            Active = true
        };
    }

    public void Update(string name, string pass)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(pass);

        Name = name;
        Pass = pass;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        Active = false;
        SetUpdatedAt();
    }
}
