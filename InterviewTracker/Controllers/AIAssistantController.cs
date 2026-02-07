using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace InterviewTracker.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIAssistantController : Controller
    {
        private readonly ChatClient _chatClient;

        public AIAssistantController()
        {
            var endpoint = new Uri("https://models.github.ai/inference");
            var apiKey =Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            var options = new OpenAIClientOptions
            {
                Endpoint = endpoint
            };

            _chatClient = new ChatClient(
                "openai/gpt-4.1",
                new ApiKeyCredential(apiKey),
                options);
        }

        [HttpPost("employer-insight")]
        public IActionResult GetEmployerInsight([FromBody] AiEmployerRequest request)
        {
            var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                "You are a career and interview assistant. " +
                "Give clear, practical advice based on employer details."
            ),

            new UserChatMessage($"""
                Employer Details:
                Company: {request.CompanyName}
                Role: {request.OfferedRole}
                Status: {request.InterviewStatus}
                CTC: {request.CtcOffered}
                Location: {request.Location}

                User Question:
                {request.UserQuestion}
            """)
        };

            var response = _chatClient.CompleteChat(
                messages,
                new ChatCompletionOptions
                {
                    Temperature = 0.7f
                });

            return Ok(new AiEmployerResponse
            {
                Answer = response.Value.Content[0].Text
            });
        }
    }

    public class AiEmployerRequest
    {
        public string CompanyName { get; set; }
        public string OfferedRole { get; set; }
        public string InterviewStatus { get; set; }
        public decimal? CtcOffered { get; set; }
        public string Location { get; set; }
        public string UserQuestion { get; set; }
    }

    public class AiEmployerResponse
    {
        public string Answer { get; set; }
    }

}
