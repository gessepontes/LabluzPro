﻿using Microsoft.AspNetCore.Mvc;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using LabluzPro.Domain.Diversos;
using Vereyon.Web;
using Microsoft.Extensions.Configuration;

namespace LabluzPro.Mvc.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFlashMessage _flashMessage;
        private readonly IConfiguration _configuration;

        public UsuarioController(IUsuarioRepository usuarioRepository, IFlashMessage flashMessage, IConfiguration configuration)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _flashMessage = flashMessage;
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
                try
                {
                    if (sImagem != null)
                    {
                        _usuario.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        Diverso.SaveImage(sImagem, "USUARIO", _usuario.sImagem);
                    }

                    _usuario.iCodUsuarioMovimentacao = Convert.ToInt16(HttpContext.Session.GetString("ID"));
                    _usuarioRepository.Add(_usuario);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");
                }
                catch (Exception)
                {
                    _flashMessage.Danger("Erro ao realizar a operação!");
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_usuario);
        }

        public IActionResult EditUser()
        {
            string id = HttpContext.Session.GetString("ID");

            var _usuario = _usuarioRepository.GetByIdUsuarioPerfil(Convert.ToInt16(id));
            if (_usuario == null)
                return NotFound();

            if (_usuario.sImagem != null)
            {
                _usuario.sImagem = _configuration.GetSection("AppConfiguration")["ResourcesPath:Usuario"] + _usuario.sImagem;
            }
            else {
                _usuario.sImagem = _configuration.GetSection("AppConfiguration")["ResourcesPath:Usuario"] + "user.png";
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

                    _usuario.iCodUsuarioMovimentacao = Convert.ToInt16(HttpContext.Session.GetString("ID"));
                    _usuarioRepository.Update(_usuario);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    _flashMessage.Danger("Errro ao realizar a operação!");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(int id, [Bind("ID,sNome,sSenha,sEmail,sTelefone")]  Usuario _usuario, IFormFile sImagem)
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

                    _usuario.iCodUsuarioMovimentacao = Convert.ToInt16(HttpContext.Session.GetString("ID"));
                    _usuarioRepository.UpdateUser(_usuario);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    _flashMessage.Danger("Errro ao realizar a operação!");

                    if (!UsuarioExists(_usuario.ID))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(EditUser));
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

            try
            {
                var _usuario = _usuarioRepository.GetById(id);
                _usuarioRepository.Remove(_usuario);

                _flashMessage.Confirmation("Operação realizada com sucesso!");

            }
            catch (Exception)
            {
                _flashMessage.Danger("Erro ao realizar a operação!");
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id) =>
            _usuarioRepository.GetById(id) != null;


    }
}