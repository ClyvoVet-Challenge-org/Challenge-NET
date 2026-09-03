using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.src.Data;
using Microsoft.AspNetCore.Http.HttpResults;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly AppDbContext _context;
    public AnimalsController(AppDbContext context)
    {
        _context = context;
    }


    // GET: api/Animal

    /// <summary>
    /// Carrgea todos os animais presente no banco
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Lista de animal</returns>
    [HttpGet]
    [Route("relatorio/animal")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAllAnimal()
    { 
        var relatorioAnimal = await _context.Animals.ToListAsync();
        return Ok(relatorioAnimal);
    }

    // GET: api/Animal/5

    /// <summary>
    /// Carregar o relatorio animal por meio do id
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <param name="id_animal">id do animal</param>
    /// <returns>Id encontrado</returns>
    [HttpGet]
    [Route("relatorio/animal/{id_animal:int}")]
    public async Task<ActionResult<Animal>> GetAnimal(int id_animal)
    {
        try
        {
            var animal = await _context.Animals.FindAsync(id_animal);
            if (animal == null)
            {
                return NotFound("Id Animal não encontrada");
            }
            return Ok(animal);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro em processar a buscar: {ex.Message}");
        }
        
    }

    // PUT: api/Animal/5

    /// <summary>
    /// Atualizar dados de animais
    /// </summary>
    /// <param name="id_animal">Id animal na url</param>
    /// <param name="animal">Novos dados de animal</param>
    /// <response code="204">Animal atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Animal não encontrado</response>
    /// <returns>Atualizado</returns>
    [HttpPut]
    [Route("atualizar/animal/{id_animal:int}")]
    public async Task<IActionResult> PutAnimal(int id_animal, Animal animal)
    { 
        if(id_animal != animal.Id_animal)
        {
            return BadRequest("O id de animal esta incorreto");
        }

        try
        {
            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AnimalExists(id_animal))
            {
                return NotFound("O animal não encontrado");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex) {
            return BadRequest($"Erro em atualizar animal: {ex.Message}");
        }
    }
    private bool AnimalExists(int id_animal)
    {
        return _context.Animals.FirstOrDefault(e => e.Id_animal == id_animal) != null;
    }

    //Criar 
    // POST: api/Animal

    /// <summary>
    /// Criacao de animal
    /// </summary>
    /// <param name="animal">Criacao de novos dados para animal</param>
    /// <response code="201">Animal criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação feita</returns>
    [HttpPost]
    [Route("criar/animal")]
    public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
    {
        try
        {
            var responsavelExistente = await _context.Tutor
                .FirstOrDefaultAsync(a => a.Id_tutor== animal.Id_tutor);
            
            if(responsavelExistente != null)
            {
                _context.Animals.Add(animal);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAnimal), new { id_animal = animal.Id_animal}, animal);
            }
            else
            {
                return BadRequest("Id responsavel não encontrado");
            }
        }
        catch (Exception e) {
            return BadRequest($"Erro ao salvar os dados: {e.Message}"); 
        }
            
    }


    // DELETE: api/Animal/5

    /// <summary>
    /// Remove dados de um animal do sistema pelo seu id
    /// </summary>
    /// <param name="id_animal">Id do animal para ser deletado</param>
    /// <response code="204">Animal removido com sucesso.</response>
    /// <response code="404">Animal não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>deletado</returns>
    [HttpDelete]
    [Route("deleta/animal/{id_animal:int}")]
    public async Task<IActionResult> DeleteAnimal(int id_animal)
    {
        try
        {
            var animalExistente = await _context.Animals
                .FirstOrDefaultAsync(e => e.Id_animal == id_animal);
            if (animalExistente == null)
            {
                return NotFound("Animal não encontrado.");
            }

            _context.Animals.Remove(animalExistente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Erro em deletar: {e.Message}");
        }
    }

    
}
