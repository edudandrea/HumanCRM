using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanCRM_Api.Attributes
{
    public class CnpjAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("O CNPJ é obrigatório");

            string cnpj = value.ToString().Trim();

            // Remove tudo que não for número
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            // Verifica o tamanho
            if (cnpj.Length != 14)
                return new ValidationResult("O CNPJ deve conter 14 dígitos numéricos");

            // Verifica se todos os dígitos são iguais (ex: 11111111111111)
            if (cnpj.Distinct().Count() == 1)
                return new ValidationResult("O CNPJ informado é inválido");

            // Calcula e valida os dígitos verificadores
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            if (!cnpj.EndsWith(digito))
                return new ValidationResult("O CNPJ informado é inválido");

            return ValidationResult.Success;
        }
    }
}