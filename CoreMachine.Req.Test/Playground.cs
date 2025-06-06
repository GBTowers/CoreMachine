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
		request.Headers.Add(name: "hello", value: "world");
		request.Method = HttpMethod.Get;
		httpClient.SendAsync(request);

		// $"/audits?" +
		// 	$"StartDateLocal={StartDateLocal?.ToString(Constants.DateTimeFormat).EscapeDataString()}" +
		// 	$"&EndDateLocal={EndDateLocal?.ToString(Constants.DateTimeFormat).EscapeDataString()}" +
		// 	$"&EventName={EventName.EscapeDataString()}" +
		// 	$"&Username={Username.EscapeDataString()}" +
		// 	$"&ApplicationName={(applicationName ?? ApplicationName).EscapeDataString()}" +
		// 	$"&TargetName={(targetName ?? TargetName)?.EscapeDataString()}" +
		// 	$"&TargetValue={(targetValue ?? TargetValue)?.EscapeDataString()}" +
		// 	$"&PageSize={pageSize}&PageNumber={pageNumber}";
		
		Req.Post.To("audits")
		.WithHeaders(new
			{
				x_functions_key = "weoifjqpewofinqe",
				hello = "header"
			})
		.WithJsonBody(new
			{
				hello = "world"
			});
		
		HttpRequestMessage req = Req.Get.To("www.google.com")
		.AddHeader(key: "hello", value: "world")
		.WithStringBody("Hello!")
		.Build();
	}
}
