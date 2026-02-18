namespace HumanCRM_Api.Dto
{
    public class ProspeccaoResponseDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string? ContatoProspeccao { get; set; }
        public string? Etapa { get; set; }
        public int Probabilidade { get; set; }
        public string? OrigemContato { get; set; }
        public string? InteressePrincipal { get; set; }
        public string? Necessidade { get; set; }
        public DateOnly? DataProximoContato { get; set; }
        public string? Canal { get; set; }
        public string? Responsavel { get; set; }
        public string? Observacoes { get; set; }
        public DateOnly DataCriacao { get; set; }
    }
}