using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challengeFiap.Domain.Entities;
using challengeFiap.Infrastruture.Data;
using challengeFiap.Domain.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class VetClinicasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<VetClinicasController> _logger;
    private readonly IVetClinica _vetClinica;

    public VetClinicasController(AppDbContext context, ILogger<VetClinicasController> logger, IVetClinica vetClinica)
    {
        _context = context;
        _logger = logger;
        _vetClinica = vetClinica;
    }

    // GET: api/VetClinica

    /// <summary>
    /// Relatorio de dados de Vet e clinica
    /// </summary>
    /// <response code="200">Busca feita com sucesso</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/vetclinica")]
    public async Task<ActionResult<IEnumerable<VetClinica>>> GetAllVetClinica()
    {
        _logger.LogInformation("Iniciando a busca de veterinários e clínicas");

        try
        {
            var clinicaVet = await _context.VetClinicas.ToListAsync();

            _logger.LogInformation("Busca de veterinários e clínicas concluída com sucesso");
            return Ok(clinicaVet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a busca de veterinários e clínicas");
            return BadRequest($"Erro em buscar: {ex.Message}");
        }
    }

    // GET: api/VetClinica/5

    /// <summary>
    /// Relatorio de Vet Clinica
    /// </summary>
    /// <param name="id_clinica_vet">Buscar id: </param>
    /// <response code="200">Busca feita com sucesso</response>
    /// <response code="404">Id não encontrado</response>
    /// <returns>Relatorio: </returns>
    [HttpGet]
    [Route("relatorio/vetclinica/{id_clinica_vet:int}")]
    public async Task<ActionResult<VetClinica>> GetVetClinica(int id_clinica_vet)
    {
        _logger.LogInformation("Iniciando a busca de vet clínica pelo ID {IdClinicaVet}", id_clinica_vet);

        try
        {
            var vetclinica = await _context.VetClinicas.FirstOrDefaultAsync(v => v.Id_clinica_vet == id_clinica_vet);

            _logger.LogInformation("Vet clínica encontrada com sucesso para o ID {IdClinicaVet}", id_clinica_vet);
            return Ok(vetclinica);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar vet clínica pelo ID {IdClinicaVet}", id_clinica_vet);
            return BadRequest($"Erro em buscar: {ex.Message}");
        }

    }

    // PUT: api/VetClinica/5

    /// <summary>
    /// Atualizar dados de vet clinica
    /// </summary>
    /// <param name="id_clinica_vet">Para inserir o id para pode atualizar: </param>
    /// <param name="vetclinica">dados para serem inseridos</param>
    /// <response code="204">Vet Clinica atualizado</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Vet Clinica não encontrado</response>
    /// <returns>Atualizar: </returns>
    [HttpPut]
    [Route("atualizar/vetclinica/{id_clinica_vet:int}")]
    public async Task<IActionResult> PutVetClinica(int id_clinica_vet, VetClinica vetclinica)
    {
        _logger.LogInformation("Iniciando a atualização de vet clínica com ID {IdClinicaVet}", id_clinica_vet);

        if (id_clinica_vet != vetclinica.Id_clinica_vet)
        {
            _logger.LogWarning("ID da vet clínica informado na rota é diferente do ID enviado no objeto. ID: {IdClinicaVet}", id_clinica_vet);
            return BadRequest("Id clinica e vet está incorretor");
        }

        try
        {
            var existeClinica = await _context.Clinicas
                .FirstOrDefaultAsync(c => c.Id_clinica == vetclinica.Id_clinica);
            var existeVet = await _context.Veterinarios
                .FirstOrDefaultAsync(c => c.Id_vet == vetclinica.Id_vet);

            if (existeClinica != null && existeVet != null)
            {
                var vetClinicaAtualizado = await _vetClinica.UpdateVetClinicaAsync(id_clinica_vet, vetclinica);

                _context.Entry(vetClinicaAtualizado).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Vet clínica com ID {IdClinicaVet} atualizada com sucesso", id_clinica_vet);
                return Ok(vetClinicaAtualizado);
            }
            else
            {
                _logger.LogWarning("Não foi possível encontrar a clínica ou o veterinário para atualização da vet clínica. ID: {IdClinicaVet}", id_clinica_vet);
                return NotFound("Não foi possivel encontrar os id clinica ou vet.");
            }

        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VetClinicaExists(id_clinica_vet))
            {
                _logger.LogWarning("Vet clínica não encontrada durante a atualização. ID: {IdClinicaVet}", id_clinica_vet);
                return NotFound("Id clinica vet não existe");
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar vet clínica com ID {IdClinicaVet}", id_clinica_vet);
            return BadRequest($"Erro em atualizar: {ex.Message}");
        }
    }
    private bool VetClinicaExists(int id_clinica_vet)
    {
        return _context.VetClinicas.FirstOrDefault(e => e.Id_clinica_vet == id_clinica_vet) != null;
    }

    // POST: api/VetClinica
    /// <summary>
    /// Inserir Vet e clinica
    /// </summary>
    /// <param name="vetclinica">Inserir os dados de vet e clinica: </param>
    /// <response code="201">Vet Clinica criado com sucesso.</response>
    /// <response code="400">Erro na validação.</response>
    /// <returns>Criação: </returns>
    [HttpPost]
    [Route("criar/vetclinica")]
    public async Task<ActionResult<VetClinica>> PostVetClinica(VetClinica vetclinica)
    {
        _logger.LogInformation("Iniciando a criação de vet clínica para a clínica ID {IdClinica} e veterinário ID {IdVet}", vetclinica.Id_clinica, vetclinica.Id_vet);

        try
        {
            var exiteClinica = await _context.Clinicas.FirstOrDefaultAsync(c => c.Id_clinica == vetclinica.Id_clinica);
            var existeVet = await _context.Veterinarios.FirstOrDefaultAsync(v => v.Id_vet == vetclinica.Id_vet);

            if (exiteClinica != null && existeVet != null)
            {
                var vetClinicaCriada = await _vetClinica.CreateVetClinicaAsync(vetclinica);

                _context.VetClinicas.Add(vetClinicaCriada);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Vet clínica criada com sucesso. ID: {IdClinicaVet}", vetclinica.Id_clinica_vet);
                return Ok(vetClinicaCriada);
            }
            else
            {
                _logger.LogWarning("Clínica ou veterinário não encontrado para criação da vet clínica. Clínica ID: {IdClinica}, Veterinário ID: {IdVet}", vetclinica.Id_clinica, vetclinica.Id_vet);
                return BadRequest("Id clinica e vet não encontrado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar vet clínica para a clínica ID {IdClinica} e veterinário ID {IdVet}", vetclinica.Id_clinica, vetclinica.Id_vet);
            return BadRequest($"Erro ao salvar os dados: {ex.Message}");
        }

    }

    // DELETE: api/VetClinica/5

    /// <summary>
    /// Remove dados de vet clinica
    /// </summary>
    /// <param name="id_clinica_vet">Inserir o id para pode deletar: </param>
    /// <response code="204">Vet clinica removido com sucesso.</response>
    /// <response code="404">Vet clinica não encontrado.</response>
    /// <response code="400">Erro ao processar.</response>
    /// <returns>Deletar: </returns>
    [HttpDelete]
    [Route("deleta/vetclinica/{id_clinica_vet:int}")]
    public async Task<IActionResult> DeleteVetClinica(int id_clinica_vet)
    {
        _logger.LogInformation("Iniciando a exclusão da vet clínica com ID {IdClinicaVet}", id_clinica_vet);

        try
        {
            var vetclinica = await _context.VetClinicas.FirstOrDefaultAsync(e => e.Id_clinica_vet == id_clinica_vet);
            
            _context.VetClinicas.Remove(vetclinica);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Vet clínica com ID {IdClinicaVet} excluída com sucesso", id_clinica_vet);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar vet clínica com ID {IdClinicaVet}", id_clinica_vet);
            return BadRequest($"Erro em deletar: {ex.Message}");
        }
    }
}