using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace langchainPDF.Controllers;

[ApiController]
[Route("api/PDF")]
public class PdfController : ControllerBase
{
    private readonly ILogger<PdfController> _logger;

    public PdfController(ILogger<PdfController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<Boolean> UploadPdf(IFormFile file)
    {
        _logger.LogInformation($"Received file: {file.FileName}");
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var pdfReader = new PdfReader(memoryStream);
        using var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader);
        var text = string.Empty;

        for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
            var page = pdfDocument.GetPage(i);
            var textListener = new LocationTextExtractionStrategy();
            var textFromPage = PdfTextExtractor.GetTextFromPage(page, textListener);
            text += textFromPage;
        }
        _logger.LogInformation(text);
        return true;
        // Handle PDF file upload and processing as described in the previous answers
    }
}
