using api.Database;
using api.DTO;
using api.Models;
using api.Repository.Interfaces;
using api.Utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

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

        public async Task<UsuarioDto> SignIn(UsuarioCreateDto usuario)
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

            return UsuarioParaUsuarioDto(novoUsuario);
        }

        public async Task<UsuarioDto> Login(UsuarioLoginDto usuario)
        {
            UsuarioDto usuarioLogado = new UsuarioDto();
            Usuario? usuarioCadastrado = await _dbContext.Usuarios.Where(x => x.Email == usuario.Email).FirstOrDefaultAsync();

            if (usuarioCadastrado == null)
                throw new Exception("Email ou senha incorreta!");

            string hashSenha = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: usuario.Senha!,
                salt: usuarioCadastrado.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
            );

            if (usuarioCadastrado.Senha.Equals(hashSenha))
                return UsuarioParaUsuarioDto(usuarioCadastrado);
            else throw new Exception("Email ou senha incorreta!");
        }

        private UsuarioDto UsuarioParaUsuarioDto(Usuario usuario)
        {
            UsuarioDto usuarioDto = new UsuarioDto();

            usuarioDto.Id = usuario.Id;
            usuarioDto.Nome = usuario.Nome;
            usuarioDto.Sobrenome = usuario.Sobrenome;
            usuarioDto.Email = usuario.Email;
            usuarioDto.DataCadastro = usuario.DataCadastro;
            usuarioDto.DataEdicao = usuario.DataEdicao;
            usuarioDto.EmailConfirmado = usuario.EmailConfirmado;
            usuarioDto.DataEmailConfirmado = usuario.DataEmailConfirmado;

            return usuarioDto;
        }
    }
}
