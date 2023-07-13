using Domain.AggregateModels;

namespace Domain.SeedWork.Interfaces;

public interface IServable
{
    ServeData ServeData { get; }
}