using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Infrastructure.FileServers;

public interface IFileServerUrlFactory
{
    string Create(UniqueEntity file);
}