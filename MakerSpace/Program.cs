using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using DotNetEnv;

var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
Env.Load(envPath);

var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
    ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set in .env");
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-5.4-mini";

#region Instructions
var criticalInstructions = @"
You are an AI strategy expert from the year 2031 for technical consultants. 

Analyze consultant's skills and tell with brutal honesty:
* what skills, habits, or systems will be worthless or obsolete in the next five years? 
* What must the person start learning or building right now, so they won't regret it in 5 years?

Please, no flattery.

When the conversation gets heavy or the user asks for humor, you can create a funny poem about their career situation.
";

var poetryInstructions = @"
You are a humorous poet specializing in technology careers and consulting life.
Create witty, rhyming poems that are light-hearted but insightful about the tech industry.
Keep poems fun and relevant to the user's career situation.
";

#endregion

// Create poetry agent for agent-as-tool pattern
AIAgent poetryAgent = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .AsAIAgent(
        instructions: poetryInstructions,
        name: "Tech Poetry Agent");

// Poetry tool function - creates poetry using the poetry agent
async Task<string> CreateHumorousPoem(string topic, string userContext = "")
{
    try
    {
        var prompt = $"Create a humorous, rhyming poem about: {topic}. Context: {userContext}. Make it witty and relevant to tech consulting.";
        var response = await poetryAgent.RunAsync(prompt);
        return $"\n🎭 Here's a humorous poem for you:\n\n{response}\n";
    }
    catch (Exception ex)
    {
        return $"Sorry, I couldn't create a poem right now. Error: {ex.Message}";
    }
}

// Create the Microsoft Consultant Career Coach Agent with tools

var careerCoachInstructions = criticalInstructions;

AIAgent careerCoach = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .AsAIAgent(
        instructions: careerCoachInstructions,
        name: "Microsoft Consultant AI Career Coach",
        tools: [AIFunctionFactory.Create(EmailTools.EmailSkillingPlan), AIFunctionFactory.Create(CreateHumorousPoem)]);

Console.WriteLine("Welcome to your Enhanced Consultant AI Career Coach!");
Console.WriteLine("Let's future-proof your Microsoft consulting career!");
Console.WriteLine();
Console.WriteLine("💡 Pro tip: Ask me to email you a personalized skilling plan after we analyze your profile!");
Console.WriteLine("🎪 Fun feature: I can create humorous poems about your career situation!");
Console.WriteLine();
Console.WriteLine("> Note: Type your message or 'exit' to quit");
Console.WriteLine();

// Collect consultant profile
Console.WriteLine("Give me some context on your profile:");
Console.WriteLine("- What field or profession are you currently in?");
Console.WriteLine("- What are your main skills or strengths right now?");
Console.WriteLine("- What's your typical workday like?");
Console.WriteLine();

Console.Write("You: ");
string userProfile = Console.ReadLine() ?? "";

// Generate comprehensive career analysis
var analysisPrompt = $@"
Analyze this profile and provide feedback. Give concise feedback for each skill/strength mentioned.
---
{userProfile}
---

In the end, if there is room for improvement, mention the need for constant learning and ask the user if they want a 6-month skilling plan to future-proof their career.
";

var analysis = await careerCoach.RunAsync(analysisPrompt);

Console.WriteLine();
Console.WriteLine(new string('=', 80));
Console.WriteLine("YOUR PERSONALIZED CAREER ANALYSIS");
Console.WriteLine(new string('=', 80));
Console.WriteLine(analysis);

Console.WriteLine();

// Create Agent Session to keep context across multiple interactions
var session = await careerCoach.CreateSessionAsync();

// Add initial context to the session
await careerCoach.RunAsync($"User Profile: {userProfile}", session);
await careerCoach.RunAsync($"Keep this analysis in mind for our conversation: {analysis}", session);

while (true)
{
    Console.Write("You: ");
    var userMessage = Console.ReadLine();
    
    if (string.IsNullOrEmpty(userMessage) || userMessage.ToLower() == "exit")
        break;
    
    // Use the session - the agent handles all conversation context automatically!
    var response = await careerCoach.RunAsync(userMessage, session);
    
    Console.WriteLine();
    Console.WriteLine($"Coach: {response}");
    Console.WriteLine();
}

Console.WriteLine("Thank you for using Microsoft Consultant Career Coach!");