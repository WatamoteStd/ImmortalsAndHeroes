using Godot;
using Shared.Characters;
using System;

public partial class CharacterCreateWindow : PanelContainer
{
	
	[Export] private Button _backButton;
	[Export] private Button _createCharacter;
	[Export] private LineEdit _nickname;
	[Export] private OptionButton _skillType;
	
	// WINDOWS
	[Export] private StatusWindow _statusWindow;
	[Export] private CharacterWindow _characterWindow;

	public override void _Ready()
	{

		_nickname.GrabFocus();
		
		_backButton.Pressed += () =>
		{
			_nickname.Text = "";
			ChangeVisiblity();
		};

		_createCharacter.Pressed += CreateCharacterHttp;

	}


	public void ChangeVisiblity()
	{
		
		Visible = !Visible;

	}

	private async void CreateCharacterHttp()
	{
		
		_createCharacter.Disabled = true;
		var response = await HttpsMasterClient.Instanсe.CreateCharacterAsync(_nickname.Text, (EntityType)_skillType.Selected);

		if (response.isSucces == true && response.character != null)
		{
			
			_statusWindow.ShowMessage("Success!", "Character created!");
			_characterWindow.UpdateChracter(response.character.Nickname, response.character.Id.ToString(), response.character.Silver.ToString());
			

		}
		else 
		{
			_statusWindow.ShowMessage("Failure!", response.message);
		}

		_createCharacter.Disabled = false;

	}

}
