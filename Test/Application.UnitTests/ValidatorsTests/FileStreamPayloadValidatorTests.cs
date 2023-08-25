namespace Application.UnitTests.ValidatorsTests;

public class FileStreamPayloadValidatorTests
{
    private readonly FileStreamPayloadValidator _payloadValidator = new();

    [Fact]
    public void Validate_ReturnsSuccess_GivenAllowedMimeTypes()
    {
        using var stream = new FileStream(GetFilePath("TestImage.png"), FileMode.Open, FileAccess.Read);
        FileStreamPayload payload = new(stream, "TestImage.png") { MimeTypes = new() { "image", "video" }};

        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenNonAllowedMimeType()
    {
        using var stream = new FileStream(GetFilePath("TestText.txt"), FileMode.Open, FileAccess.Read);
        FileStreamPayload payload = new(stream, "TestText.txt") { MimeTypes = new() { "image", "text" }};

        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenEmptyFileName()
    {
        using var stream = new FileStream(GetFilePath("TestVideo.mp4"), FileMode.Open, FileAccess.Read);
        FileStreamPayload payload = new(stream, String.Empty) { MimeTypes = new() { "image", "video" }};

        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenEmptyMimeTypes()
    {
        using var stream = new FileStream(GetFilePath("TestVideo.mp4"), FileMode.Open, FileAccess.Read);
        FileStreamPayload payload = new(stream, String.Empty) { MimeTypes = new()};

        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }

    private static string GetFilePath(string fileName)
        => Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, 
            "ValidatorsTests",
            "TestFiles",
            fileName);
}