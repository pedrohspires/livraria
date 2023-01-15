using api.DTO;
using api.Models;
using api.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioDto>> Login([FromBody]UsuarioLoginDto usuario)
        {
            try
            {
                UsuarioDto usuarioLogado = await _usuarioRepository.Login(usuario);
                return Ok(usuarioLogado);
            }
            catch(Exception ex)
            {
                return BadRequest("Erro ao executar login: " + ex.Message);
            }
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<Usuario>> SignIn([FromBody]UsuarioCreateDto usuario)
        {
            try
            {
                UsuarioDto novoUsuario = await _usuarioRepository.SignIn(usuario);

                return Ok(novoUsuario);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
