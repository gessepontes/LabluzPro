using Microsoft.AspNetCore.Mvc;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using LabluzPro.Domain.Diversos;

namespace LabluzPro.Mvc.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index() =>
            View(_usuarioRepository.GetAll());

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("sNome,sSenha,sEmail,bAtivo,sTelefone,PaginaSelecionado")] Usuario _usuario, IFormFile sImagem)
        {
            if (ModelState.IsValid)
            {
                if (sImagem != null)
                {
                    _usuario.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    Diverso.SaveImage(sImagem, "USUARIO", _usuario.sImagem);
                }

                _usuarioRepository.Add(_usuario);
                return RedirectToAction(nameof(Index));
            }

            return View(_usuario);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var _usuario = _usuarioRepository.GetByIdUsuarioPerfil(id);
            if (_usuario == null)
                return NotFound();

            return View(_usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,sNome,sSenha,sEmail,bAtivo,sTelefone,PaginaSelecionado")]  Usuario _usuario, IFormFile sImagem)
        {
            if (id != _usuario.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (sImagem != null)
                    {
                        _usuario.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        Diverso.SaveImage(sImagem, "USUARIO", _usuario.sImagem);
                    }

                    _usuarioRepository.Update(_usuario);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(_usuario.ID))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(_usuario);
        }


        public IActionResult Delete(int? id)
        {
            //Delete
            if (id == null)
                return NotFound();

            var _usuario = _usuarioRepository.GetById(id);
            if (_usuario == null)
                return NotFound();

            return View(_usuario);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var _usuario = _usuarioRepository.GetById(id);
            _usuarioRepository.Remove(_usuario);

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id) =>
            _usuarioRepository.GetById(id) != null;


    }
}