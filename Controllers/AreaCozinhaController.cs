using Microsoft.AspNetCore.Mvc;

namespace Projeto_do_semestre.Controllers
{
    public class AreaCozinhaController : Controller
    {
            public IActionResult CozIndex()
        {
            return View();
        }
            public IActionResult CozCatalogo()
        {
            return View();
        }
            public IActionResult CozInsumos()
        {
            return View();
        }
            public IActionResult CozFuncionarios()
        {
            return View();
        }
        public IActionResult CozCompras()
        {
            return View();
        }
        public IActionResult CozDashboard()
        {
            return View();
        }
    }
}
