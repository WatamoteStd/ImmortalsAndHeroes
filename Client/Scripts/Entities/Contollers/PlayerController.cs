using Godot;
using System;

public partial class PlayerController : Node
{
    
    private Camera3D _camera;
    private RayCast3D _cameraRay;

    public void Initialize(Camera3D camera, RayCast3D raycast)
    {
        
        _camera = camera;
        _cameraRay = raycast;

    }

    public override void _UnhandledInput(InputEvent @event)
    {

        if (_camera == null || _cameraRay == null) return;
        
        if (@event is InputEventMouseButton mouseBtn && mouseBtn.Pressed && mouseBtn.ButtonIndex == MouseButton.Right)
        {
            
            Vector2 mousePos = mouseBtn.Position;

            Vector3 rayDirectionGlobal = _camera.ProjectRayNormal(mousePos);
            _cameraRay.TargetPosition = _cameraRay.GlobalTransform.Basis.Inverse() * rayDirectionGlobal * 100f;

            _cameraRay.ForceRaycastUpdate();

            if (_cameraRay.IsColliding())
            {
                
                Vector3 clickPosition = _cameraRay.GetCollisionPoint();

                NetworkUdpClient.Instance.PCSendMoveRequest(clickPosition.X, clickPosition.Y, clickPosition.Z);

                GD.Print($"Click point: {clickPosition}");

            }

        }

    }



}
