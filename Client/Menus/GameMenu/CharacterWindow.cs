using Godot;
using System;

public partial class CharacterWindow : PanelContainer
{

    [Export] private Label _nickname;
    [Export] private Label _id;
    [Export] private Label _silver;

    public void UpdateChracter(string nick, string id, string silver)
    {

        _nickname.Text = nick;
        _id.Text = id;
        _silver.Text = silver;

    }

}
