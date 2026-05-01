using Godot;
using System;

public partial class HeroResource : Resource
{
	[Export] public string Name;
	[Export] public Texture2D Icon;
	[Export] public string Description;
	[Export] public int Health;
	[Export] public int Damage;
	[Export] public string AttackType;
	[Export] public int MoveSpeed;
	[Export] public int Armor;
	[Export] public float BaseAttackInterval;
	[Export] public string Skill1Name;
	[Export] public string Skill1Description;
	[Export] public string Skill2Name;
	[Export] public string Skill2Description;
	[Export] public string Skill3Name;
	[Export] public string Skill3Description;
}
