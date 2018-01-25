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

        public DocumentoController(IDocumentoRepository documentoRepository)
        {
            _documentoRepository = documentoRepository;
        }

        public IActionResult Index() =>
            View(_documentoRepository.GetAll());

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("iNumero,sNome,dVencimento,sImagem,idTipo")] Documento _documento, IFormFile IMAGEM)
        {
            if (ModelState.IsValid)
            {
                _documentoRepository.Add(_documento);
                return RedirectToAction(nameof(Index));
            }

            return View(_documento);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var _documento = _documentoRepository.GetById(id);
            if (_documento == null)
                return NotFound();

            return View(_documento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,iNumero,sNome,dVencimento,sImagem,idTipo")]  Documento _documento, IFormFile IMAGEM)
        {
            if (id != _documento.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (IMAGEM != null)
                    {
                        _documento.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        Diverso.SaveImage(IMAGEM, "DOCUMENTO", _documento.sImagem);
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