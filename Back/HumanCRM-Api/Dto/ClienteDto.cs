namespace HumanCRM_Api.Dto
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string TipoPessoa { get; set; } = string.Empty;
        public string? RG { get; set; }
        public string? CpfCnpj { get; set; }
        public int Cep { get; set; }
        public string Rua { get; set; } = string.Empty;
        public int? Numero { get; set; }
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RedeSocial { get; set; } = string.Empty;
        public string ResponsavelContato { get; set; } = string.Empty;
        public string OrigemContato { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public string? OrgaoExpedidor { get; set; }
        public int Sexo { get; set; }
        public int EstadoCivil { get; set; }
        public int DDD { get; set; }
        public string? Telefone { get; set; }
        public int Celular { get; set; }
        public List<ProspeccaoResponseDto>? Prospeccoes { get; set; }
    }
}