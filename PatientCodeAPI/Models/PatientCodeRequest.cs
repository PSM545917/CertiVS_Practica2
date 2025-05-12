using System.ComponentModel.DataAnnotations;

namespace PatientCodeAPI.Models
{
    public class PatientCodeRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "CI is required")]
        public string CI { get; set; } = string.Empty;
    }
}