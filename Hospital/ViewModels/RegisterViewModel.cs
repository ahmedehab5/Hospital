using Hospital.Models;
using System.ComponentModel.DataAnnotations;

namespace Hospital.ViewModels
{
	public class RegisterViewModel
	{
		//all validations here are client-side validations

		[Required(ErrorMessage = "First Name is Required")]
		public string FName { get; set; }




		[Required(ErrorMessage = "Last Name is Required")]
		public string LName { get; set; }


		[Required(ErrorMessage = "Age is Required")]
		[Range(0, 120, ErrorMessage = "Please enter a valid age")]
		public int Age { get; set; }



		[Required]
		public Gender Gender { get; set; }



		[Required(ErrorMessage = "Phone number is Required")]
		[Phone]
		public string PhoneNumber { get; set; }



		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }




		[Required(ErrorMessage = "Password is Required")]
		//[DataType(DataType.Password)] //must contain upper,lower,digit....so on
		public string Password { get; set; }





		[Required(ErrorMessage = "Confirm Password is Required")]
		//[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password doesn't match")]
		public string ConfirmPassword { get; set; }


		public bool Agree { get; set; }
		


	}
}
