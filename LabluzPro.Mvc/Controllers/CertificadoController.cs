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
    public class CertificadoController : Controller
    {
        private readonly ICertificadoRepository _certificadoRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly IFlashMessage _flashMessage;

        public CertificadoController(ICertificadoRepository certificadoRepository, ITipoRepository tipoRepository, IFlashMessage flashMessage)
        {
            _certificadoRepository = certificadoRepository;
            _tipoRepository = tipoRepository;
            _flashMessage = flashMessage;
        }

        public IActionResult Index() {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 3))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            return View(_certificadoRepository.GetAll());
        }
            

        public IActionResult Create()
        {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 3))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(3);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("sNumero,sNome,dVencimento,dServico,IdTipo")] Certificado _certificado, IFormFile sImagem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (sImagem != null)
                    {
                        string[] aFoto = sImagem.FileName.Split('.');

                        _certificado.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1];
                        Diverso.SaveImage(sImagem, "CERTIFICADO", _certificado.sImagem);
                    }

                    _certificado.iCodUsuarioMovimentacao = HttpContext.Session.GetComplexData<Usuario>("UserData").ID;
                    _certificadoRepository.Add(_certificado);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");
                }
                catch (Exception)
                {
                    _flashMessage.Danger("Erro ao realizar a operação!");
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(3);
            return View(_certificado);
        }


        public IActionResult Edit(int? id)
        {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 3))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

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
                        string[] aFoto = sImagem.FileName.Split('.');

                        _certificado.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1];
                        Diverso.SaveImage(sImagem, "CERTIFICADO", _certificado.sImagem);
                    }

                    _certificado.iCodUsuarioMovimentacao = HttpContext.Session.GetComplexData<Usuario>("UserData").ID;
                    _certificadoRepository.Update(_certificado);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    _flashMessage.Danger("Erro ao realizar a operação!");

                    if (!CertificadoExists(_certificado.ID))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(3);
            return View(_certificado);
        }


        public IActionResult Delete(int? id)
        {
            if (!Diverso.Acesso(HttpContext.Session.GetComplexData<Usuario>("UserData"), 3))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

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
            try
            {
                var _certificado = _certificadoRepository.GetById(id);
                _certificadoRepository.Remove(_certificado);
                _flashMessage.Confirmation("Operação realizada com sucesso!");
            }
            catch (Exception)
            {
                _flashMessage.Danger("Erro ao realizar a operação!");
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CertificadoExists(int id) =>
            _certificadoRepository.GetById(id) != null;


        public async Task<IActionResult> Download(string sImagem)
        {
            if (sImagem == null)
            {
                _flashMessage.Warning("Arquivo não encontrado!");
                return RedirectToAction(nameof(Index));
            }

            string path = Diverso.Download(sImagem, "CERTIFICADO");

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