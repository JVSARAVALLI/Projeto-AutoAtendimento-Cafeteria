namespace Projeto_do_semestre.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal PrecoAtual { get; set; }
        public bool Disponivel { get; set; } = true;
        public string Categoria { get; set; } = string.Empty;
    }
}
