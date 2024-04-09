﻿namespace Bookify.Domain.Users;

public sealed class Role
{
    public static Role Registered = new Role(1, "Registered");
    
    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public ICollection<User> Users { get; init; } = new List<User>();
    public ICollection<Permission> Permissions { get; init; } = new List<Permission>();
}