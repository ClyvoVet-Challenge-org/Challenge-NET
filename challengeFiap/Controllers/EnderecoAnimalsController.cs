using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.src.Models;
using challengeFiap.src.Data;

[Route("api/[controller]")]
[ApiController]
public class EnderecoAnimalsController : ControllerBase
{
    private readonly AppDbContext _context;
    public EnderecoAnimalsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/EnderecoAnimal

    /// <summary>
    /// Relatorio de dados endereço animal
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio endereço animal</returns>
    [HttpGet]
    [Route("relatorio/enderecoanimal")]
    public async Task<ActionResult<IEnumerable<EnderecoAnimal>>> GetAllEnderecoAnimal()
    {
        var relatorioEnAnimal = await _context.EnderecoAnimals.ToListAsync();
        return Ok(relatorioEnAnimal);
    }

    // GET: api/EnderecoAnimal/5

    /// <summary>
    /// Relatorio de endereco animal feito pelo id
    /// </summary>
    /// <param name="id_endereco_animal">Buscar pelo id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/enderecoanimal/{id_endereco_animal:int}")]
    public async Task<ActionResult<EnderecoAnimal>> GetEnderecoAnimal(int id_endereco_animal)
    {
        try
        {
            var enderecoanimal = await _context.EnderecoAnimals.FindAsync(id_endereco_animal);

            if (enderecoanimal == null)
            {
                return NotFound("Endereço animal não encontrado.");
            }

            return Ok(enderecoanimal);
        }
        catch(Exception ex)
        {
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
    }

    // PUT: api/EnderecoAnimal/5

    /// <summary>
    /// Atualizar dados endereço animal
    /// </summary>
    /// <param name="id_endereco_animal">Id para pode atualizar</param>
    /// <param name="enderecoanimal">dados para ser inseridos</param>
    /// <response code="204">Endereço animal atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Endereço animal não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/enderecoanimal/{id_endereco_animal:int}")]
    public async Task<IActionResult> PutEnderecoAnimal(int id_endereco_animal, EnderecoAnimal enderecoanimal)
    {
        if (id_endereco_animal != enderecoanimal.Id_endereco_animal)
        {
            return BadRequest("Id endereco animal está incorreto");
        }
        try
        {
            _context.Entry(enderecoanimal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnderecoAnimalExists(id_endereco_animal))
            {
                return NotFound("Id endereço animal não encontrado");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex) {
            return BadRequest($"Erro em atualizar endereço animal: {ex.Message} ");
        }
    }
    private bool EnderecoAnimalExists(int id_endereco_animal)
    {
        return _context.EnderecoAnimals.FirstOrDefault(e => e.Id_endereco_animal == id_endereco_animal) != null;
    }

    // POST: api/EnderecoAnimal

    /// <summary>
    /// Criar endereço animal
    /// </summary>
    /// <param name="enderecoanimal">Criação de dados de endereço animal</param>
    /// <response code="201">Endereço animal criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criar endereço animal</returns>
    [HttpPost]
    [Route("criar/enderecoanimal")]
    public async Task<ActionResult<EnderecoAnimal>> PostEnderecoAnimal(EnderecoAnimal enderecoanimal)
    {
        try
        {
            var animalExiste = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id_animal == enderecoanimal.Id_animal);

            if (animalExiste != null)
            {
                _context.EnderecoAnimals.Add(enderecoanimal);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetEnderecoAnimal", new { id_endereco_animal = enderecoanimal.Id_endereco_animal }, enderecoanimal);
            }
            else
            {
                return BadRequest($"Id animal não existe");
            }
        }
        catch(Exception ex)
        {
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }
    }

    // DELETE: api/EnderecoAnimal/5

    /// <summary>
    /// Remove dados do endereço animal
    /// </summary>
    /// <param name="id_endereco_animal">Id para pode remover: </param>
    /// <response code="204">Endereco animal removido com sucesso.</response>
    /// <response code="404">Endereco animal não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>deletado: </returns>
    [HttpDelete]
    [Route("deleta/enderecoanimal/{id_endereco_animal:int}")]
    public async Task<IActionResult> DeleteEnderecoAnimal(int id_endereco_animal)
    {
        try
        {
            var enderecoanimal = await _context.EnderecoAnimals.FindAsync(id_endereco_animal);
            if (enderecoanimal == null)
            {
                return NotFound("Id não encontrado");
            }

            _context.EnderecoAnimals.Remove(enderecoanimal);
            await _context.SaveChangesAsync();

            return NoContent();
        }catch(Exception ex)
        {
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
        
    }

}
