using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArticlApp.CoreView
{
    public class AuthorView
    {
        [Required]
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "معرف المستخدم")]
        public string UserId { get; set; } = string.Empty;
        [Required]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;
        [ValidateNever]
        [Display(Name = "الصورة")]
        public IFormFile ProfileImageUrl { get; set; }
        [Display(Name = "السيرة الذاتية")]
        public string? Bio { get; set; }
        [Display(Name = "فيسبوك")]
        public string? Facbook { get; set; }
        [Display(Name = "انستكرام")]
        public string? Instagram { get; set; }
        [Display(Name = "تويتر")]
        public string? Twitter { get; set; }
    }
}
