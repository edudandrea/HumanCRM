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
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public async Task<IActionResult> BuscarClientes(
    [FromQuery] int? id,
    [FromQuery] string? nome,
    [FromQuery] string? cpfCnpj,
    [FromQuery] string? telefone)
        {
            if (id == null &&
                string.IsNullOrEmpty(nome) &&
                string.IsNullOrEmpty(cpfCnpj) &&
                string.IsNullOrEmpty(telefone))
            {
                return BadRequest("Informe ao menos um filtro");
            }

            var clientes = await _context.Clientes
                .Where(c =>
                    (id != null && c.Id == id) ||
                    (!string.IsNullOrEmpty(nome) && c.Nome.Contains(nome)) ||
                    (!string.IsNullOrEmpty(cpfCnpj) && c.CpfCnpj.Contains(cpfCnpj)) ||
                    (!string.IsNullOrEmpty(telefone) && c.Telefone.Contains(telefone))
                )
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    CpfCnpj = c.CpfCnpj,
                    RG = c.RG,
                    Telefone = c.Telefone,
                    TipoPessoa = c.TipoPessoa,
                    Cep = c.Cep,
                    Rua = c.Rua,
                    Complemento = c.Complemento,
                    Bairro = c.Bairro,
                    Cidade = c.Cidade,
                    Estado = c.Estado,
                    RedeSocial = c.RedeSocial,
                    DDD = c.DDD,
                    Email = c.Email,
                    Celular = c.Celular,
                    ResponsavelContato = c.ResponsavelContato,
                    OrigemContato = c.OrigemContato,
                    Observacoes = c.Obs,
                    Sexo = c.Sexo,
                    EstadoCivil = c.EstadoCivil,
                    DataNascimento = c.DataNascimento,
                    Numero = c.Numero,
                    OrgaoExpedidor = c.OrgaoExpedidor,


                    Prospeccoes = c.Prospeccoes
                        .OrderByDescending(p => p.DataCriacao)
                        .Select(p => new ProspeccaoResponseDto
                        {
                            Id = p.Id,
                            ClienteId = p.ClienteId,
                            Etapa = p.Etapa,
                            Probabilidade = (int)p.Probabilidade,
                            Canal = p.Canal,
                            Responsavel = p.Responsavel,
                            DataCriacao = p.DataCriacao,
                            DataProximoContato = p.DataProximoContato,
                            Necessidade = p.Necessidade,
                            InteressePrincipal = p.InteressePrincipal,
                            OrigemContato = p.OrigemContato,
                            Observacoes = p.Observacoes
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(clientes);
        }

        //------------- POST -------------//

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddCliente([FromBody] ClienteDto dto)
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
                DDD = dto.DDD,

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
                Obs = string.Empty,
                Sexo = dto.Sexo,
                EstadoCivil = dto.EstadoCivil,
                DataNascimento = dto.DataNascimento
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
            // ✔ valida existência sem carregar entidade
            var clienteExiste = await _context.Clientes
                .AnyAsync(c => c.Id == clienteId);

            if (!clienteExiste)
                return NotFound($"Cliente com Id {clienteId} não encontrado.");

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
                DataCriacao = DateTime.UtcNow
            };

            _context.ProspeccoesClientes.Add(prospeccao);
            await _context.SaveChangesAsync();

            var response = new ProspeccaoResponseDto
            {
                Id = prospeccao.Id,
                ClienteId = prospeccao.ClienteId,
                Etapa = prospeccao.Etapa,
                ContatoProspeccao = prospeccao.ContatoProspeccao,
                Probabilidade = (int)prospeccao.Probabilidade,
                OrigemContato = prospeccao.OrigemContato,
                InteressePrincipal = prospeccao.InteressePrincipal,
                Necessidade = prospeccao.Necessidade,
                DataProximoContato = prospeccao.DataProximoContato,
                Canal = prospeccao.Canal,
                Responsavel = prospeccao.Responsavel,
                Observacoes = prospeccao.Observacoes,
                DataCriacao = prospeccao.DataCriacao
            };

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("{clienteId}/contratos")]
        public async Task<IActionResult> AddContrato(
                int clienteId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo foi enviado.");
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
                return NotFound($"Cliente com Id {clienteId} não encontrado.");

            // Lógica para salvar o arquivo no servidor
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Contratos");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var nomeArquivo = $"{Guid.NewGuid()}_{file.FileName}";
            var caminhoArquivo = Path.Combine(folder, nomeArquivo);

            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var contrato = new ContratoCliente
            {
                ClienteId = clienteId,
                NomeArquivo = file.FileName,
                CaminhoArquivo = caminhoArquivo,
                TipoArquivo = file.ContentType,
                DataUpload = DateTime.UtcNow
            };

            _context.ContratosClientes.Add(contrato);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, contrato);
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
            clientesToUpdate.Estado = clientes.Estado;
            clientesToUpdate.Complemento = clientes.Complemento;
            clientesToUpdate.RG = clientes.RG;
            clientesToUpdate.DDD = clientes.DDD;
            clientesToUpdate.Telefone = clientes.Telefone;
            clientesToUpdate.Celular = clientes.Celular;
            clientesToUpdate.Email = clientes.Email;
            clientesToUpdate.RedeSocial = clientes.RedeSocial;
            clientesToUpdate.RazaoSocial = clientes.RazaoSocial;
            clientesToUpdate.IE = clientes.IE;
            clientesToUpdate.IM = clientes.IM;
            clientesToUpdate.DataContato = clientes.DataContato;
            clientesToUpdate.DataCadastro = clientes.DataCadastro;
            clientesToUpdate.DataFuncacao = clientes.DataFuncacao;
            clientesToUpdate.ResponsavelContato = clientes.ResponsavelContato;
            clientesToUpdate.OrigemContato = clientes.OrigemContato;
            clientesToUpdate.Obs = clientes.Obs;
            clientesToUpdate.OrgaoExpedidor = clientes.OrgaoExpedidor;
            clientesToUpdate.Sexo = clientes.Sexo;
            clientesToUpdate.EstadoCivil = clientes.EstadoCivil;
            clientesToUpdate.DataNascimento = clientes.DataNascimento;
            clientesToUpdate.OrgaoExpedidor = clientes.OrgaoExpedidor;
            clientesToUpdate.DataContato = clientes.DataContato;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{clienteId}/prospeccoes/{prospeccaoId}")]
        public async Task<IActionResult> UpdateProspeccao(
    int clienteId,
    int prospeccaoId,
    [FromBody] ProspeccaoClienteDto dto)
        {
            var prospeccao = await _context.ProspeccoesClientes
                .FirstOrDefaultAsync(p =>
                    p.Id == prospeccaoId &&
                    p.ClienteId == clienteId);

            if (prospeccao == null)
                return NotFound("Prospecção não encontrada para este cliente.");

            prospeccao.Etapa = dto.Etapa;
            prospeccao.Probabilidade = dto.Probabilidade;
            prospeccao.OrigemContato = dto.OrigemContato;
            prospeccao.InteressePrincipal = dto.InteressePrincipal;
            prospeccao.Necessidade = dto.Necessidade;
            prospeccao.DataProximoContato = dto.DataProximoContato;
            prospeccao.Canal = dto.Canal;
            prospeccao.Responsavel = dto.Responsavel;
            prospeccao.Observacoes = dto.Observacoes;
            prospeccao.ContatoProspeccao = dto.ContatoProspeccao;

            await _context.SaveChangesAsync();

            return Ok(new ProspeccaoResponseDto
            {
                Id = prospeccao.Id,
                ClienteId = prospeccao.ClienteId,
                Etapa = prospeccao.Etapa,
                Probabilidade = (int)prospeccao.Probabilidade,
                OrigemContato = prospeccao.OrigemContato,
                InteressePrincipal = prospeccao.InteressePrincipal,
                Necessidade = prospeccao.Necessidade,
                DataProximoContato = prospeccao.DataProximoContato,
                Canal = prospeccao.Canal,
                Responsavel = prospeccao.Responsavel,
                Observacoes = prospeccao.Observacoes,
                DataCriacao = prospeccao.DataCriacao
            });
        }
    }


}