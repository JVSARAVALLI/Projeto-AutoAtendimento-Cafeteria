namespace Projeto_do_semestre.Models
{
    public class ItemCarrinho
    {
        public Produto Produto { get; set; } = new();
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => PrecoUnitario * Quantidade;
    }
}
