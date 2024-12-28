using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CanIEatIt.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string? EmailFirstName;
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string? EmailLastName;
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]+$")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email Sender")]
        public string? EmailSender;
        [Required(ErrorMessage = "Subject is required")]
        [Display(Name = "Subject")]
        public string? EmailSubject;
        [Required(ErrorMessage = "Message is required")]
        [Display(Name = "Message")]
        public string? EmailBody;

        public string? success;
    }
}

