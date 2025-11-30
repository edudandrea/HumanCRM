using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HumanCRM_Api.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace HumanCRM_Api.Models
{
    public class CadastroPF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório")]
        public string? NomeCompleto { get; set; }        

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [Cpf(ErrorMessage = "O CPF informado é inválido")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 números")]
        public int CPF { get; set; }

        public DateTime DataNascimento { get; set; }

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
        public int CanalPreferencial { get; set; }
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