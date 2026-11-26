using Microsoft.Data.Sqlite;

namespace Projeto_do_semestre.Data
{
    public class Banco
    {
        private const string ConnectionString =
            "Data Source=BD_cafeteria.db";

        public static SqliteConnection ObterConexao()
        {
            var conexao = new SqliteConnection(ConnectionString);
            conexao.Open();

            return conexao;
        }

        public static void CriarBanco()
        {
            using var conexao = ObterConexao();

            string sql = """
                CREATE TABLE IF NOT EXISTS Produto
                (
                    id_produto INTEGER PRIMARY KEY AUTOINCREMENT,
                    nome TEXT NOT NULL,
                    descricao TEXT NOT NULL,
                    preco_atual REAL NOT NULL,
                    disponivel INTEGER DEFAULT 1,
                    categoria TEXT NOT NULL,
                    imagem TEXT
                );
                """;

            using var comando = new SqliteCommand(sql, conexao);
            comando.ExecuteNonQuery();

            // Verifica se a coluna imagem já existe
            bool possuiImagem = false;

            string consulta = "PRAGMA table_info(Produto);";

            using var verificar = new SqliteCommand(consulta, conexao);
            using var leitor = verificar.ExecuteReader();

            while (leitor.Read())
            {
                if (leitor["name"].ToString() == "imagem")
                {
                    possuiImagem = true;
                    break;
                }
            }

            leitor.Close();

            // Adiciona a coluna somente se ela ainda não existir
            if (!possuiImagem)
            {
                string alterar =
                    "ALTER TABLE Produto ADD COLUMN imagem TEXT;";

                using var comandoAlterar =
                    new SqliteCommand(alterar, conexao);

                comandoAlterar.ExecuteNonQuery();
            }
        }
    }
}