namespace WebApiClient
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string url = "https://localhost:7048/api/users";
            string response = await MyApiClient.GetApiResponseAsync(url);

            if (response != null)
            {
                Console.WriteLine("API response:");
                Console.WriteLine(response);
                Console.ReadLine();
            }
        }
    }
}

