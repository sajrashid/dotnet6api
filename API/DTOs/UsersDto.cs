using System;

namespace API.DTOs
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string UserAgent { get; set; }
        public string IP { get; set; }
        public DateTime LastVisit { get; set; }
        public int Count { get; set; }

    }
}