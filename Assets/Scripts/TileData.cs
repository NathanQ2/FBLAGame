using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    None = -1,
    Grass,
    Farmland,
    FarmlandSeeds,
    Water
}

public class TileData
{
    public TileType Type;
    public Tile Tile;

    public readonly Tilemap ParentTilemap;
    public readonly Vector3Int Position;

    public TileData(Tile tile, TileType type, Vector3Int position, Tilemap parentTilemap)
    {
        Tile = tile;
        Type = type;
        ParentTilemap = parentTilemap;
        Position = position;
    }

    public TileData(Tile tile, Vector3Int position, Tilemap parentTilemap) : this(tile, WorldTileManager.TileTypeFromName(tile.name), position, parentTilemap) { }

    public bool CanBecomeFarmland() => Type is TileType.Grass or TileType.FarmlandSeeds;
    public bool CanBecomeFarmlandSeeds() => Type is TileType.Farmland;

    public bool IsFarmland() => IsFarmland(Type);

    public virtual void Update()
    { }

    public static bool IsFarmland(TileType type) => type is TileType.Farmland or TileType.FarmlandSeeds;
}

public class FarmlandTileData : TileData
{
    public static readonly float DefaultGrowthTimeSeconds = 1 * 60;
    
    private Timer m_growthTimer;
    
    public FarmlandTileData(Tile tile, TileType type, Vector3Int position, Tilemap parentTilemap, float growthTimeSeconds) : base(tile, type, position, parentTilemap)
    {
        Debug.Assert(IsFarmland(type));
        m_growthTimer = new Timer(growthTimeSeconds, ToGrown);

        if (type == TileType.FarmlandSeeds)
        {
            m_growthTimer.Start();
        }
    }
    public FarmlandTileData(Tile tile, Vector3Int position, Tilemap parentTilemap, float growthTimeSeconds) : this(tile, WorldTileManager.TileTypeFromName(tile.name), position, parentTilemap, growthTimeSeconds)
    { }

    public FarmlandTileData(TileData other, TileType newType, float growthTimeSeconds) : this(other.Tile, newType, other.Position,
        other.ParentTilemap, growthTimeSeconds)
    { }

    public void ToGrown()
    {
        Debug.Log("Grown!");
    }

    public override void Update()
    {
        m_growthTimer.Update(Time.deltaTime * WorldTileManager.Timescale);
    }
}
