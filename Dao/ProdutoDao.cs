using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Projeto_do_semestre.Data;
using Projeto_do_semestre.Models;

namespace Projeto_do_semestre.Dao;
public class ProdutoDao
{

    public List<Produto> ListarTodos()
    {
        var lista = new List<Produto>();
        using var conexao = Banco.ObterConexao();
        conexao.Open();

        var sql = "SELECT id_produto, nome, descricao, preco_atual, disponivel, categoria FROM PRODUTO;";
        using var cmd = new SqliteCommand(sql, conexao);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Produto
            {
                IdProduto = Convert.ToInt32(reader["id_produto"]),
                Nome = reader["nome"].ToString() ?? "",
                Descricao = reader["descricao"].ToString() ?? "",
                PrecoAtual = Convert.ToDecimal(reader["preco_atual"]),
                Disponivel = Convert.ToInt32(reader["disponivel"]) == 1,
                Categoria = reader["categoria"].ToString() ?? ""
            });
        }
        return lista;
    }

    public List<Produto> BuscarPorCategoria(string categoria)
    {
        var lista = new List<Produto>();
        using var conexao = Banco.ObterConexao();
        conexao.Open();

        var sql = "SELECT id_produto, nome, descricao, preco_atual, disponivel, categoria FROM PRODUTO WHERE categoria = @categoria;";
        using var cmd = new SqliteCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@categoria", categoria);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Produto
            {
                IdProduto = Convert.ToInt32(reader["id_produto"]),
                Nome = reader["nome"].ToString() ?? "",
                Descricao = reader["descricao"].ToString() ?? "",
                PrecoAtual = Convert.ToDecimal(reader["preco_atual"]),
                Disponivel = Convert.ToInt32(reader["disponivel"]) == 1,
                Categoria = reader["categoria"].ToString() ?? ""
            });
        }
        return lista;
    }

    public Produto? BuscarPorId(int id)
    {
        using var conexao = Banco.ObterConexao();
        conexao.Open();

        var sql = "SELECT id_produto, nome, descricao, preco_atual, disponivel, categoria FROM PRODUTO WHERE id_produto = @id;";
        using var cmd = new SqliteCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@id", id);
        using var leitor = cmd.ExecuteReader();

        if (leitor.Read())
        {
            return new Produto
            {
                IdProduto = leitor.GetInt32(0),
                Nome = leitor.GetString(1),
                Descricao = leitor.IsDBNull(2) ? null : leitor.GetString(2),
                PrecoAtual = leitor.GetDecimal(3),
                Disponivel = leitor.GetInt32(4) == 1,
                Categoria = leitor.GetString(5)
            };
        }
        return null;
    }

    public void Inserir(Produto produto)
    {
        using var conexao = Banco.ObterConexao();
        conexao.Open();

        var sql = @"INSERT INTO PRODUTO (nome, descricao, preco_atual, disponivel, categoria) 
                    VALUES (@nome, @descricao, @precoAtual, @disponivel, @categoria);";
        using var cmd = new SqliteCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@nome", produto.Nome);
        cmd.Parameters.AddWithValue("@descricao", (object?)produto.Descricao ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@precoAtual", produto.PrecoAtual);
        cmd.Parameters.AddWithValue("@disponivel", produto.Disponivel ? 1 : 0);
        cmd.Parameters.AddWithValue("@categoria", produto.Categoria);
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conexao = Banco.ObterConexao();
        conexao.Open();

        var sql = "DELETE FROM PRODUTO WHERE id_produto = @id;";
        using var cmd = new SqliteCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
   