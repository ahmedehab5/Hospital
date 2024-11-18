using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public abstract class Person:IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public DateOnly DOB { get; set; }
        public Gender Gender { get; set; }
        public byte[]? Image { get; set; }

        public bool Agree { get; set; }
    }
}
