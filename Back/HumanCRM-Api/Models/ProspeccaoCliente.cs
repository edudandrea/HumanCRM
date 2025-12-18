using System.Text.Json.Serialization;
using HumanCRM_Api.Models;

namespace HumanCRM_Api.Models
{
    public class ProspeccaoCliente
    {
        public int Id { get; set; }

        // FK para Cliente
        [JsonIgnore]
        public int ClienteId { get; set; }
        public Clientes Cliente { get; set; } = null!;
        public string? ContatoProspeccao { get; set; }
        public string? Etapa { get; set; }
        public int? Probabilidade { get; set; }
        public string? OrigemContato { get; set; }
        public string? InteressePrincipal { get; set; }
        public string? Necessidade { get; set; }
        public DateTime? DataProximoContato { get; set; }
        public string? Canal { get; set; }
        public string? Responsavel { get; set; }
        public string? Observacoes { get; set; }


        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}