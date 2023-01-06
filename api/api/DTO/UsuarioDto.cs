using System.ComponentModel.DataAnnotations;

namespace api.DTO
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataEdicao { get; set; }
        public bool EmailConfirmado { get; set; }
        public DateTime? DataEmailConfirmado { get; set; }
    }
}
