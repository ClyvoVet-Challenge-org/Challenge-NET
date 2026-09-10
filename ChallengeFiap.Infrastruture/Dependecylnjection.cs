using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using challengeFiap.Application.Diagnostics;
using challengeFiap.Application.Service;
using challengeFiap.Domain.Interfaces;

namespace challengeFiap.Infrastruture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAnimalService, AnimalService>();
            services.AddScoped<ICarteiraVacinalService, CateiraVacinalService>();
            services.AddScoped<IConsultaService, ConsultaService>();
            services.AddScoped<IClinicaService, ClinicaService>();
            services.AddScoped<IenderecoAnimalService, EnderecoAnimalService>();
            services.AddScoped<IenderecoClinicaService, EnderecoClinicaService>();
            services.AddScoped<IenderecoTutorService, EnderecoTutorService>();
            services.AddScoped<IMedicamentoService, MedicamentoService>();
            services.AddScoped<IprescricaoService, PrescricaoService>();
            services.AddScoped<ItutorService, TutorService>();
            services.AddScoped<IVetClinica, VetClinicasService>();
            services.AddScoped<IVeterinarioService, VeterinariosService>();

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(TelemetryConstants.ServiceName);

            // Configuração do OpenTelemetry
            services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSource(TelemetryConstants.ServiceName)
                        .AddConsoleExporter();
                })
                .WithMetrics(meterProviderBuilder =>
                {
                    meterProviderBuilder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddMeter(TelemetryConstants.MeterName)
                        .AddConsoleExporter();
                });

            return services;
        }
    }
}