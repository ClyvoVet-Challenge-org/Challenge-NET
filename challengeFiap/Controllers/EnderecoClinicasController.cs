using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EnderecoClinicasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<EnderecoClinicasController> _logger;
    private readonly IenderecoClinicaService _enderecoClinicaService;

    public EnderecoClinicasController(AppDbContext context, ILogger<EnderecoClinicasController> logger, IenderecoClinicaService enderecoClinicaService)
    {
        _context = context;
        _logger = logger;
        _enderecoClinicaService = enderecoClinicaService;
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
        _logger.LogInformation("Iniciando a busca de endereço clinica");

        try
        {
            var relatorioEnClinica = await _context.EnderecoClinicas.ToListAsync();

            _logger.LogInformation("Busca de endereço clinica concluída com sucesso");
            return Ok(relatorioEnClinica);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca a endereço clinica");
            return BadRequest($"Erro em processar a busca: {ex.Message}");
        }
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
        _logger.LogInformation("Iniciando a busca de endereço clinica com ID");

        try
        {
            var enderecoclinica = await _context.EnderecoClinicas.FindAsync(id_endereco_clinica);

            if
            {
                _logger.LogWarning("Endereço CLINICA não encontrado ");
                return NotFound("Id não encontrado");
            }

            _logger.LogInformation("Endereço clinica encontrado com sucesso.");

            return Ok(enderecoclinica);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro achado ao buscar endereço clinica. ID: {IdEnderecoClinica}", id_endereco_clinica);

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
        _logger.LogInformation("Iniciando atualização de endereço clinica com ID: {IdEnderecoClinica}", id_endereco_clinica);

        if (id_endereco_clinica != enderecoclinica.Id_endereco_clinica)
        {
            _logger.LogWarning("O id endereço clinica esta errado. ID informado: {IdInformado}, ID do endereço: {IdEnderecoClinica}",id_endereco_clinica,enderecoclinica.Id_endereco_clinica);

            return BadRequest("O id endereço clinica esta errado");
        }

        try
        {
            var enderecoClinicaAtualizado = await _enderecoClinicaService.UpdateEnderecoClinicaAsync(id_endereco_clinica, enderecoclinica);

            _context.Entry(enderecoClinicaAtualizado).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Atualização de endereço clinica concluída com sucesso. ID: {IdEnderecoClinica}", id_endereco_clinica);

            return Ok(enderecoClinicaAtualizado);

        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnderecoClinicaExists(id_endereco_clinica))
            {
                _logger.LogWarning("Id não encontrado. ID: {IdEnderecoClinica}", id_endereco_clinica);

                return NotFound("Id não encontrado");
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na atualização de endereço clinica. ID: {IdEnderecoClinica}", id_endereco_clinica);

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
    /// <response code="200">Endereço clinica criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação de endereço clinica</returns>
    [HttpPost]
    [Route("criar/enderecoclinica")]
    public async Task<ActionResult<EnderecoClinica>> PostEnderecoClinica(EnderecoClinica enderecoclinica)
    {
        _logger.LogInformation("Iniciando criação de endereço clinica. ID Clinica: {IdClinica}",enderecoclinica.Id_clinica);

        try
        {
            var clinicaexiste = await _context.Clinicas
                .FirstOrDefaultAsync(a => a.Id_clinica == enderecoclinica.Id_clinica);

            if (clinicaexiste != null)
            {
                var enderecoClinicaCriado = await _enderecoClinicaService.CreateEnderecoClinicaAsync(enderecoclinica);

                _context.EnderecoClinicas.Add(enderecoClinicaCriado);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Endereço clinica criado com sucesso. ID: {IdEnderecoClinica}",enderecoclinica.Id_endereco_clinica);

                return Ok(enderecoClinicaCriado);
            }
            else
            {
                _logger.LogWarning("Id não existe. ID Clinica: {IdClinica}", enderecoclinica.Id_clinica);

                return BadRequest($"Id não existe");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Erro ao salvar os dados do endereço clinica. ID Clinica: {IdClinica}",enderecoclinica.Id_clinica);

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
        _logger.LogInformation("Iniciando remoção de endereço clinica. ID: {IdEnderecoClinica}", id_endereco_clinica);

        try
        {
            var enderecoclinica = await _context.EnderecoClinicas.FindAsync(id_endereco_clinica);

            if (enderecoclinica == null)
            {
                _logger.LogWarning("Endereço CLINICA não encontrado para exclusão");
                return NotFound("Id não encontrado");
            }
            
            _context.EnderecoClinicas.Remove(enderecoclinica);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Endereço clinica removido com sucesso. ID: {IdEnderecoClinica}", id_endereco_clinica);

            return NoContent();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Erro em deletar endereço clinica. ID: {IdEnderecoClinica}", id_endereco_clinica);

            return BadRequest($"Erro em deletar: {ex.Message}");
        }

    }
}
