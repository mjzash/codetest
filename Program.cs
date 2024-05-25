using Microsoft.Extensions.Configuration;

var logger = new Logger();
var fileprocessor = new FileProcessor(logger);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string[] argz = Environment.GetCommandLineArgs();

string archiveName = configuration["defaultArchive"] ?? "sample.zip";
if( argz.Length == 2 ) {
    archiveName = argz[1];
}
var defaultExtensions = "pdf;xls;xlsx;doc;docx;msg;png";

var fpOptions = new FileProcessorOptions() {
    ArchiveName = archiveName,
    IndexFileName = configuration["indexfile"] ?? "party.xml",
    NameXmlPath = configuration["xmlpaths:name"] ?? "party/name",
    EmailXmlPath = configuration["xmlpaths:email"] ?? "party/email",
    ApplicationNoPath = configuration["xmlpaths:applicationno"] ?? "party/applicationno",
    Extensions = (configuration["extensions"] ?? defaultExtensions).Split(";").ToList(),
    BasePath = configuration["basepath"] ?? "processed/"
};

if( fileprocessor.ProcessArchive(fpOptions) ) {
    logger.Info($"Successfully processed '{archiveName}'.");
} else {
    logger.Error($"Failed to process {archiveName}. Check earlier messages for details.");
}