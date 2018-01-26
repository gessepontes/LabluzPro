using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabluzPro.Mvc.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("sEmail,sSenha")] Usuario _usuario)
        {
            if (_usuarioRepository.Login(_usuario))
            {
                return RedirectToAction(nameof(Index), "Documento");
            }
            else {
                return RedirectToAction(nameof(Index));
            }

        }
    }
}