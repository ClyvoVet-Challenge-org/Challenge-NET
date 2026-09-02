using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.src.Models;
using challengeFiap.src.Data;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

[Route("api/[controller]")]
[ApiController]
public class MedicamentoesController : ControllerBase
{
    private readonly AppDbContext _context;
    public MedicamentoesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Medicamento

    /// <summary>
    /// Relatorio de medicamento: 
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>relatorio: </returns>
    [HttpGet]
    [Route("relatorio/medicamento")]

    public async Task<ActionResult<IEnumerable<Medicamento>>> GetAllMedicamento()
    {
        var relatorioMedicamento = await _context.Medicamentos.ToArrayAsync();
        return Ok(relatorioMedicamento);
    }

    // GET: api/Medicamento/5

    /// <summary>
    /// Relatorio medicamento pelo id
    /// </summary>
    /// <param name="id_medicamento">Buscar pelo id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio</returns>
    [HttpGet]
    [Route("relatorio/medicamento/{id_medicamento:int}")]
    public async Task<ActionResult<Medicamento>> GetMedicamento(int id_medicamento)
    {
        try
        {
            var medicamento = await _context.Medicamentos.FindAsync(id_medicamento);

            if (medicamento == null)
            {
                return NotFound("Id não encontrado");
            }

            return Ok(medicamento);
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
        
    }

    // PUT: api/Medicamento/5

    /// <summary>
    /// Atualizar medicamento
    /// </summary>
    /// <param name="id_medicamento">Id para pode atualizar: </param>
    /// <param name="medicamento">Dados para serem inseridos</param>
    /// <response code="204">Medicamento atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Medicamento não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/medicamento/{id_medicamento:int}")]
    public async Task<IActionResult> PutMedicamento(int id_medicamento, Medicamento medicamento)
    {
        if (id_medicamento != medicamento.Id_medicamento)
        {
            return BadRequest("Id medicamento não está incorreto");
        }
        try
        {
            _context.Entry(medicamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MedicamentoExists(id_medicamento))
            {
                return NotFound("Medicamento não encontrado");
            }
            else
            {
                throw;
            }
        }catch(Exception ex)
        {
            return BadRequest($"Erro em atualizar medicamento: {ex.Message}");
        }
    }
    private bool MedicamentoExists(int id_medicamento)
    {
        return _context.Medicamentos.FirstOrDefault(e => e.Id_medicamento == id_medicamento) != null;
    }

    // POST: api/Medicamento

    /// <summary>
    /// Criar dado de medicamento
    /// </summary>
    /// <param name="medicamento">Inserir os dados de medicamento </param>
    ///  <response code="201">Medicamento criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação medicamento: </returns>
    [HttpPost]
    [Route("criar/medicamento")]
    public async Task<ActionResult<Medicamento>> PostMedicamento(Medicamento medicamento)
    {
        try
        {
            var prescricaoId = await _context.Prescricaos
                .FirstOrDefaultAsync(m => m.Id_prescricao == medicamento.Id_prescricao);
            if (prescricaoId != null)
            {
                _context.Medicamentos.Add(medicamento);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetMedicamento", new { id_medicamento = medicamento.Id_medicamento }, medicamento);
            }
            else
            {
                return BadRequest($"Id prescricao não existe");
            }
        }catch(Exception ex)
        {
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }
        
    }

    // DELETE: api/Medicamento/5

    /// <summary>
    /// Remove dados de medicamento
    /// </summary>
    /// <param name="id_medicamento">Id para pode remover: </param>
    /// <response code="204">Medicamento removido com sucesso.</response>
    /// <response code="404">Medicamento não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletar: </returns>
    [HttpDelete]
    [Route("deleta/medicamento/{id_medicamento:int}")]
    public async Task<IActionResult> DeleteMedicamento(int id_medicamento)
    {
        try
        {
            var medicamento = await _context.Medicamentos.FindAsync(id_medicamento);
            if (medicamento == null)
            {
                return NotFound("Id não encontrado.");
            }

            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
        
    }
 
}
