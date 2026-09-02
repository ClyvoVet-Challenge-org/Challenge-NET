using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.src.Models;
using challengeFiap.src.Data;

[Route("api/[controller]")]
[ApiController]
public class VeterinariosController : ControllerBase
{
    private readonly AppDbContext _context;
    public VeterinariosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Veterinario

    /// <summary>
    /// Relatorio Veterinario
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/veterinario")]
    public async Task<ActionResult<IEnumerable<Veterinario>>> GetAllVeterinario()
    {
        var relatorioVeterinario = await _context.Veterinarios.ToListAsync();
        return Ok(relatorioVeterinario);
    }

    // GET: api/Veterinario/5

    /// <summary>
    /// Relatorio de Veterinario com o id: 
    /// </summary>
    /// <param name="id_vet">Buscar pelo id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/veterinario/{id_vet:int}")]
    public async Task<ActionResult<Veterinario>> GetVeterinario(int id_vet)
    {
        try
        {
            var veterinario = await _context.Veterinarios.FindAsync(id_vet);

            if (veterinario == null)
            {
                return NotFound("Id não encontrado.");
            }

            return Ok(veterinario);
        }
        catch (Exception ex) {
            return BadRequest($"Erro em buscar pelo id: {ex.Message}");
        }
    }

    // PUT: api/Veterinario/5

    /// <summary>
    /// Atualizar dados Veterinarios
    /// </summary>
    /// <param name="id_vet">Id para pode atualizar: </param>
    /// <param name="veterinario">Dados para serem inseridos: </param>
    /// <returns>atualizar: </returns>
    [HttpPut]
    [Route("atualizar/veterinario/{id_vet:int}")]
    public async Task<IActionResult> PutVeterinario(int id_vet, Veterinario veterinario)
    {
        if (id_vet != veterinario.Id_vet)
        {
            return BadRequest("Id veterianario esta incorreto.");
        }

        try
        {
            var cpfVet = await _context.Veterinarios.FirstOrDefaultAsync(c => c.Cpf_vet == veterinario.Cpf_vet && c.Id_vet != id_vet);
            var crmvVet = await _context.Veterinarios.FirstOrDefaultAsync(cr => cr.Crmv_vet == veterinario.Crmv_vet && cr.Id_vet != id_vet);
            var email = await _context.Veterinarios.FirstOrDefaultAsync(e => e.Email_vet == veterinario.Email_vet && e.Id_vet != id_vet);

            if (cpfVet!=null || crmvVet!=null || email!=null)
            {
                return BadRequest("Não foi possivel de cadastras: cpf, crmv ou email ja estão sento utilizando");
            }
            _context.Entry(veterinario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VeterinarioExists(id_vet))
            {
                return NotFound("Id verterinario não encontrado");
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
    private bool VeterinarioExists(int id_vet)
    {
        return _context.Veterinarios.FirstOrDefault(e => e.Id_vet == id_vet) != null;
    }

    // POST: api/Veterinario

    /// <summary>
    /// Criar Veterinarios
    /// </summary>
    /// <param name="veterinario">Criação de dados de veterianos</param>
    /// <response code="201">Veterinarios criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação de veterinarios: </returns>
    [HttpPost]
    [Route("criar/veterinario")]
    public async Task<ActionResult<Veterinario>> PostVeterinario(Veterinario veterinario)
    {
        try
        {
            var cpfVet = await _context.Veterinarios.FirstOrDefaultAsync(c => c.Cpf_vet == veterinario.Cpf_vet);
            var crmvVet = await _context.Veterinarios.FirstOrDefaultAsync(cr => cr.Crmv_vet == veterinario.Crmv_vet);
            var email = await _context.Veterinarios.FirstOrDefaultAsync(e => e.Email_vet == veterinario.Email_vet);

            if (cpfVet != null || crmvVet != null || email != null)
            {
                return BadRequest("Ja existe email, cpf ou crmv existente");
            }
            else
            {
                _context.Veterinarios.Add(veterinario);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVeterinario", new { id_vet = veterinario.Id_vet }, veterinario);
            }
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em salvar os dados: {ex.Message}");
        }
    }

    // DELETE: api/Veterinario/5
    /// <summary>
    /// Remove dados de veterinarios
    /// </summary>
    /// <param name="id_vet"></param>
    /// <response code="204">Veterinario removido com sucesso.</response>
    /// <response code="404">Veterinario não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletado: </returns>
    [HttpDelete]
    [Route("deleta/veterinario/{id_vet:int}")]
    public async Task<IActionResult> DeleteVeterinario(int id_vet)
    {
        try
        {
            var veterinario = await _context.Veterinarios.FindAsync(id_vet);
            if (veterinario == null)
            {
                return NotFound("Id não encontrado de vet");
            }

            _context.Veterinarios.Remove(veterinario);
            await _context.SaveChangesAsync();

            return NoContent();

        }
        catch(Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
    }
}
