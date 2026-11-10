using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
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

    // Área Administrativa -> AreaRestrita/AdmCatalago.cshtml
    public IActionResult Index(string? categoria)
    {
        List<Produto> produtos = string.IsNullOrEmpty(categoria)
            ? _produtoDao.ListarTodos()
            : _produtoDao.BuscarPorCategoria(categoria);

        return View("~/Views/AreaRestrita/AdmCatalago.cshtml", produtos);
    }

    // Tela de Cadastro -> AreaRestrita/AdmCadastros.cshtml
    public IActionResult Criar()
    {
        return View("~/Views/AreaRestrita/AdmCadastros.cshtml", new Produto());
    }

    [HttpPost]
    public IActionResult Criar(Produto produto)
    {
        if (ModelState.IsValid)
        {
            _produtoDao.Inserir(produto);
            return RedirectToAction(nameof(Index));
        }
        return View("~/Views/AreaRestrita/AdmCadastros.cshtml", produto);
    }

    [HttpPost]
    public IActionResult Excluir(int id)
    {
        _produtoDao.Excluir(id);
        return RedirectToAction(nameof(Index));
    }

    // Totem de Autoatendimento -> AreaPublica/Catalogo.cshtml
    public IActionResult Cardapio(string? categoria)
    {
        List<Produto> produtos = string.IsNullOrEmpty(categoria)
            ? _produtoDao.ListarTodos()
            : _produtoDao.BuscarPorCategoria(categoria);

        // Removido o '~/' e o '.cshtml' para resolver a compatibilidade de tipo do Razor
        return View("~/Views/AreaPublica/Catalogo.cshtml", produtos);
    }
}




