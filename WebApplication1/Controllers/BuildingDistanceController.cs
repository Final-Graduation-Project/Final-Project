using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WebApplication1.Services.Dijkstra;


[Route("api/building-distance/[controller]")]
[ApiController]

public class BuildingDistanceController : ControllerBase
{
    private readonly IDijkstraService _dijkstraService;

    public BuildingDistanceController([FromServices] IDijkstraService dijkstraService)
    {
        _dijkstraService = dijkstraService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<DistanceResult>> GetDistance([FromQuery] string from, [FromQuery] string to)
    {
        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
        {
            return BadRequest("Both 'from' and 'to' parameters are required.");
        }

        try
        {
            var paths = _dijkstraService.CalculateShortestPath(from, to);

            if (!paths.ContainsKey(to))
            {
                return NotFound($"No path found from '{from}' to '{to}'.");
            }

            var shortestPath = paths[to];
            var distanceResult = new DistanceResult
            {
                From = from,
                To = to,
                Distance = shortestPath.Item1, 
                Path = shortestPath.Item2      
            };

            return Ok(distanceResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while calculating distance: {ex.Message}");
        }
    }

        
}

public class DistanceResult
{
    public string From { get; set; }
    public string To { get; set; }
    public double Distance { get; set; }
    public List<string> Path { get; set; } 
}
