

namespace Server.World.Zone.RegionController;

public enum DensityModes : byte
{
    
    Near = 0,
    Normal = 1,
    Far = 2

}

public static class DensityConstants
{

    
    public const float NEAR = 8.0f;
    public const float NORMAL = 20.0f;
    public const float FAR = 45.0f;

    public static float GetDensityDistanceSq(this DensityModes density) => density switch
    {
        
        DensityModes.Near => NEAR * NEAR,
        DensityModes.Normal => NORMAL * NORMAL,
        DensityModes.Far => FAR * FAR,
        _ => NORMAL * NORMAL

    };

}