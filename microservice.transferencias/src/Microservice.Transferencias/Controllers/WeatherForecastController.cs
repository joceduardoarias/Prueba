using CuentaServiceConsulta;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using Microservice.Transferencias.Wcf;
using TransfServicesTransf;
using wsHeaderRequest = CuentaServiceConsulta.wsHeaderRequest;
using System.Xml;

namespace Microservice.Transferencias.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            //Parametrizar la url de conexion
            getCuentaPorCBUResponse1 ctaDestino;
            using (var svc = new WSCuentRestriccionTransferenciaServicesClient())
            {
                if (svc.Endpoint.Binding is HttpBindingBase httpBinding)
                {
                    httpBinding.UseDefaultWebProxy = false;
                }
                svc.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
                {
                    CertificateValidationMode = X509CertificateValidationMode.None,
                    RevocationMode = X509RevocationMode.NoCheck
                };
                svc.AddSecurityMessageBehavior("CUENTASERVICES_BSF", "Lri7GH*XE*SgyMDD9V1o", _logger);

                ctaDestino = await svc.getCuentaPorCBUAsync(new wsGetCuentaPorCBURequest
                {
                    cabecera = new wsHeaderRequest
                    {
                        canal = "PD",
                        idEntidad = "0071",
                        idRequerimiento = Guid.NewGuid().ToString("N"),
                        ipCliente = "127.0.0.1",
                        timeStamp = DateTime.Now.ToString("yyyyMMddHHmmsszzz")
                    },
                    cbu = "3300000620000040988100", //cbu destino
                    datosTarjeta = new wsParametrosTarjetaRequest
                    {
                        fiidEmisorEntidad = "0071",
                        numeroTarjeta = "5046200718000000"
                    },
                    fiidPagador = "0071",
                    ipOrigen = "127.0.0.1",
                    moneda = "032",
                    tipoTerminal = "C5"
                });
            }

            var nodes = ((XmlNode[])ctaDestino.respuestaGetCuentaPorCBU.cuenta).ToList();

            //var nodes = ((XmlNode[])innerResponse.cuenta).ToList();
            //cuenta.Estado = (AccountStatusCodeEnum)int.Parse(nodes.First(x => x.Name == "estado").InnerText);
            //cuenta.TipoCuenta = nodes.First(x => x.Name == "tipoCuenta").InnerText;
            //cuenta.NumeroCuentaPBF = nodes.First(x => x.Name == "numeroCuentaPBF").InnerText;
            //cuenta.TipoPersona = nodes.First(x => x.Name == "tipoPersona").InnerText.Equals("F") ? TipoPersonaEnum.Fisica : TipoPersonaEnum.Juridica;
            //cuenta.NumeroCuentaBancaria = nodes.First(x => x.Name == "numeroCuentaBancaria").InnerText;

            //var titulares = nodes.First(x => x.Name == "titulares").ChildNodes;

            //foreach (XmlNode titular in titulares)
            //{
            //    cuenta.Titulares.Add(new TitularDto
            //    {
            //        Denominacion = titular["denominacion"]?.InnerText,
            //        IdTributario = titular["idTributario"]?.InnerText
            //    });
            //}

            //var entidadBancaria = nodes.First(x => x.Name == "ns4:entidadBancaria");

            //cuenta.EntidadBancaria.FIID = entidadBancaria["fiid"]?.InnerText;
            //cuenta.EntidadBancaria.Nombre = entidadBancaria["nombre"]?.InnerText;

            //var red = entidadBancaria["ns4:tipoRedBancaria"];
            //cuenta.EntidadBancaria.TipoRedBancaria.Codigo = red?["codigo"]?.InnerText;
            //cuenta.EntidadBancaria.TipoRedBancaria.Descripcion = red?["descripcion"]?.InnerText;
            //cuenta.EntidadBancaria.TipoRedBancaria.DescripcionAbreviada = red?["descripcionAbreviada"]?.InnerText;

            // Parametrizar la url de conexion
            //using (var svc2 = new TransfServicesTransf.WSTransfServiceClient())
            //{
            //    await svc2.realizarTransferenciaAsync(new wsTransferenciaRequest
            //    {
            //        cabeceraRequest = new TransfServicesTransf.wsHeaderRequest
            //        {

            //        },
            //        datosTransferencia = new wsParametrosTransferenciaRequest
            //        {
            //            cuentaOrigen = new wsParametrosCuentaOrigen
            //            {
            //                cuentaPBF = "",
            //                denominacion = "",
            //                idTributario = "",
            //                tipoCuenta = ""
            //            },
            //            numeroTarjeta = "?",
            //            descripcionBreve = "",
            //            enviaMailDestinatario = "",
            //            importe = "",
            //            monedaTransaccion = "032",
            //            tipoTerminal = "VAR",
            //            cuentaDestino = new wsParametrosCuentaDestino
            //            {
            //                // tipoCuenta = ctaDestino.respuestaGetCuentaPorCBU.cuenta
            //            }
            //        }
            //    });

            //}

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}