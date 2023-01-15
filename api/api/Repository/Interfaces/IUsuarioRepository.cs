using api.DTO;
using api.Models;

namespace api.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<UsuarioDto> SignIn(UsuarioCreateDto usuario);
        public Task<UsuarioDto> Login(UsuarioLoginDto usuario);
    }
}
