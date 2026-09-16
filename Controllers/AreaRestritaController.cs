using Microsoft.AspNetCore.Mvc;

namespace Projeto_do_semestre.Controllers
{
    public class AreaRestritaController : Controller
    {
        public IActionResult AdmIndex()
        {
            return View();
        }
        public IActionResult AdmCadastros()
        {
            return View();
        }

        public IActionResult AdmCatalogo()
        {
            return View();
        }

        public IActionResult AdmVendas()
        {
            return View();
        }
    }
}
