using BE_TravelDestinations.Data;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
                return Ok(destinations);
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
            public IActionResult AddDestinations([FromBody] Destinations destinations)
            {
                _destinationsServices.AddDestinations(destinations);
                return CreatedAtAction(nameof(GetDestinationsById), new { id = destinations.Id }, destinations);
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

