﻿using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Client.Blazor.Shared.Dtos
{
    public class UserDto
    {
        public string? Id { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
