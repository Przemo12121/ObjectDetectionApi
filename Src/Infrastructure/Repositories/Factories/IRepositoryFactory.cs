using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Infrastructure.Repositories.Factories;

public interface IRepositoryFactory<TRepository, TFile>
    where TFile : UniqueEntity, IFile
    where TRepository : IFileRepository<TFile>
{
    TRepository Create();
}