using Microsoft.AspNetCore.Mvc;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using LabluzPro.Domain.Diversos;

namespace LabluzPro.Mvc.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IContratoRepository _contratoRepository;

        public ContratoController(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        public IActionResult Index() =>
            View(_contratoRepository.GetAll());

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("iNumero,sNome,dVencimento,sImagem,idTipo,IdTipoEquipamento,IdTipoServico")] Contrato _contrato, IFormFile IMAGEM)
        {
            if (ModelState.IsValid)
            {
                _contratoRepository.Add(_contrato);
                return RedirectToAction(nameof(Index));
            }

            return View(_contrato);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var _contrato = _contratoRepository.GetById(id);
            if (_contrato == null)
                return NotFound();

            return View(_contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,iNumero,sNome,dVencimento,sImagem,idTipo,IdTipoEquipamento,IdTipoServico")]  Contrato _contrato, IFormFile IMAGEM)
        {
            if (id != _contrato.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (IMAGEM != null)
                    {
                        _contrato.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        Diverso.SaveImage(IMAGEM, "DOCUMENTO", _contrato.sImagem);
                    }

                    _contratoRepository.Update(_contrato);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratoExists(_contrato.ID))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_contrato);
        }


        public IActionResult Delete(int? id)
        {
            //Delete
            if (id == null)
                return NotFound();

            var _documento = _contratoRepository.GetById(id);
            if (_documento == null)
                return NotFound();

            return View(_documento);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var _documento = _contratoRepository.GetById(id);
            _contratoRepository.Remove(_documento);

            return RedirectToAction(nameof(Index));
        }

        private bool ContratoExists(int id) =>
            _contratoRepository.GetById(id) != null;


    }
}