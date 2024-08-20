﻿namespace DemoBackendArchitecture.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string? RoleName { get; set; }
    public ICollection<User>? Users { get; set; }
}