using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanCRM_Api.Data;
using HumanCRM_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HumanCRM_Api.Controllers
{
    [ApiController]
    [Route("api/clientes/{clienteId:int}/documentos")]
    public class ClienteDocumentosController : ControllerBase
    {
        private readonly DataContext _context;
        public ClienteDocumentosController(DataContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> Upload([FromRoute] int clienteId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Arquivo inválido.");

            var ct = (file.ContentType ?? "").ToLowerInvariant();
            if (ct != "application/pdf" && ct != "image/png")
                return BadRequest("Somente PDF ou PNG.");

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            var doc = new ClienteDocumento
            {
                ClienteId = clienteId,
                NomeArquivo = file.FileName,
                ContentType = ct,
                Tamanho = file.Length,
                Arquivo = bytes,
                CriadoEm = DateTime.UtcNow
            };

            _context.ClienteDocumentos.Add(doc);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = doc.Id,
                nomeArquivo = doc.NomeArquivo,
                contentType = doc.ContentType,
                tamanho = doc.Tamanho,
                criadoEm = doc.CriadoEm,
                downloadUrl = $"{Request.Scheme}://{Request.Host}/api/clientes/{clienteId}/documentos/{doc.Id}/download"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromRoute] int clienteId)
        {
            var list = await _context.ClienteDocumentos
                .Where(x => x.ClienteId == clienteId)
                .OrderByDescending(x => x.CriadoEm)
                .Select(x => new
                {
                    id = x.Id,
                    nomeArquivo = x.NomeArquivo,
                    contentType = x.ContentType,
                    tamanho = x.Tamanho,
                    criadoEm = x.CriadoEm,
                    downloadUrl = $"{Request.Scheme}://{Request.Host}/api/clientes/{clienteId}/documentos/{x.Id}/download"
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{docId:int}/download")]
        public async Task<IActionResult> Download([FromRoute] int clienteId, [FromRoute] int docId)
        {
            var doc = await _context.ClienteDocumentos
                .FirstOrDefaultAsync(x => x.ClienteId == clienteId && x.Id == docId);

            if (doc == null) return NotFound();

            return File(doc.Arquivo, doc.ContentType, doc.NomeArquivo);
        }

        [HttpDelete("{docId:int}")]
        public async Task<IActionResult> Excluir([FromRoute] int clienteId, [FromRoute] int docId)
        {
            var doc = await _context.ClienteDocumentos
                .FirstOrDefaultAsync(x => x.ClienteId == clienteId && x.Id == docId);

            if (doc == null) return NotFound();

            _context.ClienteDocumentos.Remove(doc);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}