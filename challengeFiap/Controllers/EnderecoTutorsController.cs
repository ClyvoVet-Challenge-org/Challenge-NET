using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;
using challengeFiap.Domain.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class EnderecoTutorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<EnderecoTutorsController> _logger;
    private readonly IenderecoTutorService _enderecoTutorService;
    public EnderecoTutorsController(AppDbContext context, ILogger<EnderecoTutorsController> logger, IenderecoTutorService enderecoTutorService)
    {
        _context = context;
        _logger = logger;
        _enderecoTutorService = enderecoTutorService;
    }

    // GET: api/EnderecoResponsavel

    /// <summary>
    /// Relaorio de dados endereço responsavel
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio endereço responsavel</returns>
    [HttpGet]
    [Route("relatorio/enderecoresponsavel")]
    public async Task<ActionResult<IEnumerable<EnderecoTutor>>> GetAllEnderecoResponsavel()
    {
        _logger.LogInformation("Iniciando a busca de endereço responsavel");

        try
        {
            var relatorioEnResponsavel = await _context.EnderecoTutors.ToListAsync();

            _logger.LogInformation("Busca de endereço responsavel concluída com sucesso");
            return Ok(relatorioEnResponsavel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca de endereço responsavel");
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
    }

    // GET: api/EnderecoResponsavel/5

    /// <summary>
    /// Relatorio de endereço responsavel pelo id
    /// </summary>
    /// <param name="id_endereco_responsavel"></param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/enderecoresponsavel/{id_endereco_responsavel:int}")]
    public async Task<ActionResult<EnderecoTutor>> GetEnderecoResponsavel(int id_endereco_responsavel)
    {
        _logger.LogInformation("Iniciando a busca de endereço responsavel pelo ID {IdEnderecoResponsavel}", id_endereco_responsavel);

        try
        {
            var enderecoresponsavel = await _enderecoTutorService.GetEnderecoTutorIdAsync(id_endereco_responsavel);

            _logger.LogInformation("Endereço responsavel encontrado com sucesso para o ID {IdEnderecoResponsavel}", id_endereco_responsavel);
            return Ok(enderecoresponsavel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar endereço responsavel pelo ID {IdEnderecoResponsavel}", id_endereco_responsavel);
            return BadRequest($"Erro em buscar: {ex.Message}");
        }

    }

    // PUT: api/EnderecoResponsavel/5

    /// <summary>
    /// Atualizar dados endereço responsavel
    /// </summary>
    /// <param name="id_endereco_responsavel">Id para pode atualizar</param>
    /// <param name="enderecoresponsavel">Dados para ser inseridos</param>
    /// <response code="200">Endereço responsavel atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Endereço responsavel não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/enderecoresponsavel/{id_endereco_responsavel:int}")]
    public async Task<IActionResult> PutEnderecoResponsavel(int id_endereco_responsavel, EnderecoTutor enderecoresponsavel)
    {
        _logger.LogInformation("Iniciando a atualização do endereço responsavel com ID {IdEnderecoResponsavel}", id_endereco_responsavel);

        if (id_endereco_responsavel != enderecoresponsavel.Id_endereco_tutor)
        {
            _logger.LogWarning("ID do endereço responsavel informado na rota é diferente do ID enviado no objeto. ID: {IdEnderecoResponsavel}", id_endereco_responsavel);
            return BadRequest("Id endereco responsal esta incorreto");
        }

        try
        {
            var enderecoTutorAtualizado = await _enderecoTutorService.UpdateEnderecoTutorAsync(id_endereco_responsavel, enderecoresponsavel);

            _context.Entry(enderecoTutorAtualizado).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Endereço responsavel com ID {IdEnderecoResponsavel} atualizado com sucesso", id_endereco_responsavel);
            return Ok(enderecoTutorAtualizado);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnderecoResponsavelExists(id_endereco_responsavel))
            {
                _logger.LogWarning("Endereço responsavel não encontrado durante a atualização. ID: {IdEnderecoResponsavel}", id_endereco_responsavel);
                return NotFound("Endereço responsavel não encontrado");
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar endereço responsavel com ID {IdEnderecoResponsavel}", id_endereco_responsavel);
            return BadRequest($"Erro encontrado: {ex.Message}");
        }

    }

    private bool EnderecoResponsavelExists(int id_endereco_responsavel)
    {
        return _context.EnderecoTutors.FirstOrDefault(e => e.Id_endereco_tutor == id_endereco_responsavel) != null;
    }

    // POST: api/EnderecoResponsavel

    /// <summary>
    /// Criar endereço responsavel
    /// </summary>
    /// <param name="enderecoresponsavel">Criação de dados de endereço responsavel</param>
    /// <response code="200">Endereço reponsavel criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criar endereço responsavel</returns>
    [HttpPost]
    [Route("criar/enderecoresponsavel")]
    public async Task<ActionResult<EnderecoTutor>> PostEnderecoResponsavel(EnderecoTutor enderecoresponsavel)
    {
        _logger.LogInformation("Iniciando a criação de endereço responsavel para o tutor ID {IdTutor}", enderecoresponsavel.Id_tutor);
        try
        {
            var responsavelExiste = await _context.Tutor.FirstOrDefaultAsync(a => a.Id_tutor == enderecoresponsavel.Id_tutor);

            if (responsavelExiste != null)
            {
                var enderecoTutorCriado = await _enderecoTutorService.CreateEnderecoTutorAsync(enderecoresponsavel);

                _context.EnderecoTutors.Add(enderecoTutorCriado);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Endereço responsavel criado com sucesso. ID: {IdEnderecoTutor}", enderecoresponsavel.Id_endereco_tutor);
                return Ok(enderecoTutorCriado);
            }
            else
            {
                _logger.LogWarning("Tutor não encontrado para criação do endereço responsavel. ID do tutor: {IdTutor}", enderecoresponsavel.Id_tutor);
                return BadRequest($"ID resonsavel não encontrado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar endereço responsavel para o tutor ID {IdTutor}", enderecoresponsavel.Id_tutor);
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }

    }

    // DELETE: api/EnderecoResponsavel/5

    /// <summary>
    /// Remove dados de endereço responsavel
    /// </summary>
    /// <param name="id_endereco_responsavel">Id para pode remover: </param>
    /// <response code="204">Endereco responsavel removido com sucesso.</response>
    /// <response code="404">Endereco responsavel não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletado: </returns>
    [HttpDelete]
    [Route("deleta/enderecoresponsavel/{id_endereco_responsavel:int}")]
    public async Task<IActionResult> DeleteEnderecoResponsavel(int id_endereco_responsavel)
    {
        _logger.LogInformation("Iniciando a exclusão do endereço responsavel com ID {IdEnderecoResponsavel}", id_endereco_responsavel);

        try
        {
            var enderecoresponsavel = await _enderecoTutorService.DeleteEnderecoTutorAsync(id_endereco_responsavel);

            _context.EnderecoTutors.Remove(enderecoresponsavel);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Endereço responsavel com ID {IdEnderecoResponsavel} excluído com sucesso", id_endereco_responsavel);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar endereço responsavel com ID {IdEnderecoResponsavel}", id_endereco_responsavel);
            return BadRequest($"Erro em deletar: {ex.Message}");
        }

    }
}