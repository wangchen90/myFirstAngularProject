using System.ComponentModel.DataAnnotations;

namespace BAL.Models
{
    public class UserModel
    {
        public int user_id { get; set; }

        [Required]
         
        public string name { get; set; }
        public string email { get; set; }

        [Required(ErrorMessage ="Cannot be Empty")] [Phone(ErrorMessage ="Not a Phone number")]
        [RegularExpression(@"\d{10}$", ErrorMessage = "Please provide a valid 10 Digit mobile number.")]
        public string mobile { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }

    public class UserLoginModel
    {
        [StringLength(10)]
        public string mobile { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }
}
