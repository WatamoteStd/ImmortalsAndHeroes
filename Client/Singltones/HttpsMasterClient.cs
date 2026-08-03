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
		client.Timeout = TimeSpan.FromSeconds(15);

	}

	public async Task<(bool isSuccess, string message)> RegisterRequestAsync(string login, string password, string email)
	{
		
		var registerDto = new
		{
			Username = login,
			Password = password,
			Email = email
		};
		try
		{

			HttpResponseMessage response = await client.PostAsJsonAsync("api/auth/register", registerDto);

			string serverMessage = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode) return (true, serverMessage);
			else return (false, serverMessage);

		}
		catch (Exception e)
		{
			GD.Print($"[HTTP MASTER] Eror when try register user. {e.Message}");
			return (false, "Can't connect to the server. Try again.");
		}




	}


	public async Task<(bool isSuccess, string message)> LoginRequestAsync(string login, string password)
	{
		
		var loginDto = new
		{
			Username = login,
			Password = password
		};

		try
		{
			
			var response = await client.PostAsJsonAsync("api/auth/login", loginDto);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

				if (result != null)
				{
					GameSession.Instance.GlobalId = result.UserId;
					GameSession.Instance.Username = result.Username;
					return (true, "Successful login!");
				}
				else return (false, "Failed when reading result response");

			}
			var errorResponse = await response.Content.ReadAsStringAsync();
            return (false, errorResponse);

		}
		catch (Exception e)
		{
			
			GD.Print($"[HTTP CLIENT] Server error. {e.Message}");
			return (false, "Server error. Try again.");

		}




	}

	public record LoginResponseDto(string Username, long UserId, DateTime CreatedAt);

}
