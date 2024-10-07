using System.ComponentModel.DataAnnotations;

namespace Hospital.ViewModels
{
	public class LoginViewModel
	{

		[Required(ErrorMessage ="Email is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }



		[Required(ErrorMessage ="Password is Required")]
		public string Password { get; set; }


		public bool RememberMe { get; set; } = false;


	}
}
