using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;

[Route("api/[controller]")]
[ApiController]
public class ConsultasController : ControllerBase
{
    private readonly AppDbContext _context;
    public ConsultasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Consulta

    /// <summary>
    /// Relaorio consulta
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Buscar</returns>
    [HttpGet]
    [Route("relatorio/consulta")]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetAllConsulta()
    {
        var relatorioConsulta =  await _context.Consultas.ToListAsync();
        return Ok(relatorioConsulta);
    }

    // GET: api/Consulta/5

    /// <summary>
    /// Relatorio de consulta pelo id:
    /// </summary>
    /// <param name="id_consulta">Id para buscar a informação</param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio</returns>
    [HttpGet]
    [Route("relatorio/consulta/{id_consulta:int}")]
    public async Task<ActionResult<Consulta>> GetConsulta(int id_consulta)
    {
        try
        {
            var consulta = await _context.Consultas.FindAsync(id_consulta);

            if (consulta == null)
            {
                return NotFound("Id Consulta não encontrado");
            }

            return Ok(consulta);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
        
    }

    // PUT: api/Consulta/5

    /// <summary>
    /// Atualizar dados consulta
    /// </summary>
    /// <param name="id_consulta">Id consulta para verificar: </param>
    /// <param name="consulta">dados consulta</param>
    /// <response code="204">Consulta atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Consulta não encontrado</response>
    /// <returns></returns>
    [HttpPut]
    [Route("atualizar/consulta/{id_consulta:int}")]
    public async Task<IActionResult> PutConsulta(int id_consulta, Consulta consulta)
    {
        if (id_consulta != consulta.Id_consulta)
        {
            return BadRequest("Id da consulta está incorreto");
        }

        try
        {
            _context.Entry(consulta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ConsultaExists(id_consulta))
            {
                return NotFound("Consulta não encontrada");
            }
            else
            {
                throw;
            }
        }catch(Exception ex)
        {
            return BadRequest($"Erro em atualizar: {ex.Message}");
        }
    }
    private bool ConsultaExists(int id_consulta)
    {
        return _context.Consultas.FirstOrDefault(e => e.Id_consulta == id_consulta) != null;
    }

    // POST: api/Consulta

    /// <summary>
    /// Criação de consulta
    /// </summary>
    /// <param name="consulta">Dados para ser inseridos</param>
    /// <response code="201">Consulta criada com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação de dados consulta</returns>
    [HttpPost]
    [Route("criar/consulta")]
    public async Task<ActionResult<Consulta>> PostConsulta(Consulta consulta)
    {
        try
        {
            var animalExistente = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id_animal == consulta.Id_animal);

            var vetExistente = await _context.Veterinarios
                .FirstOrDefaultAsync(v => v.Id_vet == consulta.Id_vet);

            if (animalExistente != null && vetExistente != null)
            {
                _context.Consultas.Add(consulta);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetConsulta", new { id_consulta = consulta.Id_consulta }, consulta);
            }
            else 
            {
                return BadRequest("Ids não encontrado");
            }
        }
        catch(Exception ex)
        {
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }

    }

    // DELETE: api/Consulta/5

    /// <summary>
    /// Deletar consulta
    /// </summary>
    /// <param name="id_consulta">id para buscar</param>
    /// <response code="204">consulta removido com sucesso.</response>
    /// <response code="404">consulta não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>deletar consulta</returns>
    [HttpDelete]
    [Route("deleta/consulta/{id_consulta:int}")]
    public async Task<IActionResult> DeleteConsulta(int id_consulta)
    {
        try
        {
            var consulta = await _context.Consultas.FindAsync(id_consulta);
            if (consulta == null)
            {
                return NotFound("Consulta não encontrado.");
            }

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
    }

    
}
