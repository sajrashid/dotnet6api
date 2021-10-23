// <copyright file="Roles.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Roles
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(16)]
        public string? Role { get; set; }
    }
}
