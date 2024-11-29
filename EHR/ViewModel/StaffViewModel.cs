using System.ComponentModel.DataAnnotations;

namespace EHR.ViewModel
{
    public class StaffViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
