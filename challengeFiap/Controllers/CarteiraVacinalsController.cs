using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.src.Models;
using challengeFiap.src.Data;

[Route("api/[controller]")]
[ApiController]
public class CarteiraVacinalsController : ControllerBase
{
    private readonly AppDbContext _context;
    public CarteiraVacinalsController(AppDbContext context)
    {
        _context = context;
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
        var relatorioCarteiraVacinal = await _context.CarteiraVacinals.ToArrayAsync();
        return Ok(relatorioCarteiraVacinal);
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
        try
        {
            var carteiravacinal = await _context.CarteiraVacinals.FindAsync(id_carteiravacinal);

            if (carteiravacinal == null)
            {
                return NotFound($"Id carteira vacinal não encontrada");
            }
            return Ok(carteiravacinal);
        }
        catch(Exception ex)
        {
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
        if (id_carteiravacinal != carteiravacinal.Id_carteiraVacinal)
        {
            return BadRequest("O id de carteira vacinal esta incorreto");
        }

        try
        {
            _context.Entry(carteiravacinal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarteiraVacinalExists(id_carteiravacinal))
            {
                return NotFound("Não foi encontrado");
            }
            else
            {
                throw;
            }
        }
        catch(Exception ex)
        {
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
        try
        {
            var AnimalExistente = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id_animal == carteiravacinal.Id_animal);

            if (AnimalExistente != null) 
            { 
                _context.CarteiraVacinals.Add(carteiravacinal);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCarteiraVacinal", new { id_carteiraVacinal = carteiravacinal.Id_carteiraVacinal }, carteiravacinal);
            }
            else
            {
                return BadRequest("Id do animal não encontrado");
            }
        }
        catch (Exception ex) 
        {
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
        try
        {
            var carteiraVacinalExiste = await _context.CarteiraVacinals.FirstOrDefaultAsync(e => e.Id_carteiraVacinal == id_carteiravacinal);
            if(carteiraVacinalExiste== null)
            {
                return NotFound("Carteira Vacinal não encontrada");
            }
            _context.CarteiraVacinals.Remove(carteiraVacinalExiste);
            await _context.SaveChangesAsync();
            return NoContent();
        }catch (Exception ex)
        {
            return BadRequest($"Erro ao deletar : {ex.Message}");
        }
    }
}
