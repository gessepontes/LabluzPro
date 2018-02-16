using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabluzPro.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabluzPro.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly ICertificadoRepository _certificadoRepository;

        public HomeController(ICertificadoRepository certificadoRepository, IContratoRepository contratoRepository, IDocumentoRepository documentoRepository)
        {
            _contratoRepository = contratoRepository;
            _certificadoRepository = certificadoRepository;
            _documentoRepository = documentoRepository;

        }

        public IActionResult Index()
        {
            ViewBag.DocumentoCount = _documentoRepository.Count();
            ViewBag.ContratoCount = _contratoRepository.Count();
            ViewBag.CertificadoCount = _certificadoRepository.Count();
            return View();
        }
    }
}