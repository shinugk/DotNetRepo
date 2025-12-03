//This is the file with only top-level statements

using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

public class OpenAIDemo()
{
    public static void Run()  //If it is static method we can directly call this method using classname.method in other class
    {
        var endpoint = new Uri("https://models.github.ai/inference");
        var credential = "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"; //System.Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        var model = "openai/gpt-4.1";

        var openAIOptions = new OpenAIClientOptions()
        {
            Endpoint = endpoint
        };

        var client = new ChatClient(model, new ApiKeyCredential(credential), openAIOptions);

        List<ChatMessage> messages = new List<ChatMessage>()
        {
            new SystemChatMessage("You are a helpful assistant."),
            new UserChatMessage("What is capital of india"),
        };

        var requestOptions = new ChatCompletionOptions()
        {
            Temperature = 1.0f,
            TopP = 1.0f,
        };

        var response = client.CompleteChat(messages, requestOptions);
        System.Console.WriteLine(response.Value.Content[0].Text);
    }

}


