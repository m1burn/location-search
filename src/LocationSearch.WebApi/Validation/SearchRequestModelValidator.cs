using FluentValidation;
using LocationSearch.WebApi.Models;

namespace LocationSearch.WebApi.Validation;

public class SearchRequestModelValidator : AbstractValidator<SearchRequestModel> 
{
    public SearchRequestModelValidator() 
    {
        RuleFor(x => x.Latitude).LessThanOrEqualTo(90);
        RuleFor(x => x.Latitude).GreaterThanOrEqualTo(-90);
        RuleFor(x => x.Longitude).LessThanOrEqualTo(180);
        RuleFor(x => x.Longitude).GreaterThanOrEqualTo(-180);
        RuleFor(x => x.MaxDistance).GreaterThan(0);
        RuleFor(x => x.MaxResults).GreaterThan(0);
    }
}