using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.SemanticKernel.Skills.Core;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Connectors.Memory.Postgres;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.TemplateEngine;
using Microsoft.SemanticKernel.Text;
namespace langchainPDF.Controllers;

[ApiController]
[Route("api/PDF")]
public class PdfController : ControllerBase
{
    private readonly ILogger<PdfController> _logger;
    private IKernel _kernel;
    private SKContext _context;
    private PromptTemplateEngine _template;
    private IChatCompletion GPT;
    private string sysPrompt;
    string fileName;
    public PdfController(
        ILogger<PdfController> logger,
        IKernel kernel,
        SKContext context,
        IChatCompletion chatGPT,
        PromptTemplateEngine templateEngine
    )
    {
        _context=context;

        _logger = logger;
        _kernel = kernel;
        _template=templateEngine;
        GPT=chatGPT;
        sysPrompt=@"You are an AI assistant that helps people find information based on the user inputted context chunks
Text selected:
{{ $selectedText }}";

        
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

    [HttpPost("UploadPdf")]
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
        int chunkNum = 0;
        foreach (string chunk in chunks)
        {

            string memCollectionName = "file1";
            string idString = "info " + chunkNum.ToString();
            var embed=await _kernel.Memory.SaveInformationAsync(memCollectionName, id: idString, text: chunk);
            chunkNum++;
        }
        fileName=file.FileName;

        return true;
    }



    [HttpPost("AskQuestion")]
    public async Task<Prompt> AskQuestion([FromBody] Prompt data){
        string question=data.text;
        var memories=_kernel.Memory.SearchAsync("file1",question,minRelevanceScore:0.7,limit:10); 
        int result = 0;
        string results="-------------------------------------------------"+"\n";
        await foreach (MemoryQueryResult memory in memories)
        {
            results+=$"Result {++result}"+"\n";
            results+="-------------------------------------------------"+"\n";
            results+=memory.Metadata.Text+"\n";
            results+="-------------------------------------------------"+"\n";
            

        }
        _context["selectedText"]=results;
        string systemMessage = await _template.RenderAsync(sysPrompt, _context);
        var chatHistory=GPT.CreateNewChat(systemMessage);
        chatHistory.AddUserMessage(question);
        string answer = await GPT.GenerateMessageAsync(chatHistory);
        Prompt chatResponse=new Prompt();
        chatResponse.text=answer;
        chatResponse.role="BOT";


        return chatResponse;
    }
    public struct Prompt{
        public string text {set;get;}
        public string role {set;get;}
        
    }
    
}
