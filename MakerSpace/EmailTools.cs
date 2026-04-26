using System.ComponentModel;

/// <summary>
/// Tools for handling email-related functionality in the career coach application
/// </summary>
static class EmailTools
{
    [Description("Email a personalized skilling plan to the consultant based on their profile and career goals")]
    public static string EmailSkillingPlan(
        [Description("User's current profile and skills")] string userProfile,
        [Description("Personalized learning recommendations")] string recommendations)
    {
        Console.WriteLine();
        Console.WriteLine("EMAIL SENDING SIMULATION");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"Subject: Your Personalized Consultant Skilling Plan");
        Console.WriteLine();
        Console.WriteLine("EMAIL CONTENT:");
        Console.WriteLine($"Profile: {userProfile}");
        Console.WriteLine();
        Console.WriteLine($"Recommendations: {recommendations}");
        Console.WriteLine();
        Console.WriteLine("Email would be sent successfully!");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine();
        
        return "Email sent successfully! The consultant should receive their personalized skilling plan shortly.";
    }
}