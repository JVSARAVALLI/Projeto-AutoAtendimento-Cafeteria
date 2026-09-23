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

        // 3. Central Única de Cadastros (Menu dos 3 botões)
        public IActionResult AdmCadastros()
        {
            return View();
        }

        // ==========================================
        // NOVAS ROTAS (Para os 3 botões funcionarem)
        // ==========================================

        // 4. Ecrã de Produtos e Cardápio
        public IActionResult AdmCardapio()
        {
            return View();
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