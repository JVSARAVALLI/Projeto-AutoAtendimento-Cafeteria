using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Projeto_do_semestre.Dao;
using Projeto_do_semestre.Models;

namespace Projeto_do_semestre.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoDao _produtoDao;


        public ProdutoController(ProdutoDao produtoDao)
        {
            _produtoDao = produtoDao;
        }

        //lista e filtra(READ)
        [HttpGet]
        public IActionResult Index([FromQuery] string? categoria)
        {
            List<Produto> produtos;

            if (!string.IsNullOrEmpty(categoria))
            {
                produtos = _produtoDao.BuscarPorCategoria(categoria);
            }
            else
            {
                produtos = _produtoDao.ListarTodos();
            }

            return Ok(produtos); // Retorna JSON 200 OK
        }

        //busca por id

        [HttpGet("{id; int;}")]
        public IActionResult BuscarPorId(int id)
        {
            var produto = _produtoDao.BuscarPorId(id);
            if (produto == null)
            {
                return NotFound(new { mensagem = "Produto não encontrado!" });
            }
            return Ok(produto);
        }

        //salvar no banco 
        [HttpPost]
        public IActionResult Criar([FromBody] Produto produto)
        {
            if (produto == null || string.IsNullOrWhiteSpace(produto.Nome))
            {
                return BadRequest(new { mensagem = "Dados inválidos." });
            }

            _produtoDao.Inserir(produto);
            return Ok(new { mensagem = "Produto cadastrado com sucesso!" });
        }

        //atualizar
        [HttpPut("{id; int;}")]
        public IActionResult Editar(int id, [FromBody] Produto produto)
        {
            var produtoExistente = _produtoDao.BuscarPorId(id);
            if (produtoExistente == null)
            {
                return NotFound(new { mensagem = "Produto não encontrado para atualização." });
            }

            produto.IdProduto = id;
            _produtoDao.Atualizar(produto);

            return Ok(new { mensagem = "Produto atualizado com sucesso!" });
        }

        //excluir(DELETE)
        [HttpDelete("{id; int;}")]
        public IActionResult Excluir(int id)
        {
            var produtoExistente = _produtoDao.BuscarPorId(id);
            if (produtoExistente == null)
            {
                return NotFound(new { mensagem = "Produto não encontrado para realizar exclusão." });
            }

            _produtoDao.Excluir(id);
            return Ok(new { mensagem = "Produto excluido com sucesso!"});
        }
        
    }
}
