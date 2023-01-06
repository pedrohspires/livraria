using api.Database;
using api.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace api.Utils
{
    public class ValidaUsuario
    {
        public async Task<bool> Valida(UsuarioCreateDto usuario, AppDbContext dbContext)
        {
            /*
                Devem apresentar erro
                - Emails duplicados ou vazios antes do @
                - Email que não contem @ nem ".com"
                - Senha com menos de 8 caracteres
                - Senha sem letras maiúsculas e minúsculas
                - Senha sem caracteres especiais e números
                - Nome não informado
                - Nome e sobrenome com caracteres especiais e números
            */

            await ValidaEmail(usuario.Email, dbContext);
            ValidaSenha(usuario.Senha);
            ValidaNome(usuario.Nome, usuario.Sobrenome);

            return true;
        }

        private async Task<bool> ValidaEmail(string email, AppDbContext dbContext)
        {
            var usuarioExistente = await dbContext.Usuarios.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (usuarioExistente != null)
                throw new Exception($"O email {email} já está cadastrado!");

            if (!Regex.IsMatch(email, "[a-zA-Z][a-zA-Z0-9_]{1,}@[a-zA-Z]+.com$"))
                throw new Exception("Endereço de email inválido!");

            return true;
        }

        private void ValidaSenha(string senha)
        {
            if (!Regex.IsMatch(senha, "^(?=.*[A-Z])(?=.*[!#@$%&])(?=.*[0-9])(?=.*[a-z]).{8,}$"))
                throw new Exception("A senha deve conter 8 ou mais digitos, letras maiúsculas e minúsculas, números e caracteres especiais!");
        }

        private void ValidaNome(string nome, string sobrenome)
        {
            if (nome.Length == 0)
                throw new Exception("Preencha o campo \"Nome\"!");

            if (!Regex.IsMatch(nome + sobrenome, "^[a-zA-Z ]+$"))
                throw new Exception("Nome e sobrenome devem conter apenas letras e espaços!");
        }
    }
}
