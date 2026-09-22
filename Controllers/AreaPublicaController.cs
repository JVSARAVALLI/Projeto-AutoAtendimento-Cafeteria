using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Projeto_do_semestre.Controllers
{
    public class AreaPublicaController : Controller
    {
        // 1. ECRÃ DE REPOUSO DO TOTEM (Toque para Iniciar)
        public IActionResult IndexPublico()
        {
            return View();
        }

        // 2. ESCOLHA DE ACESSO (Com Login vs Sem Login)
        public IActionResult TipoAcesso()
        {
            return View();
        }

        // 3. LOGIN DO CLIENTE (Fidelidade)
        [HttpGet]
        public IActionResult LoginCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginCliente(string telefoneOuEmail)
        {
            // Simulação simples: se o cliente digitar algo, o sistema avança
            if (!string.IsNullOrEmpty(telefoneOuEmail))
            {
                return RedirectToAction("ModoConsumo");
            }
            ViewBag.Erro = "Dados inválidos. Tente novamente.";
            return View();
        }

        // 4. MODO DE CONSUMO (Comer Aqui ou Levar)
        public IActionResult ModoConsumo()
        {
            return View();
        }

        // 5. CATÁLOGO (Menu Principal)
        public IActionResult Catalogo()
        {
            var produtos = new List<dynamic>
            {
                new { Id = 1, Nome = "Expresso", Preco = 2.50, Imagem = "☕", Categoria = "Bebidas Quentes", Descricao = "Puro e intenso" },
                new { Id = 2, Nome = "Cappuccino", Preco = 4.80, Imagem = "☕", Categoria = "Bebidas Quentes", Descricao = "Com espuma cremosa" },
                new { Id = 3, Nome = "Pão de Queijo", Preco = 3.50, Imagem = "🥐", Categoria = "Salgados", Descricao = "Tradicional mineiro" },
                new { Id = 4, Nome = "Bolo Chocolate", Preco = 5.50, Imagem = "🍰", Categoria = "Sobremesas", Descricao = "Fatia generosa" }
            };
            return View(produtos);
        }

        // 6. CARRINHO DE COMPRAS
        public IActionResult Carrinho()
        {
            return View();
        }

        // 7. OPÇÕES (Acessibilidade e Atendente)
        public IActionResult Opcoes()
        {
            return View();
        }

        // 8. PERFIL (Opcional, para clientes que fizeram login)
        public IActionResult Perfil()
        {
            return View();
        }

        // 9. LOGIN RESTRITO (Apenas Funcionários)
        [HttpGet]
        public IActionResult LoginFuncionario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginFuncionario(string email, string senha)
        {
            if (email == "admin@mokka" && senha == "123")
            {
                return RedirectToAction("AdmIndex", "AreaRestrita");
            }
            else if (email == "cozinha@mokka" && senha == "789")
            {
                return RedirectToAction("CozIndex", "AreaCozinha");
            }

            ViewBag.Erro = "Credenciais inválidas!";
            return View();
        }

        // 10. ENCERRAR SESSÃO / CANCELAR NO TOTEM
        public IActionResult Logout()
        {
            // Volta para a tela de descanso do Totem
            return RedirectToAction("IndexPublico", "AreaPublica");
        }
    }
}