using Domain.AggregateModels.OriginalFileAggregate;

namespace AiService.ProcessingHandlers;

public interface IProcessingHandler
{
    void BeginProcessing(OriginalFile file);
    void StopProcessing(OriginalFile file);
}