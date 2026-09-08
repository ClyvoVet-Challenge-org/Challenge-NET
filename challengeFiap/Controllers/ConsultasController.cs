using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;
using challengeFiap.Domain.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ConsultasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ConsultasController> _logger;
    private readonly IConsultaService _consultaService;

    public ConsultasController(
        AppDbContext context,
        ILogger<ConsultasController> logger,
        IConsultaService consultaService)
    {
        _context = context;
        _logger = logger;
        _consultaService = consultaService;
    }

    // GET: api/Consulta

    /// <summary>
    /// Relaorio consulta
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Buscar</returns>
    [HttpGet]
    [Route("relatorio/consulta")]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetAllConsulta()
    {
        _logger.LogInformation("Iniciando a busca de consultas");

        try
        {
            var relatorioConsulta = await _context.Consultas.ToListAsync();

            _logger.LogInformation("Busca de consultas concluída com sucesso");

            return Ok(relatorioConsulta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca a consulta");

            return BadRequest($"Erro em processar a busca: {ex.Message}");
        }
    }

    // GET: api/Consulta/5

    /// <summary>
    /// Relatorio de consulta pelo id:
    /// </summary>
    /// <param name="id_consulta">Id para buscar a informação</param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio</returns>
    [HttpGet]
    [Route("relatorio/consulta/{id_consulta:int}")]
    public async Task<ActionResult<Consulta>> GetConsulta(int id_consulta)
    {
        _logger.LogInformation("Iniciando a busca de consulta com ID: {IdConsulta}",id_consulta);

        try
        {
            var consulta = await _context.Consultas.FindAsync(id_consulta);

            if (consulta == null)
            {
                _logger.LogWarning("Id Consulta não encontrado. ID: {IdConsulta}",id_consulta);

                return NotFound("Id Consulta não encontrado");
            }

            _logger.LogInformation("Consulta encontrada com sucesso. ID: {IdConsulta}",id_consulta);

            return Ok(consulta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro em buscar consulta. ID: {IdConsulta}",id_consulta);

            return BadRequest($"Erro em buscar: {ex.Message}");
        }
    }

    // PUT: api/Consulta/5

    /// <summary>
    /// Atualizar dados consulta
    /// </summary>
    /// <param name="id_consulta">Id consulta para verificar: </param>
    /// <param name="consulta">dados consulta</param>
    /// <response code="200">Consulta atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Consulta não encontrado</response>
    /// <returns>Atualização consulta</returns>
    [HttpPut]
    [Route("atualizar/consulta/{id_consulta:int}")]
    public async Task<IActionResult> PutConsulta(int id_consulta, Consulta consulta)
    {
        _logger.LogInformation("Iniciando atualização de consulta com ID: {IdConsulta}",id_consulta);

        if (id_consulta != consulta.Id_consulta)
        {
            _logger.LogWarning("Id da consulta está incorreto. ID informado: {IdInformado}, ID da consulta: {IdConsulta}",id_consulta,consulta.Id_consulta);

            return BadRequest("Id da consulta está incorreto");
        }

        try
        {
            var consultaAtualizada =await _consultaService.UpdateConsultaAsync(id_consulta,consulta);

            _context.Entry(consultaAtualizada).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Consulta atualizada com sucesso. ID: {IdConsulta}",id_consulta);

            return Ok(consultaAtualizada);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ConsultaExists(id_consulta))
            {
                _logger.LogWarning("Consulta não encontrada. ID: {IdConsulta}",id_consulta);

                return NotFound("Consulta não encontrada");
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro em atualizar consulta. ID: {IdConsulta}",id_consulta);
            return BadRequest($"Erro em atualizar: {ex.Message}");
        }
    }
    private bool ConsultaExists(int id_consulta)
    {
        return _context.Consultas.FirstOrDefault(e => e.Id_consulta == id_consulta) != null;
    }

    // POST: api/Consulta

    /// <summary>
    /// Criação de consulta
    /// </summary>
    /// <param name="consulta">Dados para ser inseridos</param>
    /// <response code="201">Consulta criada com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação de dados consulta</returns>
    [HttpPost]
    [Route("criar/consulta")]
    public async Task<ActionResult<Consulta>> PostConsulta(Consulta consulta)
    {
        _logger.LogInformation("Iniciando criação de consulta. ID Animal: {IdAnimal}, ID Veterinario: {IdVet}",consulta.Id_animal,consulta.Id_vet);

        try
        {
            var animalExistente = await _context.Animals.FirstOrDefaultAsync(a => a.Id_animal == consulta.Id_animal);

            var vetExistente = await _context.Veterinarios.FirstOrDefaultAsync(v => v.Id_vet == consulta.Id_vet);

            if (animalExistente != null && vetExistente != null)
            {
                var consultaCriada = await _consultaService.CreateConsultaAsync(consulta);

                _context.Consultas.Add(consultaCriada);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Consulta criada com sucesso. ID: {IdConsulta}", consulta.Id_consulta);

                return Ok(consultaCriada);
            }
            else
            {
                _logger.LogWarning("Ids não encontrado. ID Animal: {IdAnimal}, ID Veterinario: {IdVet}", consulta.Id_animal, consulta.Id_vet);

                return BadRequest("Ids não encontrado");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar os dados da consulta. ID Animal: {IdAnimal}, ID Veterinario: {IdVet}", consulta.Id_animal, consulta.Id_vet);
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }
    }

    // DELETE: api/Consulta/5

    /// <summary>
    /// Deletar consulta
    /// </summary>
    /// <param name="id_consulta">id para buscar</param>
    /// <response code="204">consulta removido com sucesso.</response>
    /// <response code="404">consulta não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>deletar consulta</returns>
    [HttpDelete]
    [Route("deleta/consulta/{id_consulta:int}")]
    public async Task<IActionResult> DeleteConsulta(int id_consulta)
    {
        _logger.LogInformation("Iniciando remoção de consulta. ID: {IdConsulta}",id_consulta);

        try
        {
            var consulta = await _context.Consultas.FindAsync(id_consulta);

            if (consulta == null)
            {
                _logger.LogWarning("Consulta não encontrado. ID: {IdConsulta}",id_consulta);

                return NotFound("Consulta não encontrado.");
            }

            _context.Consultas.Remove(consulta);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Consulta removida com sucesso. ID: {IdConsulta}",id_consulta);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro em deletar consulta. ID: {IdConsulta}",id_consulta);

            return BadRequest($"Erro em deletar: {ex.Message}");
        }
    }
}