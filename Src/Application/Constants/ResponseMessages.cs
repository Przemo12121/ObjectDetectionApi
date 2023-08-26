using System.Diagnostics;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;

namespace Application.Constants;

public static class ResponseMessages
{
    public static class Errors
    {
        public static string FileNotFound(Guid id, Type type)
            => $"Resource of type: {MapType(type)} with Id: {id} was not found";
        
        public static string ActionForbidden(Guid id, Type type)
            => $"Action on file of type: {MapType(type)} with Id: {id} is not permitted. Ask file's owner for acquiring required permissions.";
    }
    
    public static class Successes
    {
        public const string FileUploaded = "File successfully uploaded. The processing should begin soon.";
        
        public static string FileDeleted(Guid id, Type type)
            => $"Resource of type: {MapType(type)} with Id: {id} was deleted."
        ;
        public static string FileUpdated(Guid id, Type type)
            => $"Resource of type: {MapType(type)} with Id: ${id} was updated.";
    }
    
    private static string MapType(Type type)
        => type switch
        {
            { } when type == typeof(OriginalFile) => "'Original File'",
            { } when type == typeof(ProcessedFile) => "'Processed File'",
            _ => throw new UnreachableException($"Unhandled mapping type of ${nameof(type)}.")
        };
}