using Godot;
using Shared.Characters;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
					GameSession.Instance.MasterToken = result.Token;

					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
					
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

	// CHARACTER TASKS

	public async Task<(bool isSucces, CharacterCreatedResponseDto? character, string message)> CreateCharacterAsync(string nickname, CharacterType type)
	{
		
		var cDto = new CharacterCreateRequestDto(nickname, type);

		try
		{
			
			var response = await client.PostAsJsonAsync("api/character/create", cDto);

			if (response.IsSuccessStatusCode)
			{

				var charInfo = await response.Content.ReadFromJsonAsync<CharacterCreatedResponseDto>();

				if (charInfo != null)
				{
					return (true, charInfo, "Character created!");
				}
				else return (false, null, "Server data error.");

			}
			else 
			{
				var error = await response.Content.ReadAsStringAsync();
				return (false, null, error);
			}

		}
		catch (Exception e)
		{
			
			return (false, null, "Connection with server lost. Try again..");

		}

		

	}

	public async Task<(bool isSussec, CharacterCreatedResponseDto? character, string message)> GetCharacter()
	{

		try
		{

			var response = await client.GetAsync("api/character/get");

			if (response.IsSuccessStatusCode)
			{

				var character = await response.Content.ReadFromJsonAsync<CharacterCreatedResponseDto>();
				return (true, character, "Character loaded");

			}
			else if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return (false, null, "No character");
			}

			string error = await response.Content.ReadAsStringAsync();
			return (false, null, error);

		}
		catch (Exception e)
		{
			GD.Print($"[HTTP MASTER] Error getting character: {e.Message}");
			return (false, null, "Connection lost.");
		}

	}

	public async Task<(bool isSucces, string message)> EnterWorldAsync()
	{

		try
		{

			var response = await client.GetAsync("/api/gamesession/enter");

			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadFromJsonAsync<EnterWorldResponseDto>();

				GameSession.Instance.UdpToken = data.Ticket;
				GameSession.Instance.UdpIp = data.UdpIp;
				GameSession.Instance.UdpPort = data.Port;

				return (true, "Entering world...");
			}
			else
			{
				var errorMessage = await response.Content.ReadAsStringAsync();
				return (false, errorMessage);
			}
		}
		catch (Exception e)
		{
			GD.PrintErr($"Server error. {e.Message}");
			return (false, "Server error. Please try again.");
		}
		
		

	}
	
	#region DTOS

	public record LoginResponseDto(string Username, long UserId, DateTime CreatedAt, string Token);
	public record CharacterCreateRequestDto(string Nickname, CharacterType Type);
	public record CharacterCreatedResponseDto(string Nickname, long Silver, CharacterType Type, long Id);

	public record EnterWorldResponseDto(string Ticket, string UdpIp, int Port);

	#endregion

}
