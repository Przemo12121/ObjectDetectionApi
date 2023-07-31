namespace Infrastructure.UnitTests.RepositoriesTests;

public abstract class BaseRepositoryTests
{
    protected ObjectDetectionDbContext DbContext { get; }
    protected List<AccessAccount> AccessAccounts { get; private set; } = null!;
    protected List<OriginalFile> OriginalFiles { get; private set; } = null!;
    protected List<ProcessedFile> ProcessedFiles  { get; private set; } = null!;

    protected BaseRepositoryTests(string databaseName)
    {
        DbContext = new ObjectDetectionDbContext(
            new DbContextOptionsBuilder<ObjectDetectionDbContext>()
                .UseInMemoryDatabase(databaseName + Guid.NewGuid())
                .Options);
        
        Seed();
    }

    private void Seed()
    {
        AccessAccounts = new() 
        {
            "viewer.email@not.connected",
            "first@owner.email",
            "second@owner.email",
            "third@owner.email",
            "reader.email@not.connected",
            "first@reader.email",
            "second@reader.email",
            "third@reader.email",
            "fourth@reader.email",
            "fifth@reader.email"
        };
        OriginalFiles = new()
        {
            new(
                new("a_first_original_file", MediaTypes.Image),
                new(FileStorages.LocalStorage, "sample_uri"),
                AccessAccounts[1]),
            new(
                new("b_second_original_file", MediaTypes.Image),
                new(FileStorages.LocalStorage, "sample_uri_2"),
                AccessAccounts[1]),
            new(
                new("x_third_original_file", MediaTypes.Video),
                new(FileStorages.LocalStorage, "sample_uri_3"),
                AccessAccounts[2]),
            new(
                new("c_fourth_original_file", MediaTypes.Image),
                new(FileStorages.LocalStorage, "sample_uri_4"),
                AccessAccounts[1]),
            new(
                new("c_fourth_original_file", MediaTypes.Video),
                new(FileStorages.LocalStorage, "sample_uri_5"),
                AccessAccounts[1])
        };

        var originalFilesCreationDateTimeSetter = typeof(UniqueEntity)
            .GetProperty(nameof(OriginalFile.CreationDateTime), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
        originalFilesCreationDateTimeSetter.SetValue(OriginalFiles[0], DateTime.UtcNow.AddHours(-1));
        originalFilesCreationDateTimeSetter.SetValue(OriginalFiles[1], OriginalFiles[0].CreationDateTime);
        originalFilesCreationDateTimeSetter.SetValue(OriginalFiles[2], DateTime.UtcNow.AddMinutes(-1));
        originalFilesCreationDateTimeSetter.SetValue(OriginalFiles[3], DateTime.UtcNow.AddMinutes(-10));
        originalFilesCreationDateTimeSetter.SetValue(OriginalFiles[4], DateTime.UtcNow.AddMinutes(-15));
        
        ProcessedFiles = new()
        {
            new(
                AccessAccounts[1],
                new("a_first_processed_file", MediaTypes.Image),
                new(FileStorages.LocalStorage, "sample_uri"),
                new("sample_url"),
                new List<AccessAccount>()),
            new(
                AccessAccounts[3],
                new("b_second_processed_file", MediaTypes.Video),
                new(FileStorages.LocalStorage, "sample_uri_2"),
                new("sample_url_2"),
                new List<AccessAccount>
                {
                    AccessAccounts[5],
                    AccessAccounts[6],
                    AccessAccounts[7],
                    AccessAccounts[8],
                    AccessAccounts[9]
                }),
            new(
                AccessAccounts[3],
                new("c_third_processed_file", MediaTypes.Video),
                new(FileStorages.LocalStorage, "sample_uri_3"),
                new("sample_url_3"),
                new List<AccessAccount>
                {
                    AccessAccounts[1],
                    AccessAccounts[2],
                    AccessAccounts[9]
                }),
            new(
                AccessAccounts[3],
                new("z_fourth_processed_file", MediaTypes.Image),
                new(FileStorages.LocalStorage, "sample_uri_4"),
                new("sample_url_4"),
                new List<AccessAccount>
                {
                    AccessAccounts[5],
                    AccessAccounts[6],
                    AccessAccounts[7]
                }),
            new(
                AccessAccounts[3],
                new("z_fourth_processed_file", MediaTypes.Image),
                new(FileStorages.LocalStorage, "sample_uri_5"),
                new("sample_url_5"),
                new List<AccessAccount>
                {
                    AccessAccounts[2],
                    AccessAccounts[3],
                    AccessAccounts[4]
                }),
        };
        
        var processedFilesCreationDateTimeSetter = typeof(UniqueEntity)
            .GetProperty(nameof(OriginalFile.CreationDateTime), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
        processedFilesCreationDateTimeSetter.SetValue(ProcessedFiles[0], DateTime.UtcNow.AddHours(-1));
        processedFilesCreationDateTimeSetter.SetValue(ProcessedFiles[1], ProcessedFiles[0].CreationDateTime);
        processedFilesCreationDateTimeSetter.SetValue(ProcessedFiles[2], DateTime.UtcNow.AddMinutes(-1));
        processedFilesCreationDateTimeSetter.SetValue(ProcessedFiles[3], DateTime.UtcNow.AddMinutes(-10));
        processedFilesCreationDateTimeSetter.SetValue(ProcessedFiles[4], DateTime.UtcNow.AddMinutes(-15));

        DbContext.AccessAccounts.AddRange(AccessAccounts);
        DbContext.SaveChanges();

        DbContext.OriginalFiles.AddRange(OriginalFiles);
        DbContext.ProcessedFiles.AddRange(ProcessedFiles);
        DbContext.SaveChanges();
    }
}