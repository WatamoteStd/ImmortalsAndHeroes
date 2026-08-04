using Godot;
using System;

public partial class CharacterWindow : PanelContainer
{

    [Export] private Label _nickname;
    [Export] private Label _id;
    [Export] private Label _silver;
    [Export] private PanelContainer _characterCard;
    [Export] private StatusWindow _statusWindow;
    [Export] private Button _characterCreateButton;

    public override void _Ready()
    {

        _characterCard.Visible = false;
        ServerCharacterRequest();
        
    }

    public void UpdateChracter(string nick, string id, string silver)
    {

        _nickname.Text = nick;
        _id.Text = id;
        _silver.Text = silver;
        _characterCard.Visible = true;
        _characterCreateButton.Visible = false;

    }

    private async void ServerCharacterRequest()
    {

        var response = await HttpsMasterClient.Instanсe.GetCharacter();

        if (response.Item2 == null)
        {
            _statusWindow.ShowMessage("Server Information", response.message);
        }
        else
        {
            
            UpdateChracter(response.character.Nickname, response.character.Id.ToString(), response.character.Silver.ToString());
            _statusWindow.ShowMessage("Server Information", response.message);
            
        }

    }

}
