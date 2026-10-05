using Microsoft.AspNetCore.Mvc;

namespace Projeto_do_semestre.Controllers
{
    public class AreaRestritaController : Controller
    {
        // 1. Dashboard Principal
        public IActionResult AdmIndex()
        {
            return View();
        }

        // 2. Central de Vendas
        public IActionResult AdmVendas()
        {
            return View();
        }

        // 3. Central Única de Cadastros
        public IActionResult AdmCadastros()
        {
            return View();
        }

        // 4. Ecrã de Produtos e Cardápio (Aponta explicitamente para o arquivo AdmCatalago.cshtml)
        public IActionResult AdmCatalogo()
        {
            return View("AdmCatalago");
        }

        // 5. Ecrã de Funcionários (Equipa)
        public IActionResult AdmFuncionarios()
        {
            return View();
        }

        // 6. Ecrã de Usuários (Acessos)
        public IActionResult AdmUsuarios()
        {
            return View();
        }
    }
}