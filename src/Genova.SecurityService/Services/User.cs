﻿namespace Genova.SecurityService.Services;

public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public List<string> Roles { get; set; } = [];
}
