using System;
using System.Net.Http;
using System.Net.Http.Json;
using Shared.Characters;
using Server.DTO.HTTP;

namespace Server.Network;

public class HttpMaster
{
    
    private readonly HttpClient client;
    
    public HttpMaster()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
            
        client = new HttpClient(handler);
        client.BaseAddress = new Uri("https://127.0.0.1:29557/");

    }

    public async Task<(bool isValid, HandshakeResponseDto? data, string message)> ValidateSessionAsync(string ticket)
    {
        
        try
        {
            
            var response = await client.PostAsJsonAsync("api/internal/validate-handshake", new {Ticket = ticket});

            if (response.IsSuccessStatusCode)
            {
                
                var characterData = await response.Content.ReadFromJsonAsync<HandshakeResponseDto>();
                
                if (characterData != null) return (true, characterData, "Ok");

                return (false, null, "No character");


            }
            string errorMsg = await response.Content.ReadAsStringAsync();
            return (false, null, errorMsg);

        }
        catch (Exception e)
        {
            Console.WriteLine($"[HTTP BRIDGE] Error:{e.Message}");
            return (false, null, "Master Server error");
        }

    }


}