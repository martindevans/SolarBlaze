namespace SolarBlaze.Config
{
    public class InitialSetup
    {
        public string Email { get; set; }
        public string ServerUrl { get; set; }

        public InitialSetup(string email, string serverUrl)
        {
            Email = email;
            ServerUrl = serverUrl;
        }
    }
}
