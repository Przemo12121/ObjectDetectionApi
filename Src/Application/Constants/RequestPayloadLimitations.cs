namespace Application.Constants;

public static class RequestPayloadLimitations
{
    public static class FilePagination
    {
        public const int LimitMaximum = 100;
        public const int LimitMinimum = 1;
        public const int OffsetMinimum = 0;
        public const string OrderRegex = "^((date|name):(asc|desc)){1}(,(date|name):(asc|desc)){0,1}$";
    }
    
    public static class FileUpload
    {
        public static readonly IReadOnlyList<string> AllowedMimes = new List<string> { "image", "video" };
        public const int MaxByteSize = 10000000;
    }
}