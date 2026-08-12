using UnityEngine;

public class ArchipelagoLocation
{
    public string LocationId { get; set; }      // "PrecursorSuit_12a34b56"
    public string DisplayName { get; set; }     // "Precursor Suit Fragment"
    public string TechType { get; set; }
    public string UID { get; set; }             // UID Subnautica unique
    public bool IsFragment { get; set; }
    public int TotalFragments { get; set; }
    public float[] Position { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public bool Collected { get; set; }         // Vérifié ou pas
    public string Biome { get; set; }

    public ArchipelagoLocation()
    {
        Position = new float[] { 0, 0, 0 };
    }

public ArchipelagoLocation(string locationId, string displayName, string techType, string uid)
    {
        LocationId = locationId;
        DisplayName = displayName;
        TechType = techType;
        UID = uid;
        IsFragment = false;
        TotalFragments = 1;
        Collected = false;
        Biome = "Unknown";
    }

    public Vector3 GetPosition()
    {
        return new Vector3(PosX, PosY, PosZ);
    }

    public void SetPosition(Vector3 pos)
    {
        PosX = pos.x;
        PosY = pos.y;
        PosZ = pos.z;
    }
}
