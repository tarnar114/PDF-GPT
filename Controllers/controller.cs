using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.TemplateEngine;
using Microsoft.SemanticKernel.Connectors.Memory.Redis;
using StackExchange.Redis;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.Tokenizers;
using Microsoft.SemanticKernel.Text;


namespace langchainPDF.Controllers;

[ApiController]
[Route("api/PDF")]
public class PdfController : ControllerBase
{
    private readonly ILogger<PdfController> _logger;
    private readonly IMemoryCache _memCache;
    private ConnectionMultiplexer connPlexer;
    private IChatCompletion GPT;
    private string sysPrompt;
    private string sysMessage;
    private string file;
    private ChatHistory hist;

    public PdfController(ILogger<PdfController> logger, IMemoryCache memoryCache)
    {
        _logger = logger;

        _memCache = memoryCache;
    }

    [HttpPost("InitKernel")]
    public async Task<Boolean> InitKernel([FromBody] ApiKey userData)
    {
        string apiKey = userData.Key;
        connPlexer = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
        IDatabase db = connPlexer.GetDatabase();
        RedisMemoryStore memStore = new(db, vectorSize: 1536);

        KernelBuilder builder = new();
        builder.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002", apiKey);
        builder.WithOpenAIChatCompletionService("gpt-3.5-turbo", apiKey);
        IKernel _kernel = builder.Build();
        _kernel.UseMemory(memStore);
        await Task.Delay(5000);

        GPT = _kernel.GetService<IChatCompletion>();
        var context = _kernel.CreateNewContext();
        PromptTemplateEngine _template = new();
        hist=GPT.CreateNewChat();
        _memCache.Set<IKernel>("kernel", _kernel, new DateTimeOffset(DateTime.Now.AddDays(1)));
        _memCache.Set<SKContext>("context", context, new DateTimeOffset(DateTime.Now.AddDays(1)));
        _memCache.Set<PromptTemplateEngine>(
            "template",
            _template,
            new DateTimeOffset(DateTime.Now.AddDays(1))
        );
        _memCache.Set<ChatHistory>("history", hist, new DateTimeOffset(DateTime.Now.AddDays(1)));
        return true;
    }

    [HttpGet("memCheck")]
    public Boolean MemCheck()
    {
        bool memCheck = _memCache.TryGetValue<IKernel>("kernel", out IKernel data);
        if (memCheck == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static List<string> CharacterTextSplitter(string text, int chunkSize, int chunkOverlap)
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
        var _kernel = _memCache.Get<IKernel>("_kernel");
        if (_kernel is null)
            return false;
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var pdfReader = new PdfReader(memoryStream);
        using var pdfDocument = new PdfDocument(pdfReader);
        var text = string.Empty;
        string fileName = file.Name;
        for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
            var page = pdfDocument.GetPage(i);
            var textListener = new LocationTextExtractionStrategy();
            var textFromPage = PdfTextExtractor.GetTextFromPage(page, textListener);
            text += textFromPage;
        }
        Console.WriteLine("PDF Text: \n \n" + text);

        // _logger.LogInformation(text);
        List<string> chunks = CharacterTextSplitter(text, 200, 50);
        int chunkNum = 0;
        foreach (string chunk in chunks)
        {
            string idString = "info " + chunkNum.ToString();
            Console.WriteLine(chunk);
            var embed = await _kernel.Memory.SaveInformationAsync(
                "file1",
                id: idString,
                text: chunk
            );
            chunkNum++;
        }

        return true;
    }

    [HttpPost("AskQuestion")]
    public async Task<Prompt> AskQuestion([FromBody] Prompt data)
    {
        var _kernel = _memCache.Get<IKernel>("kernel");
        var _template = _memCache.Get<PromptTemplateEngine>("template");
        var _context = _memCache.Get<SKContext>("context");
        var hist = _memCache.Get<ChatHistory>("history");
        var GPT = _kernel.GetService<IChatCompletion>();

        string question = data.Text;

        _context.Variables["userMessage"] = question;
        var memories = _kernel.Memory.SearchAsync(
            "file1",
            question,
            minRelevanceScore: 0.6,
            limit: 10 
        );
        int result = 0;
        string results = "-------------------------------------------------" + "\n";
        await foreach (MemoryQueryResult memory in memories)
        {
            results += $"Result {++result}" + "\n";
            results += "-------------------------------------------------" + "\n";
            results += memory.Metadata.Text + "\n";
            results += "-------------------------------------------------" + "\n";
        }
        Console.WriteLine(result.ToString());
        _context.Variables["selectedText"] = results;
        // sysPrompt =
        //     @"You are an AI assistant that helps people find information on a question based on user question and given text information chunks extracted from a PDF
        //     User Question: {{ $userMessage }}
        //     ---------------------------------
        //     Context Chunks:
        //     {{ $selectedText }}";
        var func=_kernel.CreateSemanticFunction(@"You are an AI assistant that helps people find information on a question based on user question and given text information chunks extracted from a PDF
            User Question: {{ $userMessage }}
            ---------------------------------
            Context Chunks:
            {{ $selectedText }}");
        var res=await func.InvokeAsync(_context);
        var answer=res.Result.ToString();
        // string systemMessage=await _template.RenderAsync(sysPrompt,_context);
        // hist.AddSystemMessage(systemMessage);
        // string answer = await GPT.GenerateMessageAsync(hist);
        // hist.AddAssistantMessage(answer);
        // Console.WriteLine(answer);
        Prompt chatResponse = new Prompt();
        chatResponse.Text = answer;
        chatResponse.Role = "BOT";
        return chatResponse;
    }

    public struct Prompt
    {
        public string Text { set; get; }
        public string Role { set; get; }
    }

    public struct ApiKey
    {
        public string Key { set; get; }
    }
}
