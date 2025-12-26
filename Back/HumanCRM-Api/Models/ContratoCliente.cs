namespace HumanCRM_Api.Models
{
    public class ContratoCliente
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Clientes? Clientes { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public string TipoArquivo { get; set; } = string.Empty;
        public DateTime DataUpload { get; set; } = DateTime.Now;
    }
}