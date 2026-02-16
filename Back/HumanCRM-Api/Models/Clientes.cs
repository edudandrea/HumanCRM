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
        public int Id { get; set; }         
        public string Nome { get; set; } = "";       
        public string TipoPessoa { get; set; } = "Física";  
        public string? RG { get; set; }      
        public string? CpfCnpj { get; set; }
        public int? Cep { get; set; }
        public string? Rua { get; set; }
        public int? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }

        public string? Estado { get; set; }
        public string? Complemento { get; set; }

        // Contato
        public int? DDD { get; set; }
        public string? Telefone { get; set; }
        public int? Celular { get; set; }
        public string? Email { get; set; }
        public string? RedeSocial { get; set; }
        public string? ResponsavelContato { get; set; }
        public string? OrigemContato { get; set; }
        public string? Obs { get; set; }
        public string? RazaoSocial { get; set; }
        public int? IE { get; set; }
        public int? IM { get; set; } 
        public string? OrgaoExpedidor { get; set; }
        public int? Sexo { get; set; }
        public int? EstadoCivil { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataFuncacao { get; set; }  
        public DateTime? DataContato { get; set; }    
        public DateTime? DataNascimento { get; set; } 
        public ICollection<ProspeccaoCliente>? Prospeccoes { get; set; } = new List<ProspeccaoCliente>();
        public ICollection<ContratoCliente> Contratos { get; set; } = new List<ContratoCliente>();
    }
}