using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SemanticFunctions;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Memory;
namespace langchainPDF.Controllers;

[ApiController]
[Route("api/PDF")]
public class PdfController : ControllerBase
{
    private readonly ILogger<PdfController> _logger;
    private IKernel _kernel;
    public PdfController(ILogger<PdfController> logger,IKernel kernel)
    {
        _logger = logger;
        _kernel=kernel;

    }

    public static List<string> CharacterTextSplitter(
        string text,
        int chunkSize,
        int chunkOverlap,
        string separator = "\n"
    )
    {
        List<string> chunks = new List<string>();
        int startIndex = 0;

        while (startIndex < text.Length)
        {
            int endIndex = Math.Min(startIndex + chunkSize, text.Length);
            string chunk = text.Substring(startIndex, endIndex - startIndex);
            chunks.Add(chunk);

            startIndex += chunkSize - chunkOverlap;
        }

        return chunks;
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
        // _logger.LogInformation(text);
        List<string> chunks = CharacterTextSplitter(text, 1000, 200);
        int chunkNum=0;
        foreach (string chunk in chunks)
        {
            Console.WriteLine("-----chunk "+chunkNum.ToString()+" ------------");
            Console.WriteLine(chunk);
            chunkNum++;
        }
        
        return true;

        // Handle PDF file upload and processing as described in the previous answers
    }
}
