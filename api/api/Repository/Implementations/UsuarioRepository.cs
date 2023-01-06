using api.Database;
using api.DTO;
using api.Models;
using api.Repository.Interfaces;
using api.Utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace api.Repository.Implementations
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ValidaUsuario _validaUsuario;

        public UsuarioRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _validaUsuario = new ValidaUsuario();
        }

        public Task<Usuario> Login(UsuarioLoginDto usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> SignIn(UsuarioCreateDto usuario)
        {
            Usuario novoUsuario = new Usuario();

            // Caso as validações falhem, as funções geram uma exeção para o Controller
            await _validaUsuario.Valida(usuario, _dbContext);

            novoUsuario.Nome = usuario.Nome;
            novoUsuario.Sobrenome = usuario.Sobrenome;
            novoUsuario.Email = usuario.Email;
            novoUsuario.Salt = new Salt().getSalt(8);

            novoUsuario.Senha = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: usuario.Senha!,
                salt: novoUsuario.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
            );

            novoUsuario.DataCadastro = DateTime.Now;
            novoUsuario.EmailConfirmado = false;

            await _dbContext.Usuarios.AddAsync(novoUsuario);
            _dbContext.SaveChanges();

            // Limpa dados sensíveis para retornar
            novoUsuario.Senha = null;
            novoUsuario.Salt = null;

            return novoUsuario;
        }
    }
}
