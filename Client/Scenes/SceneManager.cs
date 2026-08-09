using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class SceneManager : CanvasLayer
{

    [Export] private AnimationPlayer _animator;
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

    }
    
    
}
