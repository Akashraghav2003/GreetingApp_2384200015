using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog.Web;
using BusinessLayer.Interface;
namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// Class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _GreetingBL;
        private readonly ILogger<HelloGreetingController> _logger;
        private readonly DictionaryForMethod _countryDictionary;

        public HelloGreetingController(IGreetingBL GreetingBL,ILogger<HelloGreetingController> logger, DictionaryForMethod countryDictionary)
        {
            _GreetingBL = GreetingBL;
            _logger = logger;
            _countryDictionary = countryDictionary;
        }

        /// <summary>
        /// Get Method to get the greeting message.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Executing GET method.");
            try
            {
                ResponseModel<string> responseModel = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Hello to Greeting App API Endpoint",
                    Data = "Hello, World"
                };
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GET method: {ex}");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// Post Method to return the key and value.
        /// </summary>
        [HttpPost("Request")]
        public IActionResult Post(RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Request received successfully",
                Data = $"Key: {requestModel.key}, Value: {requestModel.value}"
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Add a new country.
        /// </summary>
        [HttpPost("AddCountry")]
        public IActionResult AddCountry([FromBody] Dictionary<string, long> newCountry)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                foreach (var entry in newCountry)
                {
                    if (!_countryDictionary.AddCountry(entry.Key, entry.Value))
                    {
                        _logger.LogError($"Country '{entry.Key}' already exists.");
                        responseModel.Success = false;
                        responseModel.Message = "Country is already present.";
                        return Conflict(responseModel);
                    }
                }
                responseModel.Success = true;
                responseModel.Message = "New country added successfully.";
                return Created("", responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Country is already present " + ex);
                return Conflict(responseModel);
            }

            
        }

        /// <summary>
        /// Delete a country.
        /// </summary>
        [HttpDelete("{countryName}")]
        public IActionResult Delete(string countryName)
        {
            _logger.LogInformation($"Executing DELETE for country: {countryName}");
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                if (_countryDictionary.DeleteCountry(countryName))
                {
                    responseModel.Success = true;
                    responseModel.Message = "Country deleted successfully.";
                    return Ok(responseModel);
                }
                _logger.LogError($"Country '{countryName}' not found.");
                responseModel.Success = false;
                responseModel.Message = "Country is not present.";
                return NotFound(responseModel);
            }

            catch (Exception ex)
            {
                _logger.LogError("Error occurred " + ex);
                return NotFound(responseModel);

            }

            
        }

        /// <summary>
        /// Update population using PUT (full update).
        /// </summary>
        [HttpPut("{key}/{value}")]
        public IActionResult Put(string key, long value)
        {
            _logger.LogInformation($"Executing PUT to update {key} with population {value}");

            bool isUpdated = _countryDictionary.UpdatePopulation(key, value);

            if (!isUpdated)
            {
                _logger.LogError($"Country '{key}' not found.");
                return NotFound(new { Message = "Country not found!" });
            }

            return Ok(new { Message = "Population updated successfully!" });
        }

        /// <summary>
        /// Update population using PATCH (partial update).
        /// </summary>
        [HttpPatch]
        public IActionResult Patch([FromBody] Dictionary<string, long> updateData)
        {
            if (updateData == null || !updateData.ContainsKey("key") || !updateData.ContainsKey("value"))
            {
                return BadRequest(new { Message = "Invalid request! 'key' (country name) and 'value' (population) are required." });
            }

            string key = updateData["key"].ToString();
            long value = updateData["value"];

            _logger.LogInformation($"Executing PATCH to update {key} with population {value}");

            bool isUpdated = _countryDictionary.UpdatePopulation(key, value);

            if (!isUpdated)
            {
                _logger.LogError($"Country '{key}' not found.");
                return NotFound(new { Message = "Country not found!" });
            }

            return Ok(new { Message = "Population updated successfully!", Data = new { key, value } });
        }
        /// <summary>
        /// Greeting app greet using Services layer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("helloGreet")]
        public IActionResult GetGreeting()
        {
            _logger.LogInformation("Greeting by services.");
            return Ok(_GreetingBL.GetGreeting());
        }

        [HttpPost]
        [Route("GreetingMessage")]

        public IActionResult Greetingmessage(ResponseClass responseClass)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                if (responseClass == null)
                {
                    _logger.LogWarning("Data not found ");
                    return NoContent();
                }

                var result = _GreetingBL.GreetMessage(responseClass.firstName, responseClass.lastName);

                responseModel.Success = true;
                responseModel.Message = "Greet Message";
                responseModel.Data = result;
                return Ok(responseModel);

            }catch(Exception ex)
            {
                _logger.LogError("error occured" + ex);
                return StatusCode(500, new { error = "An error Occurred ", details = ex.Message });
            }
        }
    }
}
