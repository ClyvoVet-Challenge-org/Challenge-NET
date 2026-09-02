using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.src.Models;
using challengeFiap.src.Data;

[Route("api/[controller]")]
[ApiController]
public class EnderecoResponsavelsController : ControllerBase
{
    private readonly AppDbContext _context;
    public EnderecoResponsavelsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/EnderecoResponsavel

    /// <summary>
    /// Relaorio de dados endereço responsavel
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio endereço responsavel</returns>
    [HttpGet]
    [Route("relatorio/enderecoresponsavel")]
    public async Task<ActionResult<IEnumerable<EnderecoTutor>>> GetAllEnderecoResponsavel()
    {
        var relatorioEnResponsavel = await _context.EnderecoTutors.ToListAsync();
        return Ok(relatorioEnResponsavel);
    }

    // GET: api/EnderecoResponsavel/5

    /// <summary>
    /// Relatorio de endereço responsavel pelo id
    /// </summary>
    /// <param name="id_endereco_responsavel"></param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/enderecoresponsavel/{id_endereco_responsavel:int}")]
    public async Task<ActionResult<EnderecoTutor>> GetEnderecoResponsavel(int id_endereco_responsavel)
    {
        try
        {
            var enderecoresponsavel = await _context.EnderecoTutors.FindAsync(id_endereco_responsavel);

            if (enderecoresponsavel == null)
            {
                return NotFound("Id não encontrado");
            }

            return Ok(enderecoresponsavel);
        }
        catch (Exception ex) 
        {
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
        
    }

    // PUT: api/EnderecoResponsavel/5

    /// <summary>
    /// Atualizar dados endereço responsavel
    /// </summary>
    /// <param name="id_endereco_responsavel">Id para pode atualizar</param>
    /// <param name="enderecoresponsavel">Dados para ser inseridos</param>
    /// <response code="204">Endereço responsavel atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Endereço responsavel não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/enderecoresponsavel/{id_endereco_responsavel:int}")]
    public async Task<IActionResult> PutEnderecoResponsavel(int id_endereco_responsavel, EnderecoTutor enderecoresponsavel)
    {
        if (id_endereco_responsavel != enderecoresponsavel.Id_endereco_tutor)
        {
            return BadRequest("Id endereco responsal esta incorreto");
        }

        try
        {
            _context.Entry(enderecoresponsavel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnderecoResponsavelExists(id_endereco_responsavel))
            {
                return NotFound("Endereço responsavel não encontrado");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro encontrado: {ex.Message}");
        }

    }
    private bool EnderecoResponsavelExists(int id_endereco_responsavel)
    {
        return _context.EnderecoTutors.FirstOrDefault(e => e.Id_endereco_tutor == id_endereco_responsavel) != null;
    }

    // POST: api/EnderecoResponsavel

    /// <summary>
    /// Criar endereço responsavel
    /// </summary>
    /// <param name="enderecoresponsavel">Criação de dados de endereço responsavel</param>
    /// <response code="201">Endereço reponsavel criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criar endereço responsavel</returns>
    [HttpPost]
    [Route("criar/enderecoresponsavel")]
    public async Task<ActionResult<EnderecoTutor>> PostEnderecoResponsavel(EnderecoTutor enderecoresponsavel)
    {
        try
        {
            var responsavelExiste = await _context.Tutor
                .FirstOrDefaultAsync(a => a.Id_tutor == enderecoresponsavel.Id_tutor);
            if (responsavelExiste != null)
            {
                _context.EnderecoTutors.Add(enderecoresponsavel);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetEnderecoResponsavel", new { id_endereco_responsavel = enderecoresponsavel.Id_endereco_tutor }, enderecoresponsavel);
            }
            else
            {
                return BadRequest($"ID resonsavel não encontrado");
            }
        }catch(Exception ex)
        {
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }
        
    }

    // DELETE: api/EnderecoResponsavel/5

    /// <summary>
    /// Remove dados de endereço responsavel
    /// </summary>
    /// <param name="id_endereco_responsavel">Id para pode remover: </param>
    /// <response code="204">Endereco responsavel removido com sucesso.</response>
    /// <response code="404">Endereco responsavel não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletado: </returns>
    [HttpDelete]
    [Route("deleta/enderecoresponsavel/{id_endereco_responsavel:int}")]
    public async Task<IActionResult> DeleteEnderecoResponsavel(int id_endereco_responsavel)
    {
        try
        {
            var enderecoresponsavel = await _context.EnderecoTutors.FindAsync(id_endereco_responsavel);
            if (enderecoresponsavel == null)
            {
                return NotFound("Id não encontrado");
            }

            _context.EnderecoTutors.Remove(enderecoresponsavel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
        
    }

    
}
