using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;

namespace Cafeteria.Data
{
    public class Banco
    {
        private const string ConnectionString =
            "Data Source=produtos.db";
        
        public static SqliteConnection ObterConexao()
        {
            var conexao = new SqliteConnection(ConnectionString);
            conexao.Open(); // Já abre a conexão para o DAO usar diretamente
            return conexao;
        }
        public static void CriarBanco()
        {
            using var conexao = new SqliteConnection(ConnectionString);
            conexao.Open();

            string sql ="""
                 CREATE TABLE IF NOT EXISTS Produto
                (
                    id_produto INTEGER PRIMARY KEY AUTOINCREMENT,
                    nome TEXT NOT NULL,
                    descricao TEXT NOT NULL,
                    preco_atual REAL NOT NULL,
                    disponivel INTEGER DEFAULT 1,
                    categoria TEXT NOT NULL
                )
                """;

            using var comando =
                new SqliteCommand(sql, conexao);

            comando.ExecuteNonQuery();
        }
    }


}

