using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataDriven.Models
{
    //[Table("Produto")]
    public class Product 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [DataType("nvarchar")]
        public string Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage="O preço deve ser maior que zero")]
        public decimal Price { get; set; }

        //propriedade que o entityframework le automaticamente da categoria (Id)
        [Required(ErrorMessage="Categoria inválida")]
        [Range(1, int.MaxValue, ErrorMessage="O id da categoria deve ser maior que zero")]
        public int CategoryId { get; set; }

        //propriedade que o entityframework le automaticamente da categoria (objeto)
        public Category Category { get; set; }
    }   
}