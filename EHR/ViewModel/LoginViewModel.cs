using System.ComponentModel.DataAnnotations;

namespace EHR.ViewModel
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
         public string Password { get; set; }
    }
}
