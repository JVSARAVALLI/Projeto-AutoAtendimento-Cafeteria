using Microsoft.Data.Sqlite;
using Projeto_do_semestre.Data;
using Projeto_do_semestre.Models;

namespace Projeto_do_semestre.Dao;

public class ProdutoDao
{
    // Lista todos os produtos
    public List<Produto> ListarTodos()
    {
        var lista = new List<Produto>();

        using var conexao = Banco.ObterConexao();

        var sql = @"SELECT id_produto, nome, descricao,
                    preco_atual, disponivel, categoria, imagem
                    FROM PRODUTO;";

        using var cmd = new SqliteCommand(sql, conexao);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Produto
            {
                IdProduto = Convert.ToInt32(reader["id_produto"]),
                Nome = reader["nome"].ToString() ?? "",
                Descricao = reader["descricao"] == DBNull.Value
                    ? null
                    : reader["descricao"].ToString(),
                PrecoAtual = Convert.ToDecimal(reader["preco_atual"]),
                Disponivel = Convert.ToInt32(reader["disponivel"]) == 1,
                Categoria = reader["categoria"].ToString() ?? "",
                Imagem = reader["imagem"] == DBNull.Value
                    ? null
                    : reader["imagem"].ToString()
            });
        }

        return lista;
    }

    // Busca produtos por categoria
    public List<Produto> BuscarPorCategoria(string categoria)
    {
        var lista = new List<Produto>();

        using var conexao = Banco.ObterConexao();

        var sql = @"SELECT id_produto, nome, descricao,
                    preco_atual, disponivel, categoria, imagem
                    FROM PRODUTO
                    WHERE categoria = @categoria;";

        using var cmd = new SqliteCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@categoria", categoria);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Produto
            {
                IdProduto = Convert.ToInt32(reader["id_produto"]),
                Nome = reader["nome"].ToString() ?? "",
                Descricao = reader["descricao"] == DBNull.Value
                    ? null
                    : reader["descricao"].ToString(),
                PrecoAtual = Convert.ToDecimal(reader["preco_atual"]),
                Disponivel = Convert.ToInt32(reader["disponivel"]) == 1,
                Categoria = reader["categoria"].ToString() ?? "",
                Imagem = reader["imagem"] == DBNull.Value
                    ? null
                    : reader["imagem"].ToString()
            });
        }

        return lista;
    }

    // Busca produto pelo ID
    public Produto? BuscarPorId(int id)
    {
        using var conexao = Banco.ObterConexao();

        var sql = @"SELECT id_produto, nome, descricao,
                    preco_atual, disponivel, categoria, imagem
                    FROM PRODUTO
                    WHERE id_produto = @id;";

        using var cmd = new SqliteCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@id", id);

        using var leitor = cmd.ExecuteReader();

        if (leitor.Read())
        {
            return new Produto
            {
                IdProduto = leitor.GetInt32(0),
                Nome = leitor.GetString(1),
                Descricao = leitor.IsDBNull(2)
                    ? null
                    : leitor.GetString(2),
                PrecoAtual = leitor.GetDecimal(3),
                Disponivel = leitor.GetInt32(4) == 1,
                Categoria = leitor.GetString(5),
                Imagem = leitor.IsDBNull(6)
                    ? null
                    : leitor.GetString(6)
            };
        }

        return null;
    }

    // Insere produto no banco
    public void Inserir(Produto produto)
    {
        using var conexao = Banco.ObterConexao();

        var sql = @"INSERT INTO PRODUTO
                    (nome, descricao, preco_atual, disponivel,
                     categoria, imagem)
                    VALUES
                    (@nome, @descricao, @precoAtual, @disponivel,
                     @categoria, @imagem);";

        using var cmd = new SqliteCommand(sql, conexao);

        cmd.Parameters.AddWithValue("@nome", produto.Nome);

        cmd.Parameters.AddWithValue(
            "@descricao",
            (object?)produto.Descricao ?? string.Empty
        );

        cmd.Parameters.AddWithValue(
            "@precoAtual",
            produto.PrecoAtual
        );

        cmd.Parameters.AddWithValue(
            "@disponivel",
            produto.Disponivel ? 1 : 0
        );

        cmd.Parameters.AddWithValue(
            "@categoria",
            produto.Categoria
        );

        cmd.Parameters.AddWithValue(
            "@imagem",
            (object?)produto.Imagem ?? DBNull.Value
        );

        cmd.ExecuteNonQuery();
    }

    // Exclui produto
    public void Excluir(int id)
    {
        using var conexao = Banco.ObterConexao();

        var sql = "DELETE FROM PRODUTO WHERE id_produto = @id;";

        using var cmd = new SqliteCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }
    // Atualiza um produto existente
    public void Atualizar(Produto produto)
    {
        using var conexao = Banco.ObterConexao();

        var sql = @"UPDATE PRODUTO
                SET nome = @nome,
                    descricao = @descricao,
                    preco_atual = @precoAtual,
                    disponivel = @disponivel,
                    categoria = @categoria,
                    imagem = @imagem
                WHERE id_produto = @id;";

        using var cmd = new SqliteCommand(sql, conexao);

        cmd.Parameters.AddWithValue("@nome", produto.Nome);
        cmd.Parameters.AddWithValue(
            "@descricao",
            (object?)produto.Descricao ?? string.Empty
        );
        cmd.Parameters.AddWithValue("@precoAtual", produto.PrecoAtual);
        cmd.Parameters.AddWithValue("@disponivel", produto.Disponivel ? 1 : 0);
        cmd.Parameters.AddWithValue("@categoria", produto.Categoria);
        cmd.Parameters.AddWithValue(
            "@imagem",
            (object?)produto.Imagem ?? DBNull.Value
        );
        cmd.Parameters.AddWithValue("@id", produto.IdProduto);

        cmd.ExecuteNonQuery();
    }
}
