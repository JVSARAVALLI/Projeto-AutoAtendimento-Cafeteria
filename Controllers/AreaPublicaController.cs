using Microsoft.AspNetCore.Mvc;

namespace Projeto_do_semestre.Controllers
{
    public class AreaPublicaController : Controller
    {
        // 1. Método GET: Apenas exibe a tela de Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Método POST: Recebe os dados digitados no formulário
        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            // Regra A: Administrador
            if (email == "a@a" && senha == "123")
            {
                // Redireciona para o Dashboard na Área Restrita
                return RedirectToAction("AdmIndex", "AreaRestrita");
            }
            // Regra B: Cliente Público
            else if (email == "b@b" && senha == "456")
            {

                // Redireciona para o Perfil (ou IndexPublico) na Área Pública
                return RedirectToAction("Perfil", "AreaPublica");
            }
            // Regra B: Cliente Público
            else if (email == "c@c" && senha == "789")
            {

                // Redireciona para o Perfil (ou IndexPublico) na Área Pública
                return RedirectToAction("CozIndex", "AreaCozinha");
            }
            // Regra C: Errou a senha
            else
            {
                // Manda uma mensagem de erro de volta para a tela
                ViewBag.Erro = "E-mail ou senha incorretos!";
                return View();
            }
        }
        public IActionResult IndexPublico()
        {
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

        public IActionResult Opcoes()
        {
            return View();
        }

        public IActionResult Identificacao()
        {
            // Mudei para true para testarmos o novo redirecionamento!
            bool usuarioEstaLogado = false;

            if (usuarioEstaLogado)
            {
                // Se ESTIVER logado, redireciona para a página de Perfil
                return RedirectToAction("Perfil");
            }
            else
            {
                // Se NÃO estiver logado, redireciona para a página de Login
                return RedirectToAction("Login");
            }
        }

        // Nova ação para a página de Perfil que você vai criar
        public IActionResult Perfil()
        {
            return View();
        }
    }
}