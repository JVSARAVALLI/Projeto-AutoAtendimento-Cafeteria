using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Projeto_do_semestre.Controllers
{
    public class AreaPublicaController : Controller
    {
        // Método de entrada padrão da Controller que garante o redirecionamento para o Totem
        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectToAction("IndexPublico");
        }

        // Tela de Descanso (Attract Screen) do Totem Mokka Café
        [AllowAnonymous]
        public IActionResult IndexPublico()
        {
            return View();
        }

        // FLUXO DO CLIENTE AO TOCAR EM "INICIAR": Redireciona para a escolha de acesso do Totem
        [AllowAnonymous]
        public IActionResult Identificacao()
        {
            return RedirectToAction("TipoAcesso");
        }

        // Carrinho de compras
        [AllowAnonymous]
        public IActionResult Carrinho()
        {
            return View();
        }

        // 1. Acesso Interno / Login (GET: Exibe a tela de Login da equipe)
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Acesso Interno / Login (POST: Processa as credenciais de Admin e Cozinha)
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            // Regra A: Administrador
            if (email == "a@a" && senha == "123")
            {
                return RedirectToAction("AdmIndex", "AreaRestrita");
            }
            // Regra C: Cozinha
            else if (email == "c@c" && senha == "789")
            {
                return RedirectToAction("CozIndex", "AreaCozinha");
            }
            // Regra D: Credenciais inválidas
            else
            {
                ViewBag.Erro = "E-mail ou senha incorretos!";
                return View();
            }
        }

        // 2. ESCOLHA DE ACESSO DO CLIENTE (Com Fidelidade vs Sem Cadastro)
        [AllowAnonymous]
        public IActionResult TipoAcesso()
        {
            return View();
        }

        // 3. LOGIN DO CLIENTE (Programa de Fidelidade)
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginCliente()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginCliente(string telefoneOuEmail)
        {
            if (!string.IsNullOrEmpty(telefoneOuEmail))
            {
                return RedirectToAction("ModoConsumo");
            }
            ViewBag.Erro = "Dados inválidos. Tente novamente.";
            return View();
        }

        // 4. MODO DE CONSUMO (Comer Aqui ou Levar)
        [AllowAnonymous]
        public IActionResult ModoConsumo()
        {
            return View();
        }

        // 5. CATÁLOGO (Menu Principal do Cliente)
        [AllowAnonymous]
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

        // 7. OPÇÕES (Acessibilidade e Atendente)
        [AllowAnonymous]
        public IActionResult Opcoes()
        {
            return View();
        }

        // 8. PERFIL (Para clientes que fizeram login no programa de fidelidade)
        [AllowAnonymous]
        public IActionResult Perfil()
        {
            return View();
        }

        // 9. ENCERRAR SESSÃO / CANCELAR NO TOTEM
        [AllowAnonymous]
        public IActionResult Logout()
        {
            return RedirectToAction("IndexPublico", "AreaPublica");
        }
    }
}