using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace Application.Helpers
{
    public static class DocumentTextExtractor
    {
        /// <summary>
        /// Extract text from DOCX, DOC, or PDF file
        /// </summary>
        public static async Task<string> ExtractTextFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return extension switch
            {
                ".docx" => ExtractTextFromDocx(memoryStream),
                ".doc" => ExtractTextFromDoc(memoryStream),
                ".pdf" => ExtractTextFromPdf(memoryStream),
                _ => throw new NotSupportedException($"File type {extension} is not supported")
            };
        }

        private static string ExtractTextFromDocx(Stream stream)
        {
            var text = new StringBuilder();

            using (var doc = WordprocessingDocument.Open(stream, false))
            {
                var body = doc.MainDocumentPart?.Document?.Body;
                if (body == null)
                {
                    return string.Empty;
                }

                // Extract text from paragraphs
                foreach (var paragraph in body.Descendants<Paragraph>())
                {
                    var paragraphText = paragraph.InnerText;
                    if (!string.IsNullOrWhiteSpace(paragraphText))
                    {
                        text.AppendLine(paragraphText);
                    }
                }

                // Extract text from tables
                foreach (var table in body.Descendants<Table>())
                {
                    foreach (var row in table.Descendants<TableRow>())
                    {
                        var rowText = string.Join(" | ", row.Descendants<TableCell>().Select(c => c.InnerText));
                        if (!string.IsNullOrWhiteSpace(rowText))
                        {
                            text.AppendLine(rowText);
                        }
                    }
                }
            }

            return text.ToString();
        }

        private static string ExtractTextFromDoc(Stream stream)
        {
            // DOC format is binary and complex, for basic support we can try to read it as DOCX
            // or return a message that DOC needs conversion
            // For production, you might want to use a more robust library or convert DOC to DOCX first
            try
            {
                return ExtractTextFromDocx(stream);
            }
            catch
            {
                throw new NotSupportedException("DOC format is not fully supported. Please convert to DOCX or PDF.");
            }
        }

        private static string ExtractTextFromPdf(Stream stream)
        {
            var text = new StringBuilder();

            using (var document = PdfDocument.Open(stream))
            {
                foreach (var page in document.GetPages())
                {
                    var pageText = page.Text;
                    if (!string.IsNullOrWhiteSpace(pageText))
                    {
                        text.AppendLine(pageText);
                    }
                }
            }

            return text.ToString();
        }
    }
}

