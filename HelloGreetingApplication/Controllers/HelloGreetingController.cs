using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog.Web;
using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using Middleware.GlobalException;
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
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
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
            catch(KeyNotFoundException ex)
            {
                _logger.LogError("Country is already present " + ex);
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
            }

            
        }

        

        /// <summary>
        /// Update the Greeting message
        /// </summary>
        [HttpPut]
        public IActionResult Put(UpdateGreetingModel updateGreetingModel)
        {
            _logger.LogInformation("Update the data base.");

            ResponseModel<string> responseModel = new ResponseModel<string>();

            try
            {
                var result = _GreetingBL.UpdateGreeting(updateGreetingModel);
                
                responseModel.Success = true;
                responseModel.Message = "Update successfull";
                responseModel.Data = result;

                return Ok(responseModel);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
            }

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

        /// <summary>
        /// Greet User According To Name
        /// </summary>
        /// <param name="responseClass"></param>
        /// <returns></returns>
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
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Method to save the Greeting Message.
        /// </summary>
        /// <param name="greetingModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveGreeting")]
        public IActionResult SaveGreeting(GreetingModel greetingModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();

            try
            {
                var result = _GreetingBL.SavedGreeting(greetingModel);
                responseModel.Success = true;
                responseModel.Message = "Operation held.";
                responseModel.Data = result;

                return Ok(responseModel);
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);

                return StatusCode(500, errorResponse);
            }

            
        }

        /// <summary>
        /// Checking the greeting by id
        /// </summary>
        /// <param name="checkGreetingModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckGreeting")]

        public IActionResult CheckGreeting(CheckGreetingModel checkGreetingModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();

            try
            {
                var result = _GreetingBL.CheckGreeting(checkGreetingModel);
                responseModel.Success = true;
                responseModel.Message = "Operation held.";
                responseModel.Data = $"Id : {result.id}, GreetingMessage {result.greetingMessage}";

                return Ok(responseModel);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Greeting not present.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
            }
            catch(ArgumentException ex)
            {
                _logger.LogError("Invalid greeting.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Show all greetings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ListOfGreeting")]

        public IActionResult ListOfGreeting()
        {
            ResponseModel<List<GreetingEntity>> responseModel = new ResponseModel<List<GreetingEntity>>();
            try
            {
                _logger.LogInformation("Data of Greeting");
                var result = _GreetingBL.ListGreetingMessage();

                responseModel.Success = true;
                responseModel.Message = "All Greeting present in database.";
                responseModel.Data = result;

                return Ok(responseModel);


            }
            catch(Exception ex)
            {
                _logger.LogError("Some Error Occurred" + ex);
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Delete the Greeting.
        /// </summary>
        [HttpDelete]
        public IActionResult Delete(CheckGreetingModel checkGreetingModel)
        {
            _logger.LogInformation("Delete the Greeting");
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var result = _GreetingBL.DeleteGreeting(checkGreetingModel);

                responseModel.Success = true;
                responseModel.Message = result;
                return Ok(responseModel);

            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Greeting does not present");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return NotFound(errorResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError("Error occurred " + ex);
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return NotFound(errorResponse);

            }


        }
    }
}
