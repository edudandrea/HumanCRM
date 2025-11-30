using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HumanCRM_Api.Attributes;

namespace HumanCRM_Api.Models
{
    public class CadastroPJ
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int clientId { get; set; }

        [Required(ErrorMessage = "A Razão Social é obrigatória")]
        public string? RazaoSocial { get; set; }

        [Required(ErrorMessage = "O Nome Fantasia é obrigatório")]
        public string? NomeFantasia { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [Cnpj(ErrorMessage = "O CNPJ informado é inválido")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter exatamente 14 números")]
        public int CNPJ { get; set; }

        public DateTime DataFundacao { get; set; }

        [Required(ErrorMessage = "O Endereço é obrigatório")]
        public string? Endereco { get; set; }

        [Required(ErrorMessage = "A Cidade é obrigatória")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "O Estado é obrigatório")]
        public string? Estado { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "O CEP deve conter exatamente 8 números")]
        public int CEP { get; set; }

        [Required(ErrorMessage = "O Telefone é obrigatório")]
        public int Phone { get; set; }
        
        [Required(ErrorMessage = "O Email é obrigatório")]
        public string? Email { get; set; }
        public string? RedeSocial { get; set; }
        public string? CanalPreferencial { get; set; }
        public string? OrigemContato { get; set; }
        public DateTime DataContato { get; set; }
        public string? ResponsavelContato { get; set; }

        [Required(ErrorMessage = "O Tipo de Cliente é obrigatório")]
        public int tipoCliente { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, MinimumLength = 4, ErrorMessage = "A descrição deve ter entre 4 e 1000 caracteres")]
        public string? descricao { get; set; }
    }
}