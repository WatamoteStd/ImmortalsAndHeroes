using Godot;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;

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

    }

    private async void LoginRequest()
    {
        
        var responseMessage = await HttpsMasterClient.Instanсe.LoginRequestAsync(_loginLine.Text, _passwordLine.Text);

        if (responseMessage.StatusCode == HttpStatusCode.OK)
        {
            
            var data = await responseMessage.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            if (data != null && data.TryGetValue("token", out var token))
            {
                
                _answerCodeLabel.Text = $"Succesful login.";
                GD.Print(token);

                 await Task.Delay(1500);
                _heroSelectWindow.Visible = true;

            }

            

        }

    }


}
