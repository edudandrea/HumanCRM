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
                string.IsNullOrWhiteSpace(nome) &&
                string.IsNullOrWhiteSpace(cpfCnpj) &&
                string.IsNullOrWhiteSpace(telefone))
            {
                return BadRequest("Informe ao menos um filtro");
            }

            var query = _context.Clientes
                .AsNoTracking()
                .AsQueryable();

            if (id.HasValue)
                query = query.Where(c => c.Id == id.Value);

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(c => EF.Functions.ILike(c.Nome ?? "", $"%{nome}%"));

            if (!string.IsNullOrWhiteSpace(cpfCnpj))
                query = query.Where(c => (c.CpfCnpj ?? "").Contains(cpfCnpj));

            if (!string.IsNullOrWhiteSpace(telefone))
                query = query.Where(c => EF.Functions.ILike(c.Telefone ?? "", $"%{telefone}%"));


            var clientes = await query
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
            try
            {
                if (dto == null) return BadRequest("Payload inválido.");
                if (string.IsNullOrWhiteSpace(dto.Nome)) return BadRequest("Nome é obrigatório.");
                if (string.IsNullOrWhiteSpace(dto.TipoPessoa)) return BadRequest("TipoPessoa é obrigatório.");
                if (dto.DDD <= 0) return BadRequest("DDD é obrigatório.");
                if (dto.Numero <= 0) return BadRequest("Número é obrigatório.");

                var cliente = new Clientes
                {
                    Nome = dto.Nome.Trim(),
                    TipoPessoa = dto.TipoPessoa.Trim(),
                    CpfCnpj = dto.CpfCnpj,
                    RG = dto.RG,
                    DDD = dto.DDD,
                    Numero = dto.Numero,

                    Telefone = dto.Telefone,
                    Celular = dto.Celular,
                    Email = dto.Email,

                    Cep = dto.Cep,
                    Rua = dto.Rua,
                    Bairro = dto.Bairro,
                    Cidade = dto.Cidade,
                    Estado = dto.Estado,
                    Complemento = dto.Complemento,

                    Sexo = dto.Sexo,
                    EstadoCivil = dto.EstadoCivil,
                    OrgaoExpedidor = dto.OrgaoExpedidor,

                    DataCadastro = DateOnly.FromDateTime(DateTime.UtcNow),

                    // ✅ importante: garantir listas não nulas
                    Prospeccoes = new List<ProspeccaoCliente>(),
                    Contratos = new List<ContratoCliente>()
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return Ok(new { cliente.Id });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    inner2 = ex.InnerException?.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
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
                DataCriacao = DateOnly.FromDateTime(DateTime.UtcNow)
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

        [HttpPut("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateClientes(int id, [FromBody] ClienteDto dto)
        {
            try
            {
                var c = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (c == null) return NotFound();

                // validações mínimas (o que é NOT NULL no banco)
                if (string.IsNullOrWhiteSpace(dto.Nome))
                    return BadRequest("Nome é obrigatório.");
                if (string.IsNullOrWhiteSpace(dto.TipoPessoa))
                    return BadRequest("TipoPessoa é obrigatório.");
                if (dto.DDD <= 0)
                    return BadRequest("DDD é obrigatório.");
                if (dto.Numero <= 0)
                    return BadRequest("Número é obrigatório.");

                c.Nome = dto.Nome.Trim();
                c.TipoPessoa = dto.TipoPessoa.Trim();
                c.CpfCnpj = dto.CpfCnpj;
                c.RG = dto.RG;

                c.Telefone = dto.Telefone;           // string
                c.Email = dto.Email;
                c.Celular = dto.Celular;             // int?/long? conforme você definiu

                c.Cep = dto.Cep;
                c.Rua = dto.Rua;
                c.Numero = dto.Numero;
                c.Bairro = dto.Bairro;
                c.Cidade = dto.Cidade;
                c.Estado = dto.Estado;
                c.Complemento = dto.Complemento;

                c.DDD = dto.DDD;

                c.RedeSocial = dto.RedeSocial;
                c.ResponsavelContato = dto.ResponsavelContato;
                c.OrigemContato = dto.OrigemContato;
                c.Obs = dto.Observacoes;             // seu DTO usa Observacoes

                c.OrgaoExpedidor = dto.OrgaoExpedidor;
                c.Sexo = dto.Sexo;
                c.EstadoCivil = dto.EstadoCivil;
                c.DataNascimento = dto.DataNascimento;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
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