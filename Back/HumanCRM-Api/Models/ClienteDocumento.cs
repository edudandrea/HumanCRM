namespace HumanCRM_Api.Models
{
    public class ClienteDocumento
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }

        public string NomeArquivo { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Tamanho { get; set; }

        public byte[] Arquivo { get; set; } = null!;
        public DateTime CriadoEm { get; set; }
    }
}