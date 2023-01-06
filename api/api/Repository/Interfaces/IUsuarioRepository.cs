using api.DTO;
using api.Models;

namespace api.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<Usuario> SignIn(UsuarioCreateDto usuario);
        public Task<Usuario> Login(UsuarioLoginDto usuario);
    }
}
