using System;

namespace DataInjector
{
    public class DocumentInformation
    {
        public DataHandlerService.FileType FileType { get; set; }
        public Byte[] Document { get; set; }
        public String Content { get; set; }
        public OdfMetadata Metadata { get; set; }

        public DocumentInformation(DataHandlerService.FileType fileType, Byte[] document, string content, OdfMetadata metadata)
        {
            FileType = fileType;
            Document = document;
            Content = content;
            Metadata = metadata;
        }
    }
}