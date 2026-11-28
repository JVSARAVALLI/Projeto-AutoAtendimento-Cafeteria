
using Microsoft.AspNetCore.Mvc;
using Projeto_do_semestre.Dao;
using Projeto_do_semestre.Models;
using System.Text.Json;
using System.Globalization;

namespace Projeto_do_semestre.Controllers;

public class PedidoController(PedidoDao pedidoDao, ProdutoDao produtoDao) : Controller
{
    // POST: /Pedido/AdicionarAoCarrinho
    [HttpPost]
    public IActionResult AdicionarAoCarrinho(
        int idProduto,
        int quantidade,
        string? observacao,
        string precoUnitario)
    {
        if (quantidade <= 0)
            return BadRequest("Quantidade inválida.");

        // Converte o preço enviado pelo JavaScript
        if (!decimal.TryParse(
            precoUnitario,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out decimal preco))
        {
            return BadRequest("Preço inválido.");
        }

        if (preco < 0)
            return BadRequest("Preço inválido.");

        var produto = produtoDao.BuscarPorId(idProduto);

        if (produto == null)
            return NotFound();

        string? jsonCarrinho =
            HttpContext.Session.GetString("CarrinhoTotem");

        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho)
              ?? new List<ItemCarrinho>();

        // Verifica se já existe o mesmo produto com a mesma personalização
        var itemExistente = carrinho.FirstOrDefault(i =>
            i.Produto.IdProduto == idProduto &&
            i.Observacao == observacao &&
            i.PrecoUnitario == preco);

        if (itemExistente != null)
        {
            itemExistente.Quantidade += quantidade;
        }
        else
        {
            carrinho.Add(new ItemCarrinho
            {
                Produto = produto,
                Quantidade = quantidade,
                Observacao = observacao,
                PrecoUnitario = preco
            });
        }

        // Salva os produtos na Session
        HttpContext.Session.SetString(
            "CarrinhoTotem",
            JsonSerializer.Serialize(carrinho)
        );

        return Ok();
    }

    // GET: /Pedido/Carrinho
    public IActionResult Carrinho()
    {
        string? jsonCarrinho =
            HttpContext.Session.GetString("CarrinhoTotem");

        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho)
              ?? new List<ItemCarrinho>();

        return View("~/Views/AreaPublica/Carrinho.cshtml", carrinho);
    }

    // POST: /Pedido/AlterarQuantidade
    [HttpPost]
    public IActionResult AlterarQuantidade(int index, int delta)
    {
        string? jsonCarrinho =
            HttpContext.Session.GetString("CarrinhoTotem");

        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho)
              ?? new List<ItemCarrinho>();

        if (index < 0 || index >= carrinho.Count)
            return BadRequest();

        carrinho[index].Quantidade += delta;

        if (carrinho[index].Quantidade <= 0)
        {
            carrinho.RemoveAt(index);
        }

        HttpContext.Session.SetString(
            "CarrinhoTotem",
            JsonSerializer.Serialize(carrinho)
        );

        return Ok();
    }

    // POST: /Pedido/LimparCarrinho
    [HttpPost]
    public IActionResult LimparCarrinho()
    {
        HttpContext.Session.Remove("CarrinhoTotem");

        return Ok();
    }

    // POST: /Pedido/FinalizarPedido
    [HttpPost]
    public IActionResult FinalizarPedido(int? idCliente)
    {
        string? jsonCarrinho =
            HttpContext.Session.GetString("CarrinhoTotem");

        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho)
              ?? new List<ItemCarrinho>();

        if (carrinho.Count == 0)
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

                // Registra o preço incluindo os adicionais
                PrecoUnitario = i.PrecoUnitario,

                Observacao = i.Observacao

            }).ToList()
        };

        // Grava o pedido no SQLite
        int idPedido = pedidoDao.CriarPedido(novoPedido);

        HttpContext.Session.Remove("CarrinhoTotem");

        return Json(new
        {
            sucesso = true,
            senha = novoPedido.NumeroSenha
        });
    }
    [HttpGet]
    public IActionResult ResumoCarrinho()
    {
        string? jsonCarrinho =
            HttpContext.Session.GetString("CarrinhoTotem");

        var carrinho = string.IsNullOrEmpty(jsonCarrinho)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(jsonCarrinho)
              ?? new List<ItemCarrinho>();

        return Json(new
        {
            quantidadeItens = carrinho.Sum(i => i.Quantidade),
            valorTotal = carrinho.Sum(i => i.Subtotal)
        });
    }
    // ==========================================
    // ÁREA DA COZINHA
    // ==========================================

    // GET: /Pedido/Cozinha
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
