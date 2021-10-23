// <copyright file="Visitor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Models
{
    public class Visitor
    {
        public int Id { get; set; }

        public string Hash { get; set; } = null!;

        public string UserAgent { get; set; } = null!;

        public string IP { get; set; } = null!;

        public DateTime LastVisit { get; set; }

        public int Count { get; set; }
    }
}