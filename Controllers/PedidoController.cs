using Microsoft.AspNetCore.Mvc;
using Projeto_do_semestre.Dao;
using Projeto_do_semestre.Models;
using System.Text.Json;

namespace Projeto_do_semestre.Controllers;

public class PedidoController(PedidoDao pedidoDao, ProdutoDao produtoDao) : Controller
{

        // POST: /Pedido/AdicionarAoCarrinho
    [HttpPost]
    public IActionResult AdicionarAoCarrinho(int idProduto, int quantidade, string? observacao)
    {
        var produto = produtoDao.BuscarPorId(idProduto);
        if (produto == null) return NotFound();

        // 1. Ler da Session manualmente sem a classe Helper
        string? jsonCarrinho = HttpContext.Session.GetString("CarrinhoTotem");
        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho) ?? new List<ItemCarrinho>();

        // 2. Adicionar ou atualizar o item
        var itemExistente = carrinho.FirstOrDefault(i => i.Produto.IdProduto == idProduto);
        if (itemExistente != null)
        {
            itemExistente.Quantidade += quantidade;
            if (!string.IsNullOrEmpty(observacao))
            {
                itemExistente.Observacao = observacao;
            }
        }
        else
        {
            carrinho.Add(new ItemCarrinho
            {
                Produto = produto,
                Quantidade = quantidade,
                Observacao = observacao
            });
        }

        // 3. Salvar de volta na Session em formato JSON
        string jsonAtualizado = JsonSerializer.Serialize(carrinho);
        HttpContext.Session.SetString("CarrinhoTotem", jsonAtualizado);

        return RedirectToAction(nameof(Carrinho));
    }

    // GET: /Pedido/Carrinho (Abre a View AreaPublica/Carrinho.cshtml)
    public IActionResult Carrinho()
    {
        string? jsonCarrinho = HttpContext.Session.GetString("CarrinhoTotem");
        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho) ?? new List<ItemCarrinho>();

        return View("~/Views/AreaPublica/Carrinho.cshtml", carrinho);
    }

    // POST: /Pedido/FinalizarPedido
    [HttpPost]
    public IActionResult FinalizarPedido(int? idCliente)
    {
        string? jsonCarrinho = HttpContext.Session.GetString("CarrinhoTotem");
        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho);

        if (carrinho == null || carrinho.Count == 0)
        {
            return RedirectToAction(nameof(Carrinho));
        }

        var novoPedido = new Pedido
        {
            Status = "pendente",
            ValorTotal = carrinho.Sum(i => i.Subtotal),
            IdCliente = idCliente,
            Itens = carrinho.Select(i => new ItemPedido
            {
                IdProduto = i.Produto.IdProduto,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.Produto.PrecoAtual,
                Observacao = i.Observacao
            }).ToList()
        };

        // Grava no SQLite e retorna o ID e o número da senha gerada
        int idPedido = pedidoDao.CriarPedido(novoPedido);

        // Limpa a Session
        HttpContext.Session.Remove("CarrinhoTotem");

        ViewBag.SenhaGerada = novoPedido.NumeroSenha;
        return View("~/Views/AreaPublica/IndexPublico.cshtml");
    }

    // ==========================================
    // 2. ÁREA DA COZINHA
    // ==========================================

    // GET: /Pedido/Cozinha (Abre a View AreaCozinha/CozIndex.cshtml)
    public IActionResult Cozinha()
    {
        var pedidosPendentes = pedidoDao.ListarPedidosCozinha();
        return View("~/Views/AreaCozinha/CozIndex.cshtml", pedidosPendentes);
    }

    // POST: /Pedido/AtualizarStatus
    [HttpPost]
    public IActionResult AtualizarStatus(int idPedido, string novoStatus)
    {
        pedidoDao.AtualizarStatus(idPedido, novoStatus);
        return RedirectToAction(nameof(Cozinha));
    }
}
