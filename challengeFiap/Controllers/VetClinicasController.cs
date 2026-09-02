using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.src.Models;
using challengeFiap.src.Data;

[Route("api/[controller]")]
[ApiController]
public class VetClinicasController : ControllerBase
{
    private readonly AppDbContext _context;
    public VetClinicasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/VetClinica

    /// <summary>
    /// Relatorio de dados de Vet e clinica
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/vetclinica")]
    public async Task<ActionResult<IEnumerable<VetClinica>>> GetAllVetClinica()
    {
        var clinicaVet = await _context.VetClinicas.ToListAsync();
        return Ok(clinicaVet);
    }

    // GET: api/VetClinica/5

    /// <summary>
    /// Relatorio de Vet Clinica
    /// </summary>
    /// <param name="id_clinica_vet">Buscar id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/vetclinica/{id_clinica_vet:int}")]
    public async Task<ActionResult<VetClinica>> GetVetClinica(int id_clinica_vet)
    {
        try
        {
            var vetclinica = await _context.VetClinicas.FindAsync(id_clinica_vet);

            if (vetclinica == null)
            {
                return NotFound("Id vet clinica não encontrado");
            }

            return Ok(vetclinica);
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
        
    }

    // PUT: api/VetClinica/5

    /// <summary>
    /// Atualizar dados de vet clinica
    /// </summary>
    /// <param name="id_clinica_vet">Para inserir o id para pode atualizar: </param>
    /// <param name="vetclinica">dados para serem inseridos</param>
    /// <response code="204">Vet Clinica atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Vet Clinica não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/vetclinica/{id_clinica_vet:int}")]
    public async Task<IActionResult> PutVetClinica(int id_clinica_vet, VetClinica vetclinica)
    {
        if (id_clinica_vet != vetclinica.Id_clinica_vet)
        {
            return BadRequest("Id clinica e vet está incorretor");
        }

        try
        {
            var existeClinica = await _context.Clinicas
                .FirstOrDefaultAsync(c => c.Id_clinica == vetclinica.Id_clinica);
            var existeVet = await _context.Veterinarios
                .FirstOrDefaultAsync(c => c.Id_vet == vetclinica.Id_vet);
            if (existeClinica != null && existeVet != null)
            {
                _context.Entry(vetclinica).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return NotFound("Não foi possivel encontrar os id clinica ou vet.");
            }

        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VetClinicaExists(id_clinica_vet))
            {
                return NotFound("Id clinica vet não existe");
            }
            else
            {
                throw;
            }
        }catch (Exception ex)
        {
            return BadRequest($"Erro em atualizar: {ex.Message}");
        }
    }
    private bool VetClinicaExists(int id_clinica_vet)
    {
        return _context.VetClinicas.FirstOrDefault(e => e.Id_clinica_vet == id_clinica_vet) != null;
    }

    // POST: api/VetClinica
    /// <summary>
    /// Inserir Vet e clinica
    /// </summary>
    /// <param name="vetclinica">Inserir os dados de vet e clinica: </param>
    /// <response code="201">Vet Clinica criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação: </returns>
    [HttpPost]
    [Route("criar/vetclinica")]
    public async Task<ActionResult<VetClinica>> PostVetClinica(VetClinica vetclinica)
    {
        try
        {
            var exiteClinica = await _context.Clinicas.FirstOrDefaultAsync(c => c.Id_clinica == vetclinica.Id_clinica);
            var existeVet = await _context.Veterinarios.FirstOrDefaultAsync(v => v.Id_vet == vetclinica.Id_vet);
            if (exiteClinica != null && existeVet != null)
            {
                _context.VetClinicas.Add(vetclinica);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVetClinica", new { id_clinica_vet = vetclinica.Id_clinica_vet }, vetclinica);
            }
            else
            {
                return BadRequest("Id clinica e vet não encontrado");
            }
        }
        catch(Exception ex)
        {
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }
       
    }

    // DELETE: api/VetClinica/5

    /// <summary>
    /// Remove dados de vet clinica
    /// </summary>
    /// <param name="id_clinica_vet">Inserir o id para pode deletar: </param>
    /// <response code="204">Vet clinica removido com sucesso.</response>
    /// <response code="404">Vet clinica não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletar: </returns>
    [HttpDelete]
    [Route("deleta/vetclinica/{id_clinica_vet:int}")]
    public async Task<IActionResult> DeleteVetClinica(int id_clinica_vet)
    {
        try
        {
            var vetclinica = await _context.VetClinicas.FindAsync(id_clinica_vet);
            if (vetclinica == null)
            {
                return NotFound("Id vet clinica não encontrado");
            }
            _context.VetClinicas.Remove(vetclinica);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
    }
}
