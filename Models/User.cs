using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataDriven.Models
{
    //[Table("Usuario")]
    public class User 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [DataType("nvarchar")]
        public string Username { get; set; }

        [Required(ErrorMessage="Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [DataType("nvarchar")]
        public string Password { get; set; }

        public string Role { get; set; }
    }   
}