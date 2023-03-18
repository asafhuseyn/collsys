using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CollSys.Matm.Kitabxana.Presentation.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the UserModel class
    public class UserModel : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Ad")]
        [Required(ErrorMessage = " *Ad yazılmalıdır! ")]
        [MaxLength(50, ErrorMessage = " *Ad 50 xarakterdən az olmalıdır! ")]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Soyad")]
        [Required(ErrorMessage = " *Soyad yazılmalıdır! ")]
        [MaxLength(50, ErrorMessage = " *Soyad 50 xarakterdən az olmalıdır! ")]
        public string LastName { get; set; }


        // Self Join
        [PersonalData]
        [Display(Name = "Referans")]
        [MaxLength(450)]
        public string ReferenceId { get; set; }
        public UserModel Reference { get; set; }
    }
}
