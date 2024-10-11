using System.ComponentModel.DataAnnotations;

namespace Hospital.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is Required")]
        //[DataType(DataType.Password)] //must contain upper,lower,digit....so on
        public string NewPassword { get; set; }



        [Required(ErrorMessage = "Confirm Password is Required")]
        //[DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password doesn't match")]
        public string ConfirmNewPassword { get; set; }


    }
}
