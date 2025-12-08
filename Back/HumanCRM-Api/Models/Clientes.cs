using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HumanCRM_Api.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace HumanCRM_Api.Models
{
    public class Clientes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }              // PK
        public string Codigo { get; set; } = ""; // cliente.codigo
        public string Nome { get; set; } = "";   // cliente.nome

        // "Física" ou "Jurídica"
        public string TipoPessoa { get; set; } = "Física";

        // Dados básicos (opcional, mas combina com sua tela)
        public string? CpfCnpj { get; set; }
        public string? Cep { get; set; }
        public string? Rua { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Complemento { get; set; }

        // Contato
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public string? RedeSocial { get; set; }
        public string? ResponsavelContato { get; set; }
        public string? OrigemContato { get; set; }
        public string? Observacoes { get; set; }

        public DateTime? DataCadastro { get; set; }

        // Navegação – 1 Cliente -> muitas Prospecções
        public ICollection<ProspeccaoCliente> Prospeccoes { get; set; } = new List<ProspeccaoCliente>();
    }
}