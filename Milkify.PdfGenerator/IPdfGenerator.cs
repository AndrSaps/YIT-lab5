using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Milkify.PdfGenerator
{
    public interface IPdfGenerator
    {
        Stream GenerateTablePdf(string header, PdfPTable headerTable, PdfPTable table);
    }
    public class PdfGenerator: IPdfGenerator
    {
        public Stream GenerateTablePdf(string header, PdfPTable headerTable, PdfPTable table)
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            var headerParagraph = new Paragraph(header, new Font(BaseFont.CreateFont(), 32.0f));
            headerParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(headerParagraph);
            if (headerTable != null)
            {
                document.Add(headerTable);
            }
            document.Add(table);
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;
        }
    }
}
