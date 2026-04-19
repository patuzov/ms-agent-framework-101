using System;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI.Chat;
using DotNetEnv;

var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
Env.Load(envPath);

var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
    ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set in .env");
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-5.4-mini";

AIAgent agent = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .AsAIAgent(
        instructions: "You are a friendly assistant. Keep your answers brief.",
        name: "MakerSpaceAgent");

Console.WriteLine(await agent.RunAsync("What is the largest city in France?"));
