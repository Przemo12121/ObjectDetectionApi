using Domain.SeedWork.Enums;

namespace Application.Constants;

public static class RequestPayloadDefaults
{
    public static class FilePagination
    {
        public const int Limit = 20;
        public const int Offset = 0;
        public const string Order = "date:desc";
        public static readonly QueryMediaTypes QueryMediaTypes = QueryMediaTypes.All;
    }
    
    public static class UpdateProcessedFileViewers
    {
        public static readonly IReadOnlyList<string> ViewersEmails = Enumerable.Empty<string>().ToList();
    }
}