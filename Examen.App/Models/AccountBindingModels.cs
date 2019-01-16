using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Examen.App.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Clave actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe de tener mas de {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva clave")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva clave")]
        [Compare("NewPassword", ErrorMessage = "La clave y la clave de confirmacion no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required(ErrorMessage = "Proporcione un correo electrónico")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La clave no puede estar vacia")]
        [StringLength(100, ErrorMessage = "La {0} debe de tener mas de {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar clave")]
        [Compare("Password", ErrorMessage = "La nueva clave y la clave de confirmacion no coinciden.")]
        public string ConfirmPassword { get; set; }

        //Estas annotationes no me estan funcionando. Aunque verifico la validacion en el controller
        [Required(ErrorMessage = "Seleccione por lo menos un role.")]
        [MinLength(1, ErrorMessage = "Seleccione por lo menos un role.")]
        public string[] Roles;
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
