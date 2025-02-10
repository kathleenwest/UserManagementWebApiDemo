public class MyApiClient
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetApiResponseAsync(string url)
    {
        try
        {
            // Make an HTTP GET request
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Read the response content
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return null;
        }
    }
}

