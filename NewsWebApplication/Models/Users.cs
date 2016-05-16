//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewsWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Users
    {
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Заполните поле:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Заполните поле:")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Заполните поле:")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Заполните поле:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int IdRole { get; set; }
    
        public virtual Roles Roles { get; set; }
    }
}
