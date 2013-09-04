using System.ComponentModel.DataAnnotations;

namespace MvcTFA.Models
{
    public class SecondFactorModel
    {
        [Required]
        [Display(Name = "Two Fator Authentication Password")]
        public string SecondFactor { get; set; }
    }
}