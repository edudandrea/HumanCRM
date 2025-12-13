namespace HumanCRM_Api.Dto
{
    public class ProspeccaoClienteDto
    {
        public string Etapa { get; set; } = string.Empty;
        public int Probabilidade { get; set; }
        public string OrigemContato { get; set; } = string.Empty;
        public string InteressePrincipal { get; set; } = string.Empty;
        public string Necessidade { get; set; } = string.Empty;
        public DateTime DataProximoContato { get; set; }
        public string Canal { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
    }
}