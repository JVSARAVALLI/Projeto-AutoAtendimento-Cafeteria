namespace Projeto_do_semestre.Models
{
    public class ItemPedido
    {
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string? Observacao { get; set; }
    }
}
