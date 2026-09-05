using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;

[Route("api/[controller]")]
[ApiController]
public class ClinicasController : ControllerBase
{
    private readonly AppDbContext _context;
    public ClinicasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Clinica

    /// <summary>
    /// Relatorio clinica.
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio clinica</returns>
    [HttpGet]
    [Route("relatorio/clinica")]
    public async Task<ActionResult<IEnumerable<Clinica>>> GetAllClinica()
    {
        var relatorioClinica = await _context.Clinicas.ToListAsync();
        return Ok(relatorioClinica);
    }

    // GET: api/Clinica/5

    /// <summary>
    /// Relatorio de clinica feito pelo id.
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <param name="id_clinica">Id de buscar da clinica</param>
    /// <returns>Relatorio clinica</returns>
    [HttpGet]
    [Route("relatorio/clinica/{id_clinica:int}")]
    public async Task<ActionResult<Clinica>> GetClinica(int id_clinica)
    {
        try
        {
            var clinica = await _context.Clinicas.FindAsync(id_clinica);
            if(clinica == null)
            {
                return NotFound("Id clinica não encontrado");
            }
            return Ok(clinica);

        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em processar a buscar pela clinica: {ex.Message}");
        }
    }

    // PUT: api/Clinica/5

    /// <summary>
    /// Atualizar dados clinica
    /// </summary>
    /// <param name="id_clinica">Id clinica para a url</param>
    /// <param name="clinica">Novos dados a clinica</param>
    /// <response code="204">clinica atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">clinica não encontrado</response>
    /// <returns>Atualização clinica</returns>
    [HttpPut]
    [Route("atualizar/clinica/{id_clinica:int}")]
    public async Task<IActionResult> PutClinica(int id_clinica, Clinica clinica)
    {
        if (id_clinica != clinica.Id_clinica)
        {
            return BadRequest("O id da clinica esta incorreto");
        }

        try
        {
            _context.Entry(clinica).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClinicaExists(id_clinica))
            {
                return NotFound("A clinica não encontrada");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em atualizar clinica: {ex.Message}");

        }
    }
    private bool ClinicaExists(int id_clinica)
    {
        return _context.Clinicas.FirstOrDefault(e => e.Id_clinica == id_clinica) != null;
    }

    // POST: api/Clinica

    /// <summary>
    /// Criação clinica
    /// </summary>
    /// <param name="clinica">Inserir dados</param>
    /// <response code="201">clinica criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criado sucesso</returns>
    [HttpPost]
    [Route("criar/clinica")]
    public async Task<ActionResult<Clinica>> PostClinica(Clinica clinica)
    {
        try
        {
            var existecpnj = await _context.Clinicas
                .FirstOrDefaultAsync(c => c.Cnpj_clinica == clinica.Cnpj_clinica);

            if (existecpnj !=null)
            {
                return BadRequest("O cnpj já existe");
            }
            _context.Clinicas.Add(clinica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClinica", new { id_clinica = clinica.Id_clinica }, clinica);
        }
        catch(Exception ex) 
        {
        
            return BadRequest($"Erro em salvar os dados: {ex.Message}");
        }
        
    }

    // DELETE: api/Clinica/5

    /// <summary>
    /// Remove dados de um clinica pelo id
    /// </summary>
    /// <param name="id_clinica">Id clinica para ser buscado</param>
    /// <response code="204">clinica removido com sucesso.</response>
    /// <response code="404">Animal não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns></returns>
    [HttpDelete]
    [Route("deleta/clinica/{id_clinica:int}")]
    public async Task<IActionResult> DeleteClinica(int id_clinica)
    {
        try
        {
            var clinica = await _context.Clinicas.FirstOrDefaultAsync(e => e.Id_clinica == id_clinica);
            if (clinica == null)
            {
                return NotFound("Clinica não encontrada");
            }
            _context.Clinicas.Remove(clinica);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
    }
}
