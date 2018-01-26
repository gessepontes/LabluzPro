using Microsoft.AspNetCore.Mvc;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using LabluzPro.Domain.Diversos;

namespace LabluzPro.Mvc.Controllers
{
    public class DocumentoController : Controller
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly ITipoRepository _tipoRepository;

        public DocumentoController(IDocumentoRepository documentoRepository, ITipoRepository tipoRepository)
        {
            _documentoRepository = documentoRepository;
            _tipoRepository = tipoRepository;
        }

        public IActionResult Index() =>
            View(_documentoRepository.GetAll());

        public IActionResult Create()
        {
            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(1);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("sNumero,sNome,dVencimento,sImagem,IdTipo")] Documento _documento, IFormFile sImagem)
        {
            if (ModelState.IsValid)
            {
                if (sImagem != null)
                {
                    _documento.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    Diverso.SaveImage(sImagem, "DOCUMENTO", _documento.sImagem);
                }

                _documentoRepository.Add(_documento);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(1);
            return View(_documento);
        }


        public IActionResult Edit(int? id)
        {
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
                        _documento.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        Diverso.SaveImage(sImagem, "DOCUMENTO", _documento.sImagem);
                    }

                    _documentoRepository.Update(_documento);

                }
                catch (DbUpdateConcurrencyException)
                {
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
            var _documento = _documentoRepository.GetById(id);
            _documentoRepository.Remove(_documento);

            return RedirectToAction(nameof(Index));
        }

        private bool DocumentoExists(int id) =>
            _documentoRepository.GetById(id) != null;


    }
}