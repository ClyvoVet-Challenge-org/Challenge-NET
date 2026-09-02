using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.src.Models;
using challengeFiap.src.Data;

[Route("api/[controller]")]
[ApiController]
public class TutorController : ControllerBase
{
    private readonly AppDbContext _context;
    public TutorController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Tutor

    /// <summary>
    /// Relatorio Tutor
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/Tutor")]
    public async Task<ActionResult<IEnumerable<Tutor>>> GetAllTutor()
    {
        var TutorRelatorio = await _context.Tutor.ToListAsync();
        return Ok(TutorRelatorio);
    }

    // GET: api/Tutor/5

    /// <summary>
    /// Relatorio de Tutor feito pelo id
    /// </summary>
    /// <param name="id_Tutor">Buscar pelo id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/Tutor/{id_Tutor:int}")]
    public async Task<ActionResult<Tutor>> GetTutor(int id_Tutor)
    {
        try
        {
            var Tutor = await _context.Tutor.FindAsync(id_Tutor);

            if (Tutor == null)
            {
                return NotFound("Id Tutor não encontrado");
            }

            return Ok(Tutor);
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
        
    }

    // PUT: api/Tutor/5

    /// <summary>
    /// Atualizar dados reponsavel
    /// </summary>
    /// <param name="id_Tutor">Id para pode atualizar</param>
    /// <param name="Tutor">Dados para serem inseridos</param>
    ///  <response code="204">Tutor atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Tutor não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/Tutor/{id_Tutor:int}")]
    public async Task<IActionResult> PutTutor(int id_Tutor, Tutor Tutor)
    {
        if (id_Tutor != Tutor.Id_tutor)
        {
            return BadRequest("Id Tutor está incorreto");
        }

        try
        {
            var cpfExiste = await _context.Tutor
                .FirstOrDefaultAsync(
                c => c.Cpf_tutor == Tutor.Cpf_tutor && c.Id_tutor != id_Tutor);
            if (cpfExiste != null)
            {
                return BadRequest("Cpf já esta sendo utilizando");
            }
            _context.Entry(Tutor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TutorExists(id_Tutor))
            {
                return NotFound("Id Tutor nao achando");
            }
            else
            {
                throw;
            }
        }catch(Exception ex)
        {
            return BadRequest($"Erro em atualizar Tutor: {ex.Message}");
        }
    }
    private bool TutorExists(int id_Tutor)
    {
        return _context.Tutor.FirstOrDefault(e => e.Id_tutor == id_Tutor) != null;
    }

    // POST: api/Tutor

    /// <summary>
    /// Criar Tutor
    /// </summary>
    /// <param name="Tutor">Criação de dados de endereço Tutor </param>
    /// <response code="201">Tutor criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação Tutor</returns>
    [HttpPost]
    [Route("criar/Tutor")]
    public async Task<ActionResult<Tutor>> PostTutor(Tutor Tutor)
    {
        try
        {
            var cpfExiste = await 
                _context.Tutor
                .FirstOrDefaultAsync(a => a.Cpf_tutor == Tutor.Cpf_tutor);
            if (cpfExiste == null)
            {
                _context.Tutor.Add(Tutor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTutor", new { id_Tutor = Tutor.Id_tutor }, Tutor);
            }else
            {
                return BadRequest("Cpf já existente");
            }
        }catch(Exception ex)
        {
            return BadRequest($"Erro encontrado: {ex.Message}");
        }
    }

    // DELETE: api/Tutor/5

    /// <summary>
    /// Remove dados de Tutor
    /// </summary>
    /// <param name="id_Tutor">Id para pode remover: </param>
    /// <response code="204">Tutor removido com sucesso.</response>
    /// <response code="404">Tutor não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletado: </returns>
    [HttpDelete]
    [Route("deleta/Tutor/{id_Tutor:int}")]
    public async Task<IActionResult> DeleteTutor(int id_Tutor)
    {
        try
        {
            var Tutor = await _context.Tutor.FindAsync(id_Tutor);
            if (Tutor == null)
            {
                return NotFound("Id nao encontrado");
            }

            _context.Tutor.Remove(Tutor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
    }

 
}
