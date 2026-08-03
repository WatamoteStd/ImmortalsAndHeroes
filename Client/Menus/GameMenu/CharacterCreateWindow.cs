using Godot;
using System;

public partial class CharacterCreateWindow : PanelContainer
{
	
	[Export] private Button _backButton;
	[Export] private Button _createCharacter;
	[Export] private LineEdit _nickname;
	[Export] private OptionButton _skillType;

	public override void _Ready()
	{

		_nickname.GrabFocus();
		
		_backButton.Pressed += () =>
		{
			_nickname.Text = "";
			ChangeVisiblity();
		};

	}


	public void ChangeVisiblity()
	{
		
		Visible = !Visible;

	}

	private async void CreateCharacterHttp()
	{
		
		

	}

}
