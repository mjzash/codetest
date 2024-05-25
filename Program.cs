var logger = new Logger();
var fileprocessor = new FileProcessor(logger);

string[] argz = Environment.GetCommandLineArgs();

string archiveName = argz.Count() == 2 ? argz[1] : "sample.zip";
var defaultExtensions = "pdf;xls;xlsx;doc;docx;msg;png";

var fpOptions = new FileProcessorOptions() {
    ArchiveName = archiveName,
    IndexFileName = "party.xml",
    NameXmlPath = "party/name",
    EmailXmlPath = "party/email",
    ApplicationNoPath = "party/applicationno",
    Extensions = defaultExtensions.Split(";").ToList(),
    BasePath = "processed/"
};

if( fileprocessor.ProcessArchive(fpOptions) ) {
    logger.Info($"Successfully processed '{archiveName}'.");
} else {
    logger.Error($"Failed to process {archiveName}. Check earlier messages for details.");
}