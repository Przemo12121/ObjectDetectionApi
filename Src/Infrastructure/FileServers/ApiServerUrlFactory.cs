using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Infrastructure.FileServers;

public class ApiServerUrlFactory : IFileServerUrlFactory
{
    private readonly string _apiUrl;

    public ApiServerUrlFactory(string apiUrl)
        => _apiUrl = apiUrl;

    public string Create(UniqueEntity file)
        => $"{_apiUrl}/files/processed/{file.Id}";
}