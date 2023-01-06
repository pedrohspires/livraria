using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string? Sobrenome { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string Senha { get; set; }

        [Required]
        [MaxLength(8)]
        public byte[] Salt { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }
        
        public DateTime? DataEdicao { get; set; }

        [Required]
        public bool EmailConfirmado { get; set; }
        
        public DateTime? DataEmailConfirmado { get; set; }
    }
}
