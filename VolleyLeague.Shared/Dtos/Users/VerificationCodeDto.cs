using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyLeague.Shared.Dtos.Users
{
    public class VerificationCodeDto
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Kod weryfikacyjny jest wymagany.")]
        public string VerificationCode { get; set; }
    }

}
