using FluentValidation.AspNetCore;
using LocationSearch.Application.Search;
using LocationSearch.WebApi.Models;
using LocationSearch.WebApi.Validation;
using Microsoft.AspNetCore.Mvc;

namespace LocationSearch.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchLocationWithinDistance _searchLocationWithinDistance;

    public SearchController(ISearchLocationWithinDistance searchLocationWithinDistance)
    {
        _searchLocationWithinDistance = searchLocationWithinDistance;
    }

    [HttpGet]
    public async Task<IActionResult> Search(double latitude, double longitude, int maxDistance, int maxResults)
    {
        var model = new SearchRequestModel(latitude, longitude, maxDistance, maxResults);
        var validation = await new SearchRequestModelValidator().ValidateAsync(model);
        validation.AddToModelState(ModelState, null);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _searchLocationWithinDistance.Search(latitude, longitude, maxDistance, maxResults));
    }

    [HttpPost]
    public Task<SearchLocationResult> Search(SearchRequestModel model)
    {
        return _searchLocationWithinDistance.Search(model.Latitude, model.Longitude, model.MaxDistance,
            model.MaxResults);
    }
}