using System.IO.Compression;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Configuration;

public class FileProcessorOptions
{
    public string ArchiveName { get; set; } = String.Empty;
    public string IndexFileName { get; set; } = String.Empty;
    public string NameXmlPath { get; set; } = String.Empty;
    public string EmailXmlPath { get; set; } = String.Empty;
    public string ApplicationNoPath { get; set; } = String.Empty;
    public List<string> Extensions { get; set; } = new();
    public string BasePath { get; set; } = String.Empty;
    public string OutputPath { get; set; } = String.Empty;
}

class FileProcessor
{
    ILogger logger;
    IConfiguration configuration;
    IEmailService emailService;

    public FileProcessor(ILogger logger, IEmailService emailService, IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.emailService = emailService;
    }

    public bool ProcessArchive(FileProcessorOptions options)
    {
        bool IsProcessed = false;
        logger.Info($"Attempting to process '{options.ArchiveName}'");
        try {
            ZipArchive archive = ZipFile.OpenRead(Path.GetFullPath(options.ArchiveName));
            var indexfile = archive.Entries.ToList().Find(e => string.Compare(e.FullName, options.IndexFileName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if( indexfile != null ) {
                var stream = indexfile.Open();
                var xdoc = XDocument.Load(stream);

                var name = xdoc.XPathSelectElement(options.NameXmlPath)?.Value ?? String.Empty;
                var email = xdoc.XPathSelectElement(options.EmailXmlPath)?.Value ?? String.Empty;
                var appno = xdoc.XPathSelectElement(options.ApplicationNoPath)?.Value ?? String.Empty;
                if( appno != "" ) {
                    options.OutputPath = Path.Combine(options.BasePath, $"{appno}-{Guid.NewGuid()}");
                } else {
                    throw new Exception($"Missing field '{options.ApplicationNoPath}' in '{options.IndexFileName}'");
                }
                
                var validExtensions = options.Extensions.Select(ext => "." + ext.ToLower());
                foreach(var e in archive.Entries) {
                    if( e != indexfile) {
                        if( ! validExtensions.Contains(Path.GetExtension(e.FullName))) {                            
                            throw new Exception($"Archive entry '{e.FullName}' has a bad extension");
                        }
                    }
                }
                archive.ExtractToDirectory(options.OutputPath, true);
                archive.Dispose();
                IsProcessed = true;
            } else {
                throw new Exception($"Missing index file {options.IndexFileName}.");
            }
        } catch(Exception e) {
            var msg = $"Error occurred while processing archive '{options.ArchiveName}': {e.Message}";
            logger.Error(msg);
            var to = configuration["email:to"] ?? String.Empty;
            var from = configuration["email:from"] ?? String.Empty;
            var subject = configuration["email:subject"] ?? String.Empty;
            if( to != "" && from != "" && subject != "" ) {
                emailService.Email(to, from, subject, msg);
            } else {
                // is a fallback required?
            }
        }

        return IsProcessed;
    }
}