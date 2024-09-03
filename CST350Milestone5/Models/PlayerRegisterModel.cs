using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CST350Milestone5.Models
{
    public class PlayerRegisterModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The name must be at least 2 characters long")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The name must be at least 2 characters long")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The gender must be at least 1 character long")]
        public string Sex { get; set; }
        [Required]
        [Range(0, 150, ErrorMessage = "The age must be between 0 and 150")]
        public int Age { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The state must be at least 2 characters long")]
        public string State { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The username must be between 2 and 30 characters long")]
        public string Username { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The password must be at least 2 characters long")]
        public string Password { get; set; }

    }
}