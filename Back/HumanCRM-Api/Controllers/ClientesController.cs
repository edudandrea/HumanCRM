using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HumanCRM_Api.Data;
using HumanCRM_Api.Dto;
using HumanCRM_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HumanCRM_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : Controller
    {
        private readonly DataContext _context;
        public ClientesController(DataContext context)
        {
            _context = context;
        }

        //---------- GET -----------//
        [HttpGet]
        [AllowAnonymous]
        public IActionResult BuscarClientes(
                            [FromQuery] int? id,
                            [FromQuery] string? nome,
                            [FromQuery] int cpfCnpj,
                            [FromQuery] string? telefone)
        {
            var query = _context.Clientes.AsQueryable();

            if(id == null &&
                string.IsNullOrEmpty(nome) &&
                cpfCnpj == null &&
                string.IsNullOrEmpty(telefone))
            {
                return BadRequest("Informe ao menos um filtro");
            }

            query = query.Where(c => 
                (id != null && c.Id == id) || 
                (!string.IsNullOrEmpty(nome) && c.Nome.Contains(nome)) ||
                (cpfCnpj == null && c.CpfCnpj == cpfCnpj) ||
                (!string.IsNullOrEmpty(telefone) && c.Telefone.Contains(telefone))

                );

            return Ok(query.ToList());
        }

        

        [HttpGet("prospeccoes/{id}")]
        public async Task<IActionResult> GetProspeccaoById(int id)
        {
            var prospeccao = _context.ProspeccoesClientes
                .FirstOrDefault(p => p.Id == id);

            if (prospeccao == null)
                return NotFound();

            return Ok(prospeccao);
        }

        //------------- POST -------------//

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddCliente([FromBody] AddClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Clientes
            {
                Nome = dto.Nome,
                CpfCnpj = dto.CpfCnpj,
                Telefone = dto.Telefone,
                Email = dto.Email,
                TipoPessoa = dto.TipoPessoa,

                // Campos extras da entidade preenchidos com default/null
                Rua = string.Empty,
                Numero = 0,
                Bairro = string.Empty,
                Cep = 0,
                Cidade = string.Empty,
                Estado = string.Empty,
                RedeSocial = string.Empty,
                OrigemContato = string.Empty,
                ResponsavelContato = string.Empty,
                Obs = string.Empty
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarClientes), new { id = cliente.Id }, cliente);
        }



        [HttpPost("{clienteId}/prospeccoes")]
        public async Task<IActionResult> AddProspeccao(
        int clienteId,
        [FromBody] ProspeccaoClienteDto dto)
        {
            // 1) Verifica se o cliente existe
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
                return NotFound($"Cliente com Id {clienteId} não encontrado.");

            // 2) Cria a prospecção vinculada ao cliente
            var prospeccao = new ProspeccaoCliente
            {
                ClienteId = clienteId,
                Etapa = dto.Etapa,
                Probabilidade = dto.Probabilidade,
                OrigemContato = dto.OrigemContato,
                InteressePrincipal = dto.InteressePrincipal,
                Necessidade = dto.Necessidade,
                DataProximoContato = dto.DataProximoContato,
                Canal = dto.Canal,
                Responsavel = dto.Responsavel,
                Observacoes = dto.Observacoes,
                DataCriacao = DateTime.Now
            };

            _context.ProspeccoesClientes.Add(prospeccao); // nome do DbSet aqui
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProspeccaoById),   // cria esse método se quiser
                new { id = prospeccao.Id },
                prospeccao);
        }

        //---------------- UPDATE ------------------//

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateClientes([FromBody] Clientes clientes)
        {
            var clientesToUpdate = _context.Clientes.FirstOrDefault(clientes => clientes.Id == clientes.Id);
            if (clientesToUpdate == null)
            {
                return NotFound();
            }

            clientesToUpdate.Nome = clientes.Nome;
            clientesToUpdate.TipoPessoa = clientes.TipoPessoa;
            clientesToUpdate.CpfCnpj = clientes.CpfCnpj;
            clientesToUpdate.Cep = clientes.Cep;
            clientesToUpdate.Rua = clientes.Rua;
            clientesToUpdate.Numero = clientes.Numero;
            clientesToUpdate.Bairro = clientes.Bairro;
            clientesToUpdate.Cidade = clientes.Cidade;
            clientesToUpdate.Complemento = clientes.Complemento;
            clientesToUpdate.Telefone = clientes.Telefone;
            clientesToUpdate.Celular = clientes.Celular;
            clientesToUpdate.Email = clientes.Email;
            clientesToUpdate.RedeSocial = clientes.RedeSocial;
            clientesToUpdate.ResponsavelContato = clientes.ResponsavelContato;
            clientesToUpdate.OrigemContato = clientes.OrigemContato;
            clientesToUpdate.Obs = clientes.Obs;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //------------- DELETE -------------//

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteClienteById(int id)
        {
            var client = _context.Clientes.FirstOrDefault(clients => clients.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            _context.Clientes.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

    public class ClienteCreateDto
    {
    }
}