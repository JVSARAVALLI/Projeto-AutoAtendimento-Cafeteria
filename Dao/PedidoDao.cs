using Microsoft.Data.Sqlite;
using Projeto_do_semestre.Data;
using Projeto_do_semestre.Models;


namespace Projeto_do_semestre.Dao
{
    public class PedidoDao
    {
        public int CriarPedido(Pedido pedido)
        {
            using var conexao = Banco.ObterConexao();
            conexao.Open();

            using var transacao = conexao.BeginTransaction();

            try
            {
                //  Gera a próxima senha numérica do dia
                var sqlSenha = "SELECT COALESCE(MAX(numero_senha), 0) + 1 FROM PEDIDO;";
                using var cmdSenha = new SqliteCommand(sqlSenha, conexao, transacao);
                int proximaSenha = Convert.ToInt32(cmdSenha.ExecuteScalar());
                pedido.NumeroSenha = proximaSenha;

                // Insere na tabela PEDIDO
                var sqlPedido = @"
                INSERT INTO PEDIDO (numero_senha, status, valor_total, id_atendente, id_cliente) 
                VALUES (@numeroSenha, @status, @valorTotal, @idAtendente, @idCliente);
                SELECT last_insert_rowid();";

                using var cmdPedido = new SqliteCommand(sqlPedido, conexao, transacao);
                cmdPedido.Parameters.AddWithValue("@numeroSenha", pedido.NumeroSenha);
                cmdPedido.Parameters.AddWithValue("@status", pedido.Status);
                cmdPedido.Parameters.AddWithValue("@valorTotal", pedido.ValorTotal);
                cmdPedido.Parameters.AddWithValue("@idAtendente", (object?)pedido.IdAtendente ?? DBNull.Value);
                cmdPedido.Parameters.AddWithValue("@idCliente", (object?)pedido.IdCliente ?? DBNull.Value);

                int idPedidoGerado = Convert.ToInt32(cmdPedido.ExecuteScalar());

                // Insere na tabela CONTEM
                var sqlContem = @"
                INSERT INTO CONTEM (id_pedido, id_produto, quantidade, preco_unitario, observacao)
                VALUES (@idPedido, @idProduto, @quantidade, @precoUnitario, @observacao);";

                foreach (var item in pedido.Itens)
                {
                    using var cmdItem = new SqliteCommand(sqlContem, conexao, transacao);
                    cmdItem.Parameters.AddWithValue("@idPedido", idPedidoGerado);
                    cmdItem.Parameters.AddWithValue("@idProduto", item.IdProduto);
                    cmdItem.Parameters.AddWithValue("@quantidade", item.Quantidade);
                    cmdItem.Parameters.AddWithValue("@precoUnitario", item.PrecoUnitario);
                    cmdItem.Parameters.AddWithValue("@observacao", (object?)item.Observacao ?? DBNull.Value);
                    cmdItem.ExecuteNonQuery();
                }

                transacao.Commit();
                return idPedidoGerado;
            }
            catch
            {
                transacao.Rollback();
                throw;
            }
        }

        public List<Pedido> ListarPedidosCozinha()
        {
            var lista = new List<Pedido>();
            using var conexao = Banco.ObterConexao();
            conexao.Open();

            var sql = @"
            SELECT id_pedido, numero_senha, data_hora_pedido, status, valor_total 
            FROM PEDIDO 
            WHERE status IN ('pendente', 'em_preparo') 
            ORDER BY id_pedido ASC;";

            using var cmd = new SqliteCommand(sql, conexao);
            using var leitor = cmd.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(new Pedido
                {
                    IdPedido = leitor.GetInt32(0),
                    NumeroSenha = leitor.GetInt32(1),
                    DataHoraPedido = leitor.GetDateTime(2),
                    Status = leitor.GetString(3),
                    ValorTotal = leitor.GetDecimal(4)
                });
            }
            return lista;
        }

        public void AtualizarStatus(int idPedido, string novoStatus)
        {
            using var conexao = Banco.ObterConexao();
            conexao.Open();

            var sql = "UPDATE PEDIDO SET status = @status WHERE id_pedido = @idPedido;";
            using var cmd = new SqliteCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@status", novoStatus);
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            cmd.ExecuteNonQuery();
        }
    }
}
