using Microsoft.AspNetCore.Mvc;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using LabluzPro.Domain.Diversos;

namespace LabluzPro.Mvc.Controllers
{
    public class CertificadoController : Controller
    {
        private readonly ICertificadoRepository _certificadoRepository;
        private readonly ITipoRepository _tipoRepository;

        public CertificadoController(ICertificadoRepository certificadoRepository, ITipoRepository tipoRepository)
        {
            _certificadoRepository = certificadoRepository;
            _tipoRepository = tipoRepository;
        }

        public IActionResult Index() =>
            View(_certificadoRepository.GetAll());

        public IActionResult Create()
        {
            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(3);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("sNumero,sNome,dVencimento,dServico,IdTipo")] Certificado _certificado, IFormFile sImagem)
        {
            if (ModelState.IsValid)
            {
                if (sImagem != null)
                {
                    _certificado.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    Diverso.SaveImage(sImagem, "CERTIFICADO", _certificado.sImagem);
                }

                _certificadoRepository.Add(_certificado);
                return RedirectToAction(nameof(Index));
            }

            return View(_certificado);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var _certificado = _certificadoRepository.GetById(id);
            if (_certificado == null)
                return NotFound();

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(3);

            return View(_certificado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,sNumero,sNome,dVencimento,dServico,IdTipo")]  Certificado _certificado, IFormFile sImagem)
        {
            if (id != _certificado.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (sImagem != null)
                    {
                        _certificado.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        Diverso.SaveImage(sImagem, "CERTIFICADO", _certificado.sImagem);
                    }

                    _certificadoRepository.Update(_certificado);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificadoExists(_certificado.ID))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_certificado);
        }


        public IActionResult Delete(int? id)
        {
            //Delete
            if (id == null)
                return NotFound();

            var _certificado = _certificadoRepository.GetById(id);
            if (_certificado == null)
                return NotFound();

            return View(_certificado);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var _certificado = _certificadoRepository.GetById(id);
            _certificadoRepository.Remove(_certificado);

            return RedirectToAction(nameof(Index));
        }

        private bool CertificadoExists(int id) =>
            _certificadoRepository.GetById(id) != null;


    }
}