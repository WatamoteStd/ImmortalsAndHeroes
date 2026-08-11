using Godot;
using Shared.Items;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class SceneManager : CanvasLayer
{

	[Export] private AnimationPlayer _animator;
	[Export] private Hud _hud;
	[Export] private Inventory _inventory;
	public static SceneManager Instance { get; private set; }

	public Dictionary<uint, string> regIdToScenePath;

	public override void _Ready()
	{

		if (Instance != null)
		{
			QueueFree();
			return;
		}
		else Instance = this;

		Layer = 200;
		Visible = false;

		regIdToScenePath = new Dictionary<uint, string>
		{
			
			{0, "res://World/Regions/Region_0.tscn"}

		};

		PlayerController.OnInventoryAction -= InventoryAction;
		PlayerController.OnInventoryAction += InventoryAction;

	}

	public async Task AuthToMainMenu() // only for auth menu -> main menu (i don't know why i did this, god bless me)
	{
		
		Visible = true;
		_animator.Play("Idle");
		GetTree().ChangeSceneToFile("res://Menus/GameMenu/GameMenu.tscn");
		var timer = GetTree().CreateTimer(6.5f);
		await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

		Visible = false;


	}

   public async Task LoadRegion(uint regionId)
	{
		HideHud();

		if (!regIdToScenePath.TryGetValue(regionId, out string path))
		{
			GD.PrintErr($"[SCENE MANAGER] Unknown RegionId: {regionId}");
			return;
		}
		
		GameSession.Instance.CurrentSessionState = GameSession.State.Loading;
		Visible = true;
		_animator.Play("Idle");
		GetTree().ChangeSceneToFile(path);
		var timer = GetTree().CreateTimer(6.5f);
		await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

		GameSession.Instance.CurrentSessionState = GameSession.State.InGame;
		Visible = false;
		ShowHud();

	}
	
	private void ShowHud()
	{
		
		_hud.Visible = true;

	}
	private void HideHud()
	{
		_hud.Visible = false;
	}

	public void InitPlayerHud(uint hp, uint mp, uint silver, uint lvl, string name)
	{
		
		_hud.InitHud(hp,mp,silver,lvl,name);

	}

	private void InventoryAction()
	{
		_inventory.Visible = !_inventory.Visible;
	}
	public void UpdateInventoryCell(ushort slotIndex, ItemType item, ushort count)
	{
		_inventory.UpdateCell(slotIndex,item, count);
	}

	
}
