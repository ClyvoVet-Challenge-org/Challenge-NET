using challengeFiap.Application.Service;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAnimalService _animalService;
    private readonly ILogger<AnimalsController> _logger;

    public AnimalsController(
        AppDbContext context,
        ILogger<AnimalsController> logger,
        IAnimalService animalService)
    {
        _context = context;
        _logger = logger;
        _animalService = animalService;
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
        _logger.LogInformation("Começando o relatorio dos animais.");
        try
        {
            var relatorioAnimal = await _context.Animals.ToListAsync();

            _logger.LogInformation("Relatorio geral de animal realizado com sucesso - total presente: {Count}", relatorioAnimal.Count);

            return Ok(relatorioAnimal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os animais");

            return StatusCode(500, "Erro interno do servidor");
        }
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
        _logger.LogInformation($"Começando a buscar pelo animal por seu id: {id_animal}");
        try
        {
            var animal = await _context.Animals.FindAsync(id_animal);

            if (animal == null)
            {
                _logger.LogWarning("Id não inseridor");

                return NotFound("Id Animal não encontrada");
            }
            _logger.LogInformation("Busca do animal com id_animal -> {IdAnimal} realizada com sucesso", id_animal);

            return Ok(animal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar o animal com ID: {IdAnimal}", id_animal);

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
        _logger.LogInformation("Começando o processo de atualização do animal com ID: {IdAnimal}", id_animal);

        if (id_animal != animal.Id_animal)
        {
            _logger.LogWarning("Id oferecido está errado.");

            return BadRequest("O id de animal esta incorreto");
        }

        try
        {
            var animalAtualizado = await _animalService.UpdateAnimalAsync(id_animal, animal);
            _context.Entry(animalAtualizado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Atualização do animal com ID: {IdAnimal} realizada", id_animal);

            return Ok(animalAtualizado);

        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Erro em atualizar o animal com ID: {IdAnimal}", id_animal);

            if (!AnimalExists(id_animal))
            {
                _logger.LogWarning("Animal com ID: {IdAnimal} não  existe", id_animal);

                return NotFound("O animal não encontrado");
            }
                throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar o animal com ID: {IdAnimal}", id_animal);

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
        _logger.LogInformation("Iniciando processo de criação do animal");
        try
        {
            var responsavelExistente = await _context.Tutor.FirstOrDefaultAsync(a => a.Id_tutor == animal.Id_tutor);

            if (responsavelExistente != null)
            {
                var animalCriado = await _animalService.CreateAnimalAsync(animal);

                _context.Animals.Add(animalCriado);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Criação do animal, {nm_animal}, concluída com sucesso", animal.Nm_animal);

                return Ok(animalCriado);
            }
            else
            {
                _logger.LogWarning("O tutor com o id {IdTutor} não foi encontrado", animal.Id_tutor);
                return BadRequest("Id Tutor não encontrado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar o animal com ID: {IdAnimal}", animal.Id_animal);

            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
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
        _logger.LogInformation("Processo de deletar do animal");
        try
        {
            var animalExistente = await _context.Animals.FirstOrDefaultAsync(e => e.Id_animal == id_animal);
            
            _context.Animals.Remove(animalExistente);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deletar o id animal: {IdAnimal} feito com sucesso", id_animal);

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro ao remover o animal {IdAnimal}", id_animal);

            return BadRequest($"Erro em deletar: {e.Message}");
        }
    }
}
