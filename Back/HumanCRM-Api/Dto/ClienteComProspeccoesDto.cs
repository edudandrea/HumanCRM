namespace HumanCRM_Api.Dto
{
    public class ClienteComProspeccoesDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? TipoPessoa { get; set; }
        public int CpfCnpj { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? ResponsavelContato { get; set; }

        public List<ProspeccaoResponseDto>? Prospeccoes { get; set; }
    }
}