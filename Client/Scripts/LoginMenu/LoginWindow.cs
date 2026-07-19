using Godot;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
        
        var responseCode = await HttpsMasterClient.Instanсe.LoginRequestAsync(_loginLine.Text, _passwordLine.Text);

        if (responseCode == HttpStatusCode.OK)
        {
            
            _answerCodeLabel.Text = "succesful login";
            await Task.Delay(1500);
            
            _heroSelectWindow.Visible = true;

        }

    }


}
