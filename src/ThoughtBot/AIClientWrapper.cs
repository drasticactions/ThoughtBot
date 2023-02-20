using ELIZA.NET;
using ThoughtBot.Utilities;

namespace AIChatModel
{
    public class AIClientWrapper
    {
        private Random random = new Random();
        public ELIZALib eliza;

        /// <summary>
        /// An "AI" Wrapper around ELIZA.NET.
        /// </summary>
        public AIClientWrapper()
        {
            // Pull in DOCTOR from an embedded resource.
            // This is a direct translation of the original DOCTOR script into JSON.
            eliza = new ELIZALib(LibraryUtilities.GetResourceFileContentAsString("Model.DOCTOR.json"));
        }

        public async Task<string> StartAsync()
        {
            // Create a delay effect to make the user think the model is "thinking".
            await Task.Delay(random.Next(2000, 3000));
            return eliza.Session.GetGreeting();
        }

        public async Task<string> StopAsync()
        {
            // Create a delay effect to make the user think the model is "thinking".
            await Task.Delay(random.Next(2000, 3000));
            return eliza.Session.GetGoodbye();
        }

        public async Task<string> QueryAsync(string q)
        {
            // Create a delay effect to make the user think the model is "thinking".
            await Task.Delay(random.Next(800, 2000));
            return eliza.GetResponse(q);
        }
    }
}
