using Hospital.Models;
using System.ComponentModel.DataAnnotations;

namespace Hospital.ViewModels
{
    public class AdminViewModel
    {
        public string Id { get; set; }



        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }


        [Required(ErrorMessage = "First Name is Required")]
        public string FName { get; set; }




        [Required(ErrorMessage = "Last Name is Required")]
        public string LName { get; set; }


        [Required(ErrorMessage = "Age is Required")]
        [Range(0, 120, ErrorMessage = "Please enter a valid age")]
        public int Age { get; set; }



        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }



        [Required(ErrorMessage = "Phone number is Required")]
        [Phone]
        public string PhoneNumber { get; set; }


        public AdminViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
