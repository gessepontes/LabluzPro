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
    public class ContratoController : Controller
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly IFlashMessage _flashMessage;

        public ContratoController(IContratoRepository contratoRepository, ITipoRepository tipoRepository , IFlashMessage flashMessage)
        {
            _contratoRepository = contratoRepository;
            _tipoRepository = tipoRepository;
            _flashMessage = flashMessage;

        }

        public IActionResult Index() {
            if (!HttpContext.Session.GetComplexData<Usuario>("UserData").PaginaSelecionado.Contains(Paginas.Contrato))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            return View(_contratoRepository.GetAll());
        }
            

        public IActionResult Create()
        {
            if (!HttpContext.Session.GetComplexData<Usuario>("UserData").PaginaSelecionado.Contains(Paginas.Contrato))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(2);
            ViewBag.ListaTipoServico = _tipoRepository.GetAllTipoDrop(4);
            ViewBag.ListaTipoEquipamento = _tipoRepository.GetAllTipoDrop(5);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("sNumero,sNome,dVencimento,sImagem,IdTipo,IdTipoEquipamento,IdTipoServico")] Contrato _contrato, IFormFile sImagem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (sImagem != null)
                    {
                        string[] aFoto = sImagem.FileName.Split('.');

                        _contrato.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1];
                        Diverso.SaveImage(sImagem, "CONTRATO", _contrato.sImagem);
                    }

                    _contrato.iCodUsuarioMovimentacao = HttpContext.Session.GetComplexData<Usuario>("UserData").ID;
                    _contratoRepository.Add(_contrato);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");

                }
                catch (Exception)
                {
                    _flashMessage.Danger("Erro ao realizar a operação!");
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(2);
            ViewBag.ListaTipoServico = _tipoRepository.GetAllTipoDrop(4);
            ViewBag.ListaTipoEquipamento = _tipoRepository.GetAllTipoDrop(5);
            return View(_contrato);
        }


        public IActionResult Edit(int? id)
        {
            if (!HttpContext.Session.GetComplexData<Usuario>("UserData").PaginaSelecionado.Contains(Paginas.Contrato))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

            if (id == null)
                return NotFound();

            var _contrato = _contratoRepository.GetById(id);
            if (_contrato == null)
                return NotFound();

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(2);
            ViewBag.ListaTipoServico = _tipoRepository.GetAllTipoDrop(4);
            ViewBag.ListaTipoEquipamento = _tipoRepository.GetAllTipoDrop(5);
            return View(_contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,sNumero,sNome,dVencimento,sImagem,IdTipo,IdTipoEquipamento,IdTipoServico")]  Contrato _contrato, IFormFile sImagem)
        {
            if (id != _contrato.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (sImagem != null)
                    {
                        string[] aFoto = sImagem.FileName.Split('.');

                        _contrato.sImagem = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1];
                        Diverso.SaveImage(sImagem, "CONTRATO", _contrato.sImagem);
                    }

                    _contrato.iCodUsuarioMovimentacao = HttpContext.Session.GetComplexData<Usuario>("UserData").ID;
                    _contratoRepository.Update(_contrato);
                    _flashMessage.Confirmation("Operação realizada com sucesso!");

                }
                catch (DbUpdateConcurrencyException)
                {
                    _flashMessage.Danger("Erro ao realizar a operação!");

                    if (!ContratoExists(_contrato.ID))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListaTipo = _tipoRepository.GetAllTipoDrop(2);
            ViewBag.ListaTipoServico = _tipoRepository.GetAllTipoDrop(4);
            ViewBag.ListaTipoEquipamento = _tipoRepository.GetAllTipoDrop(5);
            return View(_contrato);
        }


        public IActionResult Delete(int? id)
        {
            if (!HttpContext.Session.GetComplexData<Usuario>("UserData").PaginaSelecionado.Contains(Paginas.Contrato))
            {
                return RedirectToAction("DeniedAccess", "Login");
            }

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
            try
            {
                var _documento = _contratoRepository.GetById(id);
                _contratoRepository.Remove(_documento);
                _flashMessage.Confirmation("Operação realizada com sucesso!");

            }
            catch (Exception)
            {
                _flashMessage.Danger("Erro ao realizar a operação!");
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContratoExists(int id) =>
            _contratoRepository.GetById(id) != null;

        public async Task<IActionResult> Download(string sImagem)
        {
            if (sImagem == null)
            {
                _flashMessage.Warning("Arquivo não encontrado!");
                return RedirectToAction(nameof(Index));
            }

            string path = Diverso.Download(sImagem, "CONTRATO");

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