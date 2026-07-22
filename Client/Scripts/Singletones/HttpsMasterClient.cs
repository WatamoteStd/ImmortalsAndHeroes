using Godot;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public partial class HttpsMasterClient : Node
{
	public static HttpsMasterClient Instanсe {get; private set;}
	System.Net.Http.HttpClient client;

	public override void _Ready()
	{
		if (Instanсe != null)
		{
			QueueFree();
			return;
		}
		else
		{
			Instanсe = this;
		}

		var handler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
			
		};

		
		client = new System.Net.Http.HttpClient(handler);
		client.BaseAddress = new Uri("https://localhost:29557/");
		client.Timeout = TimeSpan.FromSeconds(5);

	}

	public async Task<HttpResponseMessage> LoginRequestAsync(string username, string password)
	{
		
		var loginDTO = new
		{
			Username = username,
			Password = password
		};

		var response = await client.PostAsJsonAsync("api/auth/login", loginDTO);

		return response;

	}


}
