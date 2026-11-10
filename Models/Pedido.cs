namespace Projeto_do_semestre.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int NumeroSenha { get; set; }
        public DateTime DataHoraPedido { get; set; } = DateTime.Now;
        public string Status { get; set; } = "pendente"; // ex: pendente, em_preparo, pronto, entregue
        public decimal ValorTotal { get; set; }
        public int? IdAtendente { get; set; }
        public int? IdCliente { get; set; }

        // Mapeia os registros da tabela CONTEM
        public List<ItemPedido> Itens { get; set; } = [];
    }
}
