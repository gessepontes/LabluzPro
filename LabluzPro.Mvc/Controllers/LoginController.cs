using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace LabluzPro.Mvc.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFlashMessage _flashMessage;

        public LoginController(IUsuarioRepository usuarioRepository, IFlashMessage flashMessage)
        {
            _usuarioRepository = usuarioRepository;
            _flashMessage = flashMessage;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("sEmail,sSenha")] Usuario _usuario)
        {
            _usuario = _usuarioRepository.Login(_usuario);


            if (_usuario != null)
            {
                if (_usuario.bAtivo)
                {
                    HttpContext.Session.SetString("sNome", _usuario.sNome);
                    HttpContext.Session.SetString("ID", _usuario.ID.ToString());
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    _flashMessage.Danger("Usuário encontra-se inabilitado, contate o administrador do sistema!");
                    return RedirectToAction(nameof(Index));
                }
            }
            else {
                _flashMessage.Danger("Usuário ou senha incorretas!");
                return RedirectToAction(nameof(Index));
            }

        }
    }
}