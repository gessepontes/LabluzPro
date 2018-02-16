using Microsoft.AspNetCore.Mvc;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using LabluzPro.Domain.Diversos;
using Vereyon.Web;
using System.Threading.Tasks;
using System.IO;
using LabluzPro.Mvc.Models;
using System.Linq;

namespace LabluzPro.Mvc.Controllers
{
    public class DocumentoController : Controller
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly IFlashMessage _flashMessage;

        public DocumentoController(IDocumentoRepository documentoRepository, ITipoRepository tipoRepository, IFlashMessage flashMessage)
        {
            _documentoRepository = documentoRepository;
            _tipoRepository = tipoRepository;
            _flashMessage = flashMessage;
        }

        public IActionResult Index() {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 2))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            return View(_documentoRepository.GetAll());

        }

        public IActionResult Create()
        {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 2))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(1);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("sNumero,sNome,dVencimento,sImagem,IdTipo")] Documento _documento, IFormFile sImagem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (sImagem != null)
                    {
                        string[] aFoto = sImagem.FileName.Split('.');

                        _documento.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1];
                        Diverso.SaveImage(sImagem, "DOCUMENTO", _documento.sImagem);
                    }

                    _documento.iCodUsuarioMovimentacao = HttpContext.Session.GetComplexData<Usuario>("UserData").ID;
                    _documentoRepository.Add(_documento);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");

                }
                catch (Exception)
                {
                    _flashMessage.Danger("Erro ao realizar a operação!");
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(1);
            return View(_documento);
        }


        public IActionResult Edit(int? id)
        {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 2))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            if (id == null)
                return NotFound();

            var _documento = _documentoRepository.GetById(id);
            if (_documento == null)
                return NotFound();

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(1);
            return View(_documento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,sNumero,sNome,dVencimento,sImagem,IdTipo")]  Documento _documento, IFormFile sImagem)
        {
            if (id != _documento.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (sImagem != null)
                    {
                        string[] aFoto = sImagem.FileName.Split('.');

                        _documento.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1];
                        Diverso.SaveImage(sImagem, "DOCUMENTO", _documento.sImagem);
                    }

                    _documento.iCodUsuarioMovimentacao = HttpContext.Session.GetComplexData<Usuario>("UserData").ID;
                    _documentoRepository.Update(_documento);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    _flashMessage.Danger("Erro ao realizar a operação!");

                    if (!DocumentoExists(_documento.ID))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(1);
            return View(_documento);
        }


        public IActionResult Delete(int? id)
        {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 2))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            //Delete
            if (id == null)
                return NotFound();

            var _documento = _documentoRepository.GetById(id);
            if (_documento == null)
                return NotFound();

            return View(_documento);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var _documento = _documentoRepository.GetById(id);
                _documentoRepository.Remove(_documento);
                _flashMessage.Confirmation("Operação realizada com sucesso!");

            }
            catch (Exception)
            {
                _flashMessage.Danger("Erro ao realizar a operação!");
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DocumentoExists(int id) =>
            _documentoRepository.GetById(id) != null;

        public async Task<IActionResult> Download(string sImagem)
        {
            if (sImagem == null)
            {
                _flashMessage.Warning("Arquivo não encontrado!");
                return RedirectToAction(nameof(Index));
            }

            string path = Diverso.Download(sImagem, "DOCUMENTO");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;


            return File(memory, Diverso.GetContentType(path), Path.GetFileName(path));
        }
    }
}