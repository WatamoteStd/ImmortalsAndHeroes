using Godot;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text.Json;
using Shared.Network.Packets;

public partial class LoginWindow : PanelContainer
{
    
    [Export] private LineEdit _loginLine;
    [Export] private LineEdit _passwordLine;
    [Export] private Button _loginButton;
    [Export] private Label _answerCodeLabel;
    [Export] private PanelContainer _heroSelectWindow;

    public override void _Ready()
    {
        _heroSelectWindow.Visible = false;
        _loginButton.Pressed += LoginRequest;
        NetworkPacketManager.Instance.OnHandshakeResponse += EnterWorld;

    }

    private async void LoginRequest()
    {

        var responseMessage = await HttpsMasterClient.Instanсe.LoginRequestAsync(_loginLine.Text, _passwordLine.Text);


        if (responseMessage.StatusCode == HttpStatusCode.OK)
        {
            
            var data = await responseMessage.Content.ReadFromJsonAsync<Dictionary<string, object>>();


            if (data != null && data.TryGetValue("token", out var tokenObj) && data.TryGetValue("ticket", out var ticketObj))
            {
                long ticket = ((JsonElement)ticketObj).GetInt64();
                string token = ((JsonElement)tokenObj).GetString();

                GameSession.Instance.AuthTicket = ticket;
                GameSession.Instance.AuthToken = token;
                GameSession.Instance.CurrentSessionState = GameSession.State.Loading;

                NetworkUdpClient.Instance.Connect(ticket);  

            }

            

        }

    }

    private async void EnterWorld(S2C_HandshakeResponse response)
    {
        
        if (response.Status == 1)
        {
            
            _answerCodeLabel.Text = $"Succesful login.";
            await Task.Delay(1500);
            Callable.From(() => GetTree().ChangeSceneToFile("res://Scenes/World/Region_0.tscn")).Call();

        }
        else
        {
            _answerCodeLabel.Text = $"Can't connect to the UDP server. Try again...";
        }

    }

    public override void _ExitTree()
    {
        if (NetworkPacketManager.Instance != null)
        {
            NetworkPacketManager.Instance.OnHandshakeResponse -= EnterWorld;
        }
    }



}
