using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EnderecoAnimalsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<EnderecoAnimalsController> _logger;
    private readonly IenderecoAnimalService _enderecoAnimalService;

    public EnderecoAnimalsController(
        AppDbContext context,
        ILogger<EnderecoAnimalsController> logger,
        IenderecoAnimalService enderecoAnimalService)
    {
        _context = context;
        _logger = logger;
        _enderecoAnimalService = enderecoAnimalService;
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
        _logger.LogInformation("Iniciando a busca de endereço animal");

        try
        {
            var relatorioEnAnimal = await _context.EnderecoAnimals.ToListAsync();

            _logger.LogInformation("Busca de endereço animal concluída com sucesso");

            return Ok(relatorioEnAnimal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca a endereço animal");

            return BadRequest($"Erro em processar a busca: {ex.Message}");
        }
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
        _logger.LogInformation("Iniciando a busca de endereço animal");

        try
        {
             var enderecoanimal = await _context.EnderecoAnimals.FindAsync(id_endereco_animal);

            if (enderecoanimal == null)
            {
                _logger.LogWarning("Endereço animal não encontrou o id esta vazio");

                return NotFound("Endereço animal não encontrado.");
            }
            
            _logger.LogInformation("Endereço animal encontrado com sucesso.");

            return Ok(enderecoanimal);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Erro em buscar endereço animal.");

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
    public async Task<IActionResult> PutEnderecoAnimal(int id_endereco_animal,EnderecoAnimal enderecoanimal)
    {
        _logger.LogInformation("Iniciando atualização de endereço animal com ID: {IdEnderecoAnimal}",id_endereco_animal);

        if (id_endereco_animal != enderecoanimal.Id_endereco_animal)
        {
            _logger.LogWarning("Id endereco animal está difeerente doque esta no sistema.");

            return BadRequest("Id endereco animal está incorreto");
        }
        try
        {
            var enderecoAnimalAtualizado =await _enderecoAnimalService.UpdateEnderecoAnimalAsync(id_endereco_animal,enderecoanimal);

            _context.Entry(enderecoAnimalAtualizado).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Atualização de endereço animal  foi concluída com sucesso.");

            return Ok(enderecoAnimalAtualizado);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnderecoAnimalExists(id_endereco_animal))
            {
                _logger.LogWarning("Id endereço animal não encontrado.");

                return NotFound("Id endereço animal não encontrado");
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro em atualizar endereco animal.");

            return BadRequest(
                $"Erro em atualizar endereço animal: {ex.Message} ");
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
    /// <response code="200">Endereço animal criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criar endereço animal</returns>
    [HttpPost]
    [Route("criar/enderecoanimal")]
    public async Task<ActionResult<EnderecoAnimal>> PostEnderecoAnimal(
        EnderecoAnimal enderecoanimal)
    {
        _logger.LogInformation("Iniciando criação de endereço animal");

        try
        {
            var animalExiste = await _context.Animals.FirstOrDefaultAsync(a => a.Id_animal == enderecoanimal.Id_animal);

            if (animalExiste != null)
            {
                var enderecoAnimalCriado = await _enderecoAnimalService.CreateEnderecoAnimalAsync(enderecoanimal);

                _context.EnderecoAnimals.Add(enderecoAnimalCriado);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Endereço animal criado com sucesso.");

                return Ok(enderecoAnimalCriado);
            }
            else
            {
                _logger.LogWarning("Id animal informado não existe no sistema");

                return BadRequest("Id animal não existe");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Erro ao salvar os dados do endereço animal");

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
        _logger.LogInformation("Iniciando remoção de endereço animal. ID: {IdEnderecoAnimal}",id_endereco_animal);

        try
        {
            var enderecoanimal = await _context.EnderecoAnimals.FindAsync(id_endereco_animal);

            if (enderecoanimal == null)
            {
                _logger.LogWarning("Id oferecido esta diferente ao id presente principal.");

                return NotFound("Id não encontrado");
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Endereço animal removido com sucesso.");

            return NoContent();
        }catch(Exception ex)
        {
            _logger.LogError(ex,"Erro em deletar endereço animal.");

            return BadRequest($"Erro em deletar: {ex.Message}");
        }

    }

}
