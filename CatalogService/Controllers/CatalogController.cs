using Microsoft.AspNetCore.Mvc;
using Shared.Models.DTOs.Common;
using Shared.Models.DTOs.Requests;
using Shared.Models.DTOs.Responses;
using System.Net;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class CatalogController : ControllerBase
{
    // Mock data (in real apps, inject a service/repo)
    private static readonly List<CatalogDto> _catalogs =
    [
        new(1, "Books", "A variety of books."),
        new(2, "Electronics", "Latest electronic gadgets."),
        new(3, "Clothing", "Men's and women's clothing.")
    ];

    /// <summary>
    /// Retrieves all catalogs.
    /// </summary>
    /// <remarks>
    /// Returns a list of all available catalogs.
    /// </remarks>
    /// <returns>A collection of <see cref="CatalogDto"/> objects.</returns>
    /// <response code="200">Returns the list of catalogs</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CatalogDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CatalogDto>>> GetCatalogs()
    {
        await Task.Delay(1000); // Simulated async delay
        return Ok(_catalogs);
    }

    /// <summary>
    /// Retrieves a specific catalog by ID.
    /// </summary>
    /// <param name="id">The ID of the catalog</param>
    /// <returns>A <see cref="CatalogDto"/> object if found</returns>
    /// <response code="200">Returns the catalog</response>
    /// <response code="404">If no catalog is found with the given ID</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CatalogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CatalogDto>> GetCatalog(int id)
    {
        var catalog = _catalogs.FirstOrDefault(c => c.Id == id);

        if (catalog is null)
            return NotFound(new ErrorResponse("Catalog not found", HttpStatusCode.NotFound));

        await Task.Delay(1000); // Simulated async delay
        return Ok(catalog);
    }

    /// <summary>
    /// Create a new catalog.
    /// </summary>
    /// <param name="catalogDto">Catalog to create.</param>
    /// <returns>The created <see cref="CatalogDto"/>.</returns>
    /// <response code="200">Returns the catalog</response>
    /// <response code="400">If any validation fail</response>
    [HttpPost]
    [ProducesResponseType(typeof(CatalogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CatalogDto> CreateCatalog([FromBody] CatalogRequestDto catalogDto)
    {
        var newCatalog = new CatalogDto(_catalogs.Count + 1, catalogDto.Name, catalogDto.Description);

        _catalogs.Add(newCatalog);

        return CreatedAtAction(nameof(GetCatalog), new { id = newCatalog.Id }, newCatalog);
    }
}
