using Godot;
using System;

public partial class CategoryComponent : Control
{
	
	[Export] private Label _value;

	public void SetValue(string value)
	{

		if (string.IsNullOrWhiteSpace(value) || value == "None")
		{
			Visible = false;
			return;
		}

		Visible = true;
		_value.Text = value;

		if (value == "YES")
		{
			_value.SelfModulate = new Color(0.0f, 0.506f, 0.067f);
		}
		else if (value == "NO")
		{
			_value.SelfModulate = new Color(0.651f, 0.0f, 0.067f);
		}
		else
		{
			_value.SelfModulate = Colors.White; 
		}

	}

}
