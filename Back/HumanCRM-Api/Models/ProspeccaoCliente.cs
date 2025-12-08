using HumanCRM_Api.Models;

namespace HumanCRM_Api.Models
{
    public class ProspeccaoCliente
    {
        public int Id { get; set; }

        // FK para Cliente
        public int ClienteId { get; set; }
        public Clientes Cliente { get; set; } = null!;

        public string? Etapa { get; set; }              // Lead, Proposta, etc.
        public int? Probabilidade { get; set; }         // 0–100
        public string? OrigemContato { get; set; }      // indicação, rede social...
        public string? InteressePrincipal { get; set; }
        public string? Necessidade { get; set; }
        public DateTime? DataProximoContato { get; set; }
        public string? Canal { get; set; }              // Telefone, WhatsApp, etc.
        public string? Responsavel { get; set; }
        public string? Observacoes { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}