using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;
using challengeFiap.Domain.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class PrescricaosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<PrescricaosController> _logger;
    private readonly IprescricaoService _prescricaoService;
    public PrescricaosController(AppDbContext context, ILogger<PrescricaosController> logger, IprescricaoService prescricaoService)
    {
        _context = context;
        _logger = logger;
        _prescricaoService = prescricaoService;
    }

    // GET: api/Prescricao

    /// <summary>
    /// Relatorio de daods prescrição
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio prescrição</returns>
    [HttpGet]
    [Route("relatorio/prescricao")]
    public async Task<ActionResult<IEnumerable<Prescricao>>> GetAllPrescricao()
    {
        _logger.LogInformation("Iniciando a busca de prescrições");

        try
        {
            var prescricaoRelatorio = await _prescricaoService.GetAllPrescricaoAsync();

            _logger.LogInformation("Busca de prescrições concluída com sucesso");
            return Ok(prescricaoRelatorio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca de prescrições");
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
    }

    // GET: api/Prescricao/5

    /// <summary>
    /// Relatorio de prescrição
    /// </summary>
    /// <param name="id_prescricao">Buscar pelo id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/prescricao/{id_prescricao:int}")]
    public async Task<ActionResult<Prescricao>> GetPrescricao(int id_prescricao)
    {
        _logger.LogInformation("Iniciando a busca da prescrição pelo ID {IdPrescricao}", id_prescricao);

        try
        {
            try
            {
                var prescricao = await _prescricaoService.GetPrescricaoIdAsync(id_prescricao);
                _logger.LogInformation("Prescricao foi encontrada com sucesso");
                return Ok(prescricao);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "prescricao não encontrada para o ID");
                return NotFound("Id de Prescricao não encontrado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar prescrição pelo ID {IdPrescricao}", id_prescricao);
            return BadRequest($"Erro em buscar: {ex.Message}");
        }

    }

    // PUT: api/Prescricao/5

    /// <summary>
    /// Atualizar dados de prescrição
    /// </summary>
    /// <param name="id_prescricao">ID para pode atualizar</param>
    /// <param name="prescricao">Dados para serem inseridos</param>
    /// <response code="204">Prescrição atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Prescrição não encontrado</response>
    /// <returns></returns>
    [HttpPut]
    [Route("atualizar/prescricao/{id_prescricao:int}")]
    public async Task<IActionResult> PutPrescricao(int id_prescricao, Prescricao prescricao)
    {
        _logger.LogInformation("Iniciando a atualização da prescrição com ID {IdPrescricao}", id_prescricao);

        if (id_prescricao != prescricao.Id_prescricao)
        {
            _logger.LogWarning("ID da prescrição informado na rota é diferente do ID enviado no objeto. ID: {IdPrescricao}", id_prescricao);
            return BadRequest("Id da prescrição está errado.");
        }

        try
        {
            var prescricaoAtualizada = await _prescricaoService.UpdatePrescricaoAsync(id_prescricao, prescricao);

            _context.Entry(prescricaoAtualizada).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Prescrição com ID {IdPrescricao} atualizada com sucesso", id_prescricao);
            return Ok(prescricaoAtualizada);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PrescricaoExists(id_prescricao))
            {
                _logger.LogWarning("Prescrição não encontrada durante a atualização. ID: {IdPrescricao}", id_prescricao);
                return NotFound("Id prescrição não encontrado");
            }
            throw;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar prescrição com ID {IdPrescricao}", id_prescricao);
            return BadRequest($"Erro em atualizar: {ex.Message}");
        }
    }

    private bool PrescricaoExists(int id_prescricao)
    {
        return _context.Prescricaos.FirstOrDefault(e => e.Id_prescricao == id_prescricao) != null;
    }

    // POST: api/Prescricao

    /// <summary>
    /// Criar Precrição
    /// </summary>
    /// <param name="prescricao">Criação de dados a prescrição</param>
    /// <response code="201">Precisção criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criar prescrição</returns>
    [HttpPost]
    [Route("criar/prescricao")]
    public async Task<ActionResult<Prescricao>> PostPrescricao(Prescricao prescricao)
    {
        _logger.LogInformation("Iniciando a criação de prescrição para a consulta ID {IdConsulta}", prescricao.Id_consulta);

        try
        {
            var consultaExiste = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id_consulta == prescricao.Id_consulta);

            if (consultaExiste != null)
            {
                var prescricaoCriada = await _prescricaoService.CreatePrescricaoAsync(prescricao);

                _context.Prescricaos.Add(prescricaoCriada);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Prescrição criada com sucesso. ID: {IdPrescricao}", prescricao.Id_prescricao);
                return Ok(prescricaoCriada);
            }
            else
            {
                _logger.LogWarning("Consulta não encontrada para criação da prescrição. ID: {IdConsulta}", prescricao.Id_consulta);
                return BadRequest($"Id consulta não encontrado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar prescrição para a consulta ID {IdConsulta}", prescricao.Id_consulta);
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }

    }

    // DELETE: api/Prescricao/5

    /// <summary>
    /// Remove dados Precrição
    /// </summary>
    /// <param name="id_prescricao">Id para pode remover: </param>
    /// <response code="204">Prescrição removido com sucesso.</response>
    /// <response code="404">Prescrição não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletado: </returns>
    [HttpDelete]
    [Route("deleta/prescricao/{id_prescricao:int}")]
    public async Task<IActionResult> DeletePrescricao(int id_prescricao)
    {
        _logger.LogInformation("Iniciando a exclusão da prescrição com ID {IdPrescricao}", id_prescricao);

        try
        {
            var prescricao = await _prescricaoService.DeletePrescricaoAsync(id_prescricao);

            _context.Prescricaos.Remove(prescricao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Prescricao foi excluída com sucesso");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar prescrição com ID {IdPrescricao}", id_prescricao);
            return BadRequest($"Erro em deletar: {ex.Message}");
        }

    }
}