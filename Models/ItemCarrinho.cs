namespace Projeto_do_semestre.Models
{
    public class ItemCarrinho
    {
        public Produto Produto { get; set; } = new();
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
        public decimal Subtotal => Produto.PrecoAtual * Quantidade;
    }
}
