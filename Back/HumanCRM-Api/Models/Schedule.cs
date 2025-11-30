using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanCRM_Api.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgendamentoId { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public DateTime DataAgendamento { get; set; }
        [Required]
        public string? Descricao { get; set; }
        [Required]
        public string? ResponsavelAgendamento { get; set; }

    }
}