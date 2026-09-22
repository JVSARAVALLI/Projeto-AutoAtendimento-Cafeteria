using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Projeto_do_semestre.Controllers
{
    public class AreaPublicaController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            if (email == "a@a" && senha == "123")
            {
                return RedirectToAction("AdmIndex", "AreaRestrita");
            }
            else if (email == "b@b" && senha == "456")
            {
                // Entra no sistema de autoatendimento
                return RedirectToAction("Perfil", "AreaPublica");
            }
            else if (email == "c@c" && senha == "789")
            {
                return RedirectToAction("CozIndex", "AreaCozinha");
            }
            else
            {
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
            // Mock Data enriquecido com Categoria e Descrição para o Totem
            var produtos = new List<dynamic>
            {
                new { Id = 1, Nome = "Expresso", Preco = 2.50, Imagem = "☕", Categoria = "Bebidas Quentes", Descricao = "Puro e intenso" },
                new { Id = 2, Nome = "Cappuccino", Preco = 4.80, Imagem = "☕", Categoria = "Bebidas Quentes", Descricao = "Com espuma cremosa" },
                new { Id = 3, Nome = "Pão de Queijo", Preco = 3.50, Imagem = "🥐", Categoria = "Salgados", Descricao = "Tradicional mineiro" },
                new { Id = 4, Nome = "Bolo Chocolate", Preco = 5.50, Imagem = "🍰", Categoria = "Sobremesas", Descricao = "Fatia generosa" },
                new { Id = 5, Nome = "Suco Laranja", Preco = 4.00, Imagem = "🍹", Categoria = "Bebidas Frias", Descricao = "Natural 300ml" },
                new { Id = 6, Nome = "Croissant", Preco = 6.00, Imagem = "🥐", Categoria = "Salgados", Descricao = "Massa folhada" }
            };

            return View(produtos);
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
            return View();
        }

        public IActionResult Perfil()
        {
            return View();
        }

        // Método para Encerrar Sessão no Totem
        public IActionResult Logout()
        {
            // Redireciona a máquina de volta para a tela de Login
            return RedirectToAction("Login", "AreaPublica");
        }
    }
}