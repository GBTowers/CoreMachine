namespace CoreMachine.Req.Test;

public class Playground
{
	[Fact]
	public void Play()
	{
		var httpClient = new HttpClient();

		var request = new HttpRequestMessage();
		request.Content = new StringContent("Hello!");
		request.RequestUri = new Uri("https://www.google.com");
		request.Headers.Add("hello", "world");
		request.Method = HttpMethod.Get;
		httpClient.SendAsync(request);

		var req = Req.Get
			.To("www.google.com")
			.AddHeader("hello", "world")
			.WithStringBody("Hello!")
			.Build();
	}
}