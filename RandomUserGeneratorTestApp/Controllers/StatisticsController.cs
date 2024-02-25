using Microsoft.AspNetCore.Mvc;
using RandomUserGeneratorTestApp.BusinessLogic.Services.Interfaces;
using RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Requests;
using RandomUserGeneratorTestApp.Web;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Serialization;

namespace RandomUserGeneratorTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(ILogger<StatisticsController> logger, IStatisticsService statisticsService)
        {
            _logger = logger;
            _statisticsService = statisticsService;
        }

        [SwaggerRequestExample(typeof(UserRequest[]), typeof(SwaggerExamplesProvider))]
        [HttpPost("[action]")]
        public IActionResult SummaryText(UserRequest[] users)
        {
            if (users == null)
                return BadRequest("At least one user required");

            if (users.Length == 0)
                return BadRequest("At least one user required");

            IActionResult response;

            try 
            {
                var stats = _statisticsService.GetStatsSummaryText(users);

                response = GetResponseFile(stats, "SummaryText");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting summary text (Users: {Users}, ex.Message: {ErrorMessage})", users, ex.Message);
                response = StatusCode(StatusCodes.Status500InternalServerError);
            }

            return response;
        }

        [HttpPost("[action]")]
        public IActionResult StatsData(UserRequest[] users)
        {
            if (users == null)
                return BadRequest("At least one user required");

            if (users.Length == 0)
                return BadRequest("At least one user required");

            IActionResult response;

            try
            {
                var stats = _statisticsService.GetStats(users);

                response = GetResponseFile(stats, "StatsData");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting stats data (Users: {Users}, ex.Message: {ErrorMessage})", users, ex.Message);
                response = StatusCode(StatusCodes.Status500InternalServerError);
            }

            return response;
        }

        /// <summary>
        /// Create a file response for the input data and Accept header
        /// </summary>
        /// <param name="responseData"></param>
        /// <param name="fileNameSuffix"></param>
        /// <returns></returns>
        private FileContentResult GetResponseFile(object responseData, string fileNameSuffix)
        {
            var contentType = "application/octet-stream";
            var fileExtension = "json";
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var fileContentsForType = JsonSerializer.Serialize(responseData, jsonSerializerOptions);

            if (Request.Headers.Accept.ToString().Equals("application/xml", StringComparison.CurrentCultureIgnoreCase))
            {
                fileExtension = "xml";

                using var stringWriter = new StringWriter();
                var serializer = new XmlSerializer(responseData.GetType());
                serializer.Serialize(stringWriter, responseData);
                fileContentsForType = stringWriter.ToString();
            }
            else if (Request.Headers.Accept.ToString().Equals("text/html", StringComparison.CurrentCultureIgnoreCase))
            {
                fileExtension = "txt";
                fileContentsForType = string.Join("\n", responseData);
            }

            var fileContents = Encoding.UTF8.GetBytes(fileContentsForType);

            return File(fileContents, contentType, $"RandomUserAnalysis{fileNameSuffix}.{fileExtension}");
        }
    }
}
