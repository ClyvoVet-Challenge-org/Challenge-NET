using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Application.Service;

[Route("api/[controller]")]
[ApiController]
public class ClinicasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ClinicasController> _logger;
    private readonly IClinicaService _clinicaService;

    public ClinicasController(
        AppDbContext context,
        ILogger<ClinicasController> logger,
        IClinicaService clinicaService)
    {
        _context = context;
        _logger = logger;
        _clinicaService = clinicaService;
    }

    // GET: api/Clinica

    /// <summary>
    /// Relatorio clinica.
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio clinica</returns>
    [HttpGet]
    [Route("relatorio/clinica")]
    public async Task<ActionResult<IEnumerable<Clinica>>> GetAllClinica()
    {
        _logger.LogInformation("Iniciando a busca de clinicas");

        try
        {
            var relatorioClinica = await _context.Clinicas.ToListAsync();

            _logger.LogInformation("Busca de clinicas concluída com sucesso");

            return Ok(relatorioClinica);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca na clinica");

            return BadRequest($"Erro em processar a busca: {ex.Message}");
        }
    }

    // GET: api/Clinica/5

    /// <summary>
    /// Relatorio de clinica feito pelo id.
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <param name="id_clinica">Id de buscar da clinica</param>
    /// <returns>Relatorio clinica</returns>
    [HttpGet]
    [Route("relatorio/clinica/{id_clinica:int}")]
    public async Task<ActionResult<Clinica>> GetClinica(int id_clinica)
    {
        _logger.LogInformation(
            "Iniciando a busca de clinica com ID: {IdClinica}",
            id_clinica);

        try
        {
            var clinica = await _clinicaService.GetClinicaIdAsync(id_clinica);

            _logger.LogInformation("Clinica encontrada com sucesso. ID: {IdClinica}", id_clinica);

            return Ok(clinica);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro no processamento da buscar pela clinica. ID: {IdClinica}",id_clinica);

            return BadRequest($"Erro em processar a buscar pela clinica: {ex.Message}");
        }
    }

    // PUT: api/Clinica/5

    /// <summary>
    /// Atualizar dados clinica
    /// </summary>
    /// <param name="id_clinica">Id clinica para a url</param>
    /// <param name="clinica">Novos dados a clinica</param>
    /// <response code="200">clinica atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">clinica não encontrado</response>
    /// <returns>Atualização clinica</returns>
    [HttpPut]
    [Route("atualizar/clinica/{id_clinica:int}")]
    public async Task<IActionResult> PutClinica(int id_clinica, Clinica clinica)
    {
        _logger.LogInformation("Iniciando atualizacao de clinica com ID: {IdClinica}",id_clinica);

        if (id_clinica != clinica.Id_clinica)
        {
            _logger.LogWarning("O id da clinica esta incorreto.");
            return BadRequest("O id da clinica esta incorreto");
        }

        try
        {
            var clinicaAtualizada = await _clinicaService.UpdateClinicaAsync(id_clinica, clinica);

            _context.Entry(clinicaAtualizada).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Atualizacao de clinica do id: {IdClinica} concluída com sucesso.",id_clinica);

            return Ok(clinicaAtualizada);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClinicaExists(id_clinica))
            {
                _logger.LogWarning("Id {IdClinica} não foi encontrado",id_clinica);

                return NotFound("A clinica não encontrada");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro em atualizar clinica.");

            return BadRequest($"Erro em atualizar clinica: {ex.Message}");
        }
    }

    private bool ClinicaExists(int id_clinica)
    {
        return _context.Clinicas.FirstOrDefault(e => e.Id_clinica == id_clinica) != null;
    }

    // POST: api/Clinica

    /// <summary>
    /// Criação clinica
    /// </summary>
    /// <param name="clinica">Inserir dados</param>
    /// <response code="200">clinica criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criado sucesso</returns>
    [HttpPost]
    [Route("criar/clinica")]
    public async Task<ActionResult<Clinica>> PostClinica(Clinica clinica)
    {
        _logger.LogInformation("Iniciando criação de clinica");

        try
        {
            var existecpnj = await _context.Clinicas.FirstOrDefaultAsync(c => c.Cnpj_clinica == clinica.Cnpj_clinica);

            if (existecpnj != null)
            {
                _logger.LogWarning("O cnpj informado já existe");

                return BadRequest("O cnpj já existe");
            }

            var clinicaCriada = await _clinicaService.CreateadAsync(clinica);

            _context.Clinicas.Add(clinicaCriada);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Clinica {IdClinica}, criada com sucesso",clinica.Id_clinica);

            return Ok(clinicaCriada);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Erro em salvar os dados");

            return BadRequest($"Erro em salvar os dados: {ex.Message}");
        }
    }

    // DELETE: api/Clinica/5

    /// <summary>
    /// Remove dados de um clinica pelo id
    /// </summary>
    /// <param name="id_clinica">Id clinica para ser buscado</param>
    /// <response code="204">clinica removido com sucesso.</response>
    /// <response code="404">Animal não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns></returns>
    [HttpDelete]
    [Route("deleta/clinica/{id_clinica:int}")]
    public async Task<IActionResult> DeleteClinica(int id_clinica)
    {
        _logger.LogInformation("Iniciando remoção de clinica. ID: {IdClinica}",id_clinica);

        try
        {
            var clinica = await _clinicaService.DeleteClinicaAsync(id_clinica);

            _context.Clinicas.Remove(clinica);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Clinica removida com sucesso");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro em deletar clinica.");

            return BadRequest(
                $"Erro em deletar: {ex.Message}");
        }
    }
}
