using System.Collections.Immutable;
using LocationSearch.Domain;

namespace LocationSearch.Infrastructure.Repositories;

public interface IAddressRepository
{
    public Task<ImmutableList<Address>> GetAll();
}