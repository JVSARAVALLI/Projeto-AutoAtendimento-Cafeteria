using Projeto_do_semestre.Models;
using Projeto_do_semestre.Data;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Projeto_do_semestre.Dao
{
    public class ProdutoDao
    {
        
        // 1. INSERIR (CREATE)
        
        public void Inserir(Produto produto)
        {
            using var conexao = Banco.ObterConexao(); 
            conexao.Open();

            string sql = @"INSERT INTO PRODUTO (nome, descricao, preco_atual, disponivel, categoria) 
                          VALUES (@nome, @descricao, @precoAtual, @disponivel, @categoria)";

            using var comando = new SqliteCommand(sql, conexao);
            comando.Parameters.AddWithValue("@nome", produto.Nome);
            // Trata campo opcional (string?): envia DBNull se for nulo
            comando.Parameters.AddWithValue("@descricao", (object?)produto.Descricao ?? DBNull.Value);
            comando.Parameters.AddWithValue("@precoAtual", produto.PrecoAtual);
            comando.Parameters.AddWithValue("@disponivel", produto.Disponivel ? 1 : 0);
            comando.Parameters.AddWithValue("@categoria", produto.Categoria);

            comando.ExecuteNonQuery();
        }

        
        //  CONSULTA POR ID (READ)
      
        public Produto? BuscarPorId(int id)
        {
            using var conexao = Banco.ObterConexao();
            conexao.Open();

            string sql = @"SELECT id_produto, nome, descricao, preco_atual, disponivel, categoria 
                          FROM PRODUTO WHERE id_produto = @id";

            using var comando = new SqliteCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            using var leitor = comando.ExecuteReader();

            if (leitor.Read())
            {
                return MapearProduto(leitor);
            }

            return null;
        }

        // ==========================================
        // 3. LISTAR GERAL (READ ALL)
        // ==========================================
        public List<Produto> ListarTodos()
        {
            var listaProdutos = new List<Produto>();

            using var conexao = Banco.ObterConexao();
            conexao.Open();

            string sql = @"SELECT id_produto, nome, descricao, preco_atual, disponivel, categoria 
                          FROM PRODUTO";

            using var comando = new SqliteCommand(sql, conexao);
            using var leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                listaProdutos.Add(MapearProduto(leitor));
            }

            return listaProdutos;
        }

        // ==========================================
        // 4. LISTAR POR REQUISITOS (FILTRAR POR CATEGORIA NO TOTEM)
        // ==========================================
        public List<Produto> BuscarPorCategoria(string categoria, bool apenasDisponiveis = true)
        {
            var listaProdutos = new List<Produto>();

            using var conexao = Banco.ObterConexao();
            conexao.Open();

            string sql = @"SELECT id_produto, nome, descricao, preco_atual, disponivel, categoria 
                          FROM PRODUTO 
                          WHERE categoria LIKE @categoria AND disponivel = @disponivel";

            using var comando = new SqliteCommand(sql, conexao);
            comando.Parameters.AddWithValue("@categoria", $"%{categoria}%");
            comando.Parameters.AddWithValue("@disponivel", apenasDisponiveis ? 1 : 0);

            using var leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                listaProdutos.Add(MapearProduto(leitor));
            }

            return listaProdutos;
        }

        // ==========================================
        // 5. ALTERAR / ATUALIZAR (UPDATE)
        // ==========================================
        public void Atualizar(Produto produto)
        {
            using var conexao = Banco.ObterConexao(); 
            conexao.Open();

            string sql = @"UPDATE PRODUTO 
                          SET nome = @nome, 
                              descricao = @descricao, 
                              preco_atual = @precoAtual, 
                              disponivel = @disponivel, 
                              categoria = @categoria 
                          WHERE id_produto = @id";

            using var comando = new SqliteCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", produto.IdProduto);
            comando.Parameters.AddWithValue("@nome", produto.Nome);
            comando.Parameters.AddWithValue("@descricao", (object?)produto.Descricao ?? DBNull.Value);
            comando.Parameters.AddWithValue("@precoAtual", produto.PrecoAtual);
            comando.Parameters.AddWithValue("@disponivel", produto.Disponivel ? 1 : 0);
            comando.Parameters.AddWithValue("@categoria", produto.Categoria);

            comando.ExecuteNonQuery();
        }

        // ==========================================
        // 6. EXCLUIR (DELETE)
        // ==========================================
        public void Excluir(int id)
        {
            using var conexao = Banco.ObterConexao(); 
            conexao.Open();

            string sql = "DELETE FROM PRODUTO WHERE id_produto = @id";

            using var comando = new SqliteCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }

        // ==========================================
        // MÉTODO AUXILIAR PARA EVITAR DUPLICAÇÃO DE CÓDIGO
        // ==========================================
        private static Produto MapearProduto(SqliteDataReader leitor)
        {
            return new Produto
            {
                IdProduto = leitor.GetInt32(0),
                Nome = leitor.GetString(1),
                // Trata a leitura de colunas que podem vir nulas (NULL)
                Descricao = leitor.IsDBNull(2) ? null : leitor.GetString(2),
                PrecoAtual = leitor.GetDecimal(3),
                Disponivel = leitor.GetInt32(4) == 1,
                Categoria = leitor.GetString(5)
            };
        }
    }
}