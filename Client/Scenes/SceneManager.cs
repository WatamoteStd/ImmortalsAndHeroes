using Godot;
using System;
using System.Threading.Tasks;

public partial class SceneManager : CanvasLayer
{

    [Export] private AnimationPlayer _animator;
    public static SceneManager Instance { get; private set; }

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
    }

    public async Task LoadRegionFromMenu(string path)
    {
        
        Visible = true;
        _animator.Play("Idle");
        GetTree().ChangeSceneToFile(path);
        var timer = GetTree().CreateTimer(6.5f);
        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

        Visible = false;


    }
    
    
}
