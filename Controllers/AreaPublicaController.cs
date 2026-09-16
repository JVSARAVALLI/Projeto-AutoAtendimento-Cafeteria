using Microsoft.AspNetCore.Mvc;

namespace Projeto_do_semestre.Controllers
{
    public class AreaPublicaController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult IndexPublico()
        {
            return View();
        }
        public IActionResult Identificacao()
        {
        // SIMULAÇÃO: Aqui nós checamos se o usuário está logado no sistema.
        // Como estamos apenas montando as telas, vamos criar uma variável falsa para testar.
        // Mude para "true" para ver a tela, e "false" para testar o redirecionamento!
        bool usuarioEstaLogado = true; 

        if (!usuarioEstaLogado)
        {
        // Se NÃO estiver logado, redireciona para a tela de Login
        return RedirectToAction("Login");
        }

        // Se estiver logado, mostra a tela de Identificação normalmente
        return View();
        }
        public IActionResult Catalogo()
        {
            return View();
        }
        public IActionResult Carrinho()
        {
            return View();
        }
    }
}
