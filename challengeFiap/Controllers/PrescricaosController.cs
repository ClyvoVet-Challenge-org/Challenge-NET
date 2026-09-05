using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;

[Route("api/[controller]")]
[ApiController]
public class PrescricaosController : ControllerBase
{
    private readonly AppDbContext _context;
    public PrescricaosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Prescricao

    /// <summary>
    /// Relatorio de daods prescrição
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio prescrição</returns>
    [HttpGet]
    [Route("relatorio/prescricao")]
    public async Task<ActionResult<IEnumerable<Prescricao>>> GetAllPrescricao()
    {
        var prescricaoRelatorio = await _context.Prescricaos.ToListAsync();
        return Ok(prescricaoRelatorio);
    }

    // GET: api/Prescricao/5

    /// <summary>
    /// Relatorio de prescrição
    /// </summary>
    /// <param name="id_prescricao">Buscar pelo id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/prescricao/{id_prescricao:int}")]
    public async Task<ActionResult<Prescricao>> GetPrescricao(int id_prescricao)
    {
        try
        {
            var prescricao = await _context.Prescricaos.FindAsync(id_prescricao);

            if (prescricao == null)
            {
                return NotFound("Id de prescrição não encontrado");
            }

            return Ok(prescricao);
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
        
    }

    // PUT: api/Prescricao/5

    /// <summary>
    /// Atualizar dados de prescrição
    /// </summary>
    /// <param name="id_prescricao">ID para pode atualizar</param>
    /// <param name="prescricao">Dados para serem inseridos</param>
    /// <response code="204">Prescrição atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Prescrição não encontrado</response>
    /// <returns></returns>
    [HttpPut]
    [Route("atualizar/prescricao/{id_prescricao:int}")]
    public async Task<IActionResult> PutPrescricao(int id_prescricao, Prescricao prescricao)
    {
        if (id_prescricao != prescricao.Id_prescricao)
        {
            return BadRequest("Id da prescrição está errado.");
        }

        try
        {
            _context.Entry(prescricao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PrescricaoExists(id_prescricao))
            {
                return NotFound("Id prescrição não encontrado");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em atualizar: {ex.Message}");
        }
    }
    private bool PrescricaoExists(int id_prescricao)
    {
        return _context.Prescricaos.FirstOrDefault(e => e.Id_prescricao == id_prescricao) != null;
    }

    // POST: api/Prescricao

    /// <summary>
    /// Criar Precrição
    /// </summary>
    /// <param name="prescricao">Criação de dados a prescrição</param>
    /// <response code="201">Precisção criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criar prescrição</returns>
    [HttpPost]
    [Route("criar/prescricao")]
    public async Task<ActionResult<Prescricao>> PostPrescricao(Prescricao prescricao)
    {
        try
        {
            var consultaExiste = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id_consulta == prescricao.Id_consulta);
            if (consultaExiste != null)
            {
                _context.Prescricaos.Add(prescricao);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPrescricao", new { id_prescricao = prescricao.Id_prescricao }, prescricao);
            }
            else 
            {
                return BadRequest($"Id consulta não encontrado");
            }
        }
        catch(Exception ex)
        {
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }
        
    }

    // DELETE: api/Prescricao/5

    /// <summary>
    /// Remove dados Precrição
    /// </summary>
    /// <param name="id_prescricao">Id para pode remover: </param>
    /// <response code="204">Prescrição removido com sucesso.</response>
    /// <response code="404">Prescrição não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletado: </returns>
    [HttpDelete]
    [Route("deleta/prescricao/{id_prescricao:int}")]
    public async Task<IActionResult> DeletePrescricao(int id_prescricao)
    {
        try
        {
            var prescricao = await _context.Prescricaos.FindAsync(id_prescricao);
            if (prescricao == null)
            {
                return NotFound("Id prescrição não encontrado");
            }

            _context.Prescricaos.Remove(prescricao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
        
    }
}
