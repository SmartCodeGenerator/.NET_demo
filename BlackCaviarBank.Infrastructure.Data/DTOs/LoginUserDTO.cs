﻿namespace BlackCaviarBank.Infrastructure.Data
{
    public class LoginUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? RememberMe { get; set; }
    }
}