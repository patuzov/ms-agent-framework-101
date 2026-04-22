// using System.Text.Json;
// using System.Collections.Generic;
// using Azure.AI.OpenAI;
// using Azure.Identity;
// using Microsoft.Agents.AI;
// using OpenAI.Chat;
// using DotNetEnv;

// var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
// Env.Load(envPath);

// var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
//     ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set in .env");
// var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-5.4-mini";

// #region Instructions
// var basicInstructions = @"
// You are an AI strategy expert from the year 2031 for technical consultants. 
// Analyze consultant's skills and provide feedback on their relevance in the next 5 years.
// ";

// var criticalInstructions = @"
// You are an AI strategy expert from the year 2031 for technical consultants. 

// Analyze consultant's skills and tell:
// * what skills, habits, or systems will be worthless or obsolete in the next five years? 
// * What must the person start learning or building right now, so they won't regret it in 5 years?
// ";

// var noBsInstructions = @"
// Analyze consultant's skills and tell with brutal honesty:
// * what skills, habits, or systems will be worthless or obsolete in the next five years? 
// * What must the person start learning or building right now, so they won't regret it in 5 years?
// Please, no flattery.
// ";

// #endregion

// // Create the Microsoft Consultant Career Coach Agent
// var careerCoachInstructions = noBsInstructions;

// AIAgent careerCoach = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
//     .GetChatClient(deploymentName)
//     .AsAIAgent(
//         instructions: careerCoachInstructions,
//         name: "Microsoft Consultant Career Coach");

// // Welcome message and skill assessment
// Console.WriteLine("Welcome to your Consultant AI Career Coach!\n");
// Console.WriteLine("Let's future-proof your Microsoft consulting career!\n");

// // Collect consultant profile
// var userProfile = await CollectUserProfile();

// // Generate comprehensive career analysis
// var analysisPrompt = CreateAnalysisPrompt(userProfile);
// var analysis = await careerCoach.RunAsync(analysisPrompt);

// Console.WriteLine("\n" + new string('=', 80));
// Console.WriteLine("YOUR PERSONALIZED CAREER ANALYSIS");
// Console.WriteLine(new string('=', 80));
// Console.WriteLine(analysis);

// Console.WriteLine();
// Console.WriteLine("Type your message or 'exit' to quit");
// Console.WriteLine();

// // Initialize conversation history to maintain context
// var conversationHistory = new List<string>
// {
//     $"User Profile: {userProfile}",
//     $"Initial Analysis: {analysis}"
// };

// while (true)
// {
//     Console.Write("You: ");
//     var userMessage = Console.ReadLine();
    
//     if (string.IsNullOrEmpty(userMessage) || userMessage.ToLower() == "exit")
//         break;
    
//     // Add the user's question to conversation history
//     conversationHistory.Add($"User: {userMessage}");
    
//     // Build the full context including all previous conversation
//     var contextualPrompt = BuildContextualPrompt(conversationHistory, userMessage);
    
//     // Get AI response with full conversation context
//     var response = await careerCoach.RunAsync(contextualPrompt);
    
//     // Add AI response to conversation history
//     conversationHistory.Add($"AI Coach: {response}");
    
//     Console.WriteLine($"\nCoach: {response}\n");
// }

// Console.WriteLine("Thank you for using Microsoft Consultant Career Coach!");

// static async Task<string> CollectUserProfile()
// {
//     Console.WriteLine("Give me some context on your profile:");

//     Console.WriteLine("What field or profession are you currently in?");
//     Console.WriteLine("What are your main skills or strengths right now?");
//     Console.WriteLine("What's your typical workday like?");
//     Console.WriteLine();


//     Console.Write("You: ");
//     string context = Console.ReadLine() ?? "";

//     return context;    
// }

// // Helper method to create analysis prompt
// static string CreateAnalysisPrompt(string userProfile)
// {
//     return $@"
// Analyze this profile and provide feedback. Give concise feedback for each skill/strength mentioned.
// ---
// {userProfile}
// ---

// In the end, if there is room for improvement, mention the need for constant learning and ask the user if the want a 6-months skilling plan to future-proof their career.
// ";
// }

// // Helper method to build contextual prompt with conversation history
// static string BuildContextualPrompt(List<string> conversationHistory, string currentQuestion)
// {
//     var context = string.Join("\n\n", conversationHistory);
    
//     return $@"
// Previous conversation context:
// {context}

// Current user question: {currentQuestion}

// Please respond to the current question while being aware of the full conversation context above. 
// If the user responds with 'yes' or agrees to something mentioned in the previous analysis (like a 6-month skilling plan), 
// provide that specific deliverable. Keep your response focused and actionable.
// ";
// }


