
using BE_TravelDestinations.DataPart;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BE_TravelDestinations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationsServices _destinationsServices;

     
        
        public DestinationsController(IDestinationsServices destinationsServices)
        {
            _destinationsServices = destinationsServices;
        }

        [HttpGet]
        public IActionResult GetAllDestinations()
        {
            var destinations = _destinationsServices.GetAllDestinations();
            var jsonString = JsonSerializer.Serialize(destinations);
            return Ok(jsonString);
        }


        [HttpGet("{id}")]
        public IActionResult GetDestinationsById(int id)
        {
            var destinations = _destinationsServices.GetDestinationsById(id);
            if (destinations == null)
            {
                return NotFound();
            }
            return Ok(destinations);
        }

        [HttpPost]
        public IActionResult AddDestinations([FromBody] JsonElement jsonContent)
        {
            var type = typeof(Destinations); // Determine the type
            if (!DeserializationValidator.IsAllowedType(type))
            {
                return BadRequest("Invalid data type.");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

            try
            {
                var destinations = JsonSerializer.Deserialize<Destinations>(jsonContent.GetRawText(), options);
                if (destinations == null)
                {
                    return BadRequest("Deserialization failed - object was null after deserialization.");
                }
                _destinationsServices.AddDestinations(destinations);
                return CreatedAtAction(nameof(GetDestinationsById), new { id = destinations.Id }, destinations);
            }
            catch (JsonException ex)
            {
                return BadRequest($"Data could not be deserialized: {ex.Message}, {ex.StackTrace}");
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateDestinations(int id, [FromBody] Destinations destinations)
        {
            if (id != destinations.Id)
            {
                return BadRequest();
            }

            var existingDestinations = _destinationsServices.GetDestinationsById(id);
            if (existingDestinations == null)
            {
                return NotFound();
            }

            _destinationsServices.UpdateDestinations(destinations);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDestinations(int id)
        {
            var destinations = _destinationsServices.GetDestinationsById(id);
            if (destinations == null)
            {
                return NotFound();
            }

            _destinationsServices.DeleteDestinations(id);
            return NoContent();
        }
    }
}
