using Microsoft.AspNetCore.Mvc;

namespace Projeto_do_semestre.Controllers
{
    public class AreaCozinhaController : Controller
    {
        public IActionResult CozIndex()
        {
            return View();
        }
    }
}
