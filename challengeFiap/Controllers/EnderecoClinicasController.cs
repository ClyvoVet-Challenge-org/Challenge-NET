using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;

[Route("api/[controller]")]
[ApiController]
public class EnderecoClinicasController : ControllerBase
{
    private readonly AppDbContext _context;
    public EnderecoClinicasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/EnderecoClinica

    /// <summary>
    /// Relatorio de dados endereço clinica
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio endereço clinica</returns>
    [HttpGet]
    [Route("relatorio/enderecoclinica")]
    public async Task<ActionResult<IEnumerable<EnderecoClinica>>> GetAllEnderecoClinica()
    {
        var relatorioEnClinica = await _context.EnderecoClinicas.ToListAsync();
        return Ok(relatorioEnClinica);
    }

    // GET: api/EnderecoClinica/5

    /// <summary>
    /// Relatorio de endereco clinica pelo id
    /// </summary>
    /// <param name="id_endereco_clinica">Buscar pelo id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/enderecoclinica/{id_endereco_clinica:int}")]
    public async Task<ActionResult<EnderecoClinica>> GetEnderecoClinica(int id_endereco_clinica)
    {
        try
        {
            var enderecoclinica = await _context.EnderecoClinicas.FindAsync(id_endereco_clinica);

            if (enderecoclinica == null)
            {
                return NotFound("Id não encontrado.");
            }

            return Ok(enderecoclinica);
        }
        catch (Exception ex) {
            return BadRequest($"Erro achado: {ex.Message}");
        }
        
    }

    // PUT: api/EnderecoClinica/5

    /// <summary>
    /// Atualizar dados endereço endereço
    /// </summary>
    /// <param name="id_endereco_clinica">Id para pode atualizar</param>
    /// <param name="enderecoclinica">dados para ser inseridos</param>
    /// <response code="204">Endereço clinica atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Endereço clinica não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/enderecoclinica/{id_endereco_clinica:int}")]
    public async Task<IActionResult> PutEnderecoClinica(int id_endereco_clinica, EnderecoClinica enderecoclinica)
    {
        if (id_endereco_clinica != enderecoclinica.Id_endereco_clinica)
        {
            return BadRequest("O id endereço clinica esta errado");
        }
        try
        {
            _context.Entry(enderecoclinica).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();

        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnderecoClinicaExists(id_endereco_clinica))
            {
                return NotFound("Id não encontrado");
            }
            else
            {
                throw;
            }
        }catch(Exception ex)
        {
            return BadRequest($"Erro na atualização: {ex.Message}");
        }
    }
    private bool EnderecoClinicaExists(int id_endereco_clinica)
    {
        return _context.EnderecoClinicas.FirstOrDefault(e => e.Id_endereco_clinica == id_endereco_clinica) != null;
    }

    // POST: api/EnderecoClinica

    /// <summary>
    /// Criar endereço clinica
    /// </summary>
    /// <param name="enderecoclinica">Dados para serem inseridos</param>
    /// <response code="201">Endereço clinica criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação de endereço clinica</returns>
    [HttpPost]
    [Route("criar/enderecoclinica")]
    public async Task<ActionResult<EnderecoClinica>> PostEnderecoClinica(EnderecoClinica enderecoclinica)
    {
        try
        {
            var clinicaexiste = await _context.Clinicas
                .FirstOrDefaultAsync(a => a.Id_clinica == enderecoclinica.Id_clinica);
            if (clinicaexiste != null)
            {
                _context.EnderecoClinicas.Add(enderecoclinica);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEnderecoClinica", new { id_endereco_clinica = enderecoclinica.Id_endereco_clinica }, enderecoclinica);
            }
            else
            {
                return BadRequest($"Id não existe");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }

    }

    // DELETE: api/EnderecoClinica/5

    /// <summary>
    /// Remove daods do endereço clinica
    /// </summary>
    /// <param name="id_endereco_clinica">Id para pode remover: </param>
    /// <response code="204">Endereco clinica removido com sucesso.</response>
    /// <response code="404">Endereco clinica não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletado: </returns>
    [HttpDelete]
    [Route("deleta/enderecoclinica/{id_endereco_clinica:int}")]
    public async Task<IActionResult> DeleteEnderecoClinica(int id_endereco_clinica)
    {
        try
        {
            var enderecoclinica = await _context.EnderecoClinicas.FindAsync(id_endereco_clinica);
            if (enderecoclinica == null)
            {
                return NotFound("Id clinica endereço não encontrado");
            }

            _context.EnderecoClinicas.Remove(enderecoclinica);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }

    }

    
}
