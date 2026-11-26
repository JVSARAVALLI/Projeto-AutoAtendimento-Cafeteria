using Microsoft.AspNetCore.Mvc;
using Projeto_do_semestre.Dao;
using Projeto_do_semestre.Models;

namespace Projeto_do_semestre.Controllers;

public class ProdutoController : Controller
{
    private readonly ProdutoDao _produtoDao;

    public ProdutoController(ProdutoDao produtoDao)
    {
        _produtoDao = produtoDao;
    }

    // Área Administrativa - Catálogo
    public IActionResult Index(string? categoria)
    {
        List<Produto> produtos;

        if (string.IsNullOrEmpty(categoria))
        {
            produtos = _produtoDao.ListarTodos();
        }
        else
        {
            produtos = _produtoDao.BuscarPorCategoria(categoria);
        }

        Console.WriteLine("Produtos encontrados: " + produtos.Count);

        return View("~/Views/AreaRestrita/AdmCatalago.cshtml", produtos);
    }

    // Tela de Cadastro
    public IActionResult Criar()
    {
        return View(
            "~/Views/AreaRestrita/AdmCadastros.cshtml",
            new Produto()
        );
    }

    // Recebe os dados do formulário
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Criar(Produto produto)
    {
        // Validação do link da imagem
        if (!string.IsNullOrWhiteSpace(produto.Imagem))
        {
            if (!Uri.TryCreate(
                produto.Imagem,
                UriKind.Absolute,
                out Uri? endereco) ||
                (endereco.Scheme != Uri.UriSchemeHttp &&
                 endereco.Scheme != Uri.UriSchemeHttps))
            {
                ModelState.AddModelError(
                    nameof(produto.Imagem),
                    "Informe um link de imagem válido (HTTP ou HTTPS)."
                );
            }
        }

        if (ModelState.IsValid)
        {
            _produtoDao.Inserir(produto);

            return RedirectToAction(nameof(Index));
        }

        return View(
            "~/Views/AreaRestrita/AdmCadastros.cshtml",
            produto
        );
    }

    // Excluir produto
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Excluir(int id)
    {
        _produtoDao.Excluir(id);

        return RedirectToAction(nameof(Index));
    }

    // Cardápio público
    public IActionResult Cardapio(string? categoria)
    {
        List<Produto> produtos = string.IsNullOrEmpty(categoria)
            ? _produtoDao.ListarTodos()
            : _produtoDao.BuscarPorCategoria(categoria);

        return View("~/Views/AreaPublica/Catalogo.cshtml", produtos);
    }
    // Abre o formulário para editar um produto
    public IActionResult Editar(int id)
    {
        Produto? produto = _produtoDao.BuscarPorId(id);

        if (produto == null)
        {
            return NotFound();
        }

        return View(
            "~/Views/AreaRestrita/AdmCadastros.cshtml",
            produto
        );
    }

    // Recebe os dados editados
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Produto produto)
    {
        // Valida o link da imagem
        if (!string.IsNullOrWhiteSpace(produto.Imagem))
        {
            if (!Uri.TryCreate(
                produto.Imagem,
                UriKind.Absolute,
                out Uri? endereco) ||
                (endereco.Scheme != Uri.UriSchemeHttp &&
                 endereco.Scheme != Uri.UriSchemeHttps))
            {
                ModelState.AddModelError(
                    nameof(produto.Imagem),
                    "Informe um link de imagem válido (HTTP ou HTTPS)."
                );
            }
        }

        if (ModelState.IsValid)
        {
            _produtoDao.Atualizar(produto);
            return RedirectToAction(nameof(Index));
        }

        return View(
            "~/Views/AreaRestrita/AdmCadastros.cshtml",
            produto
        );
    }
}
