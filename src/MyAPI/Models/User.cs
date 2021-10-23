// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public byte[]? Salt { get; set; }
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly

        [Required]
        public string? Hash { get; set; }

        [Required]
        public DateTime? LastVisit { get; set; }
    }
}