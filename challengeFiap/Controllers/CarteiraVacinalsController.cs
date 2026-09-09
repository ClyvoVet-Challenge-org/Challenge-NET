using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;
using challengeFiap.Application.Service;
using challengeFiap.Domain.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class CarteiraVacinalsController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly ICarteiraVacinalService _carteiraVacinalService;
    private readonly ILogger<CarteiraVacinalsController> _logger;

    public CarteiraVacinalsController(
        AppDbContext context,
        ILogger<CarteiraVacinalsController> logger,
        ICarteiraVacinalService carteiraVacinalService)
    {
        _context = context;
        _logger = logger;
        _carteiraVacinalService = carteiraVacinalService;
    }

    // GET: api/CarteiraVacinal

    /// <summary>
    /// Carrgea todos os dados da carteira vacinal presente no banco
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Lista de carteira vacinal</returns>
    [HttpGet]
    [Route("relatorio/carteiravacinal")]
    public async Task<ActionResult<IEnumerable<CarteiraVacinal>>> GetAllCarteiraVacinal()
    {
        _logger.LogInformation("Começado a busca de carteira vacinal do pet");

        try
        {
            var relatorioCarteiraVacinal = await _context.CarteiraVacinals.ToArrayAsync();

            _logger.LogInformation("Busca realizada com sucesso.");
            return Ok(relatorioCarteiraVacinal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca de carteira vacinal");
            return BadRequest($"Erro em processar a busca: {ex.Message}");
        }
    }

    // GET: api/CarteiraVacinal/5

    /// <summary>
    /// Carregar os dados de carteira vacinal por id
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <param name="id_carteiravacinal">Id de carteira de vacinação</param>
    /// <returns>Lista com id vacinação</returns>
    [HttpGet]
    [Route("relatorio/carteiravacinal/{id_carteiravacinal:int}")]
    public async Task<ActionResult<CarteiraVacinal>> GetCarteiraVacinal(int id_carteiravacinal)
    {
        _logger.LogInformation("busca de carteira vacinal com ID: {IdCarteiraVacinal}",id_carteiravacinal);
        try
        {
            var carteiravacinal = await _context.CarteiraVacinals.FindAsync(id_carteiravacinal);

            if (carteiravacinal == null)
            {
                _logger.LogWarning("Carteira vacinal, ID: {IdCarteiraVacinal}, nao foi encontrada.",id_carteiravacinal);
                return NotFound($"Id carteira vacinal não encontrada");
            }

            _logger.LogInformation("Carteira Vacinal encontrada com sucesso,");

            return Ok(carteiravacinal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro em processamento");
            return BadRequest($"Erro em processar a buscar: {ex.Message}");
        }

    }

    // PUT: api/CarteiraVacinal/5

    /// <summary>
    /// Atualizar dados de carteira vacinação
    /// </summary>
    /// <param name="id_carteiravacinal">Id da carteira que deseja ser atualizada</param>
    /// <param name="carteiravacinal">Dados de carteira vacinação</param>
    /// <response code="204">Carteira vacinação atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Id carteira vacinação não encontrado</response>
    /// <returns>Dados atualizados</returns>
    [HttpPut]
    [Route("atualizar/carteiravacinal/{id_carteiravacinal:int}")]
    public async Task<IActionResult> PutCarteiraVacinal(int id_carteiravacinal, CarteiraVacinal carteiravacinal)
    {
        _logger.LogInformation("Iniciando atualizacao de carteira vacinal.");

        if (id_carteiravacinal != carteiravacinal.Id_carteiraVacinal)
        {
            _logger.LogWarning("O id de carteira vacinal esta errada com a principal. ID informada: {IdInformado}, ID oferecido para atualizar: {IdCarteiraVacinal}",id_carteiravacinal,carteiravacinal.Id_carteiraVacinal);

            return BadRequest("O id de carteira vacinal esta incorreto");
        }

        try
        {
            var carteiraVacinalAtualizar= await _carteiraVacinalService.UpdateCarteiraVacinalAsync(id_carteiravacinal, carteiravacinal);
            _context.Entry(carteiraVacinalAtualizar).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Atualizacao de carteira vacinal concluída com sucesso. ID: {IdCarteiraVacinal}",id_carteiravacinal);

            return Ok(carteiraVacinalAtualizar);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarteiraVacinalExists(id_carteiravacinal))
            {
                _logger.LogWarning("Carteira vacinal não encontrada para atualização. ID: {IdCarteiraVacinal}",id_carteiravacinal);

                return NotFound("Não foi encontrado");
            }

            throw;
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro em atualizar carteirs vacinal. ID: {IdCarteiraVacinal}",id_carteiravacinal);

            return BadRequest($"Erro em atualizar carteirs vacinal: {ex.Message}");
        }
    }

    private bool CarteiraVacinalExists(int id_carteiravacinal)
    {
        return _context.CarteiraVacinals.FirstOrDefault(e => e.Id_carteiraVacinal == id_carteiravacinal) != null;
    }

    // POST: api/CarteiraVacinal

    /// <summary>
    /// Criar dados de carteira vacinação
    /// </summary>
    /// <param name="carteiravacinal">Dados para ser inseridos</param>
    /// <response code="201">carteira vacinação criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns></returns>
    [HttpPost]
    [Route("criar/carteiravacinal")]
    public async Task<ActionResult<CarteiraVacinal>> PostCarteiraVacinal(CarteiraVacinal carteiravacinal)
    {
        _logger.LogInformation("Iniciando criação da carteira vacinal. ID Animal: {IdAnimal}",carteiravacinal.Id_animal);

        try
        {
            var AnimalExistente = await _context.Animals.FirstOrDefaultAsync(a => a.Id_animal == carteiravacinal.Id_animal);

            if (AnimalExistente != null)
            {
                var carteiraVacinal = await _carteiraVacinalService.CreateCarteiraVacinalAsync(carteiravacinal);

                _context.CarteiraVacinals.Add(carteiraVacinal);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Carteira vacinal criada com sucesso. ID: {IdCarteiraVacinal}",carteiravacinal.Id_carteiraVacinal);

                return CreatedAtAction("GetCarteiraVacinal", new { id_carteiraVacinal = carteiravacinal.Id_carteiraVacinal }, carteiravacinal);
            }
            else
            {
                _logger.LogWarning("Id do animal não encontrado. ID Animal: {IdAnimal}",carteiravacinal.Id_animal);

                return BadRequest("Id do animal não encontrado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro em salvar os dados da carteira vacinal. ID Animal: {IdAnimal}",
                carteiravacinal.Id_animal);

            return BadRequest($"Erro em salvar os dados: {ex.Message}");
        }
    }

    // DELETE: api/CarteiraVacinal/5

    /// <summary>
    /// Remove dados da carteira de vacinação
    /// </summary>
    /// <param name="id_carteiravacinal">Id para deletar</param>
    /// <response code="204">carteira vacinação removido com sucesso.</response>
    /// <response code="404"> não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>sucesso deletado</returns>
    [HttpDelete]
    [Route("deleta/carteiravacinal/{id_carteiravacinal:int}")]
    public async Task<IActionResult> DeleteCarteiraVacinal(int id_carteiravacinal)
    {
        _logger.LogInformation("comecando processo de deletar da carteira vacinal. ID: {IdCarteiraVacinal}",id_carteiravacinal);

        try
        {
            var carteiraVacinalExiste = await _context.CarteiraVacinals.FirstOrDefaultAsync(e => e.Id_carteiraVacinal == id_carteiravacinal);

            if (carteiraVacinalExiste==null)
            {
                _logger.LogWarning("Carteira Vacinal não encontrada. ID: {IdCarteiraVacinal}", id_carteiravacinal);
                return NotFound("Carteira Vacinal não encontrada");
            }

            _context.CarteiraVacinals.Remove(carteiraVacinalExiste);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Remoção da carteira vacinal concluída com sucesso. ID: {IdCarteiraVacinal}",id_carteiravacinal);

            return NoContent();
        }catch (Exception ex)
        {
            _logger.LogError(ex,"Erro ao deletar a carteira vacinal. ID: {IdCarteiraVacinal}",id_carteiravacinal);

            return BadRequest($"Erro ao deletar : {ex.Message}");
        }
    }
}