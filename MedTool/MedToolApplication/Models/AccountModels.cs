using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace MedToolApplication.Models
{

  public class ChangePasswordModel
  {
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Praegune parool")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} peab olema vähemalt {2} tähemärki pikk.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Uus parool")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Kinnita uus parool")]
    [Compare("NewPassword", ErrorMessage = "Uus parool ja kinnitusparool ei ühti.")]
    public string ConfirmPassword { get; set; }
  }

  public class LogOnModel
  {
    [Required]
    [Display(Name = "Kasutajanimi")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Parool")]
    public string Password { get; set; }

    [Display(Name = "Jäta mind meelde?")]
    public bool RememberMe { get; set; }
  }

  public class RegisterModel
  {
    [Required]
    [Display(Name = "Kasutajanimi")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Emaili aadress")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} peab olema vähemalt {2} tähemärki pikk.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Kinnitusparool")]
    [Compare("Password", ErrorMessage = "Parool ja kinnitusparool ei ühti.")]
    public string ConfirmPassword { get; set; }
  }
}
