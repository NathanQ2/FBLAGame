using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;

public enum TileType
{
    None = -1,
    Grass,
    Farmland,
    FarmlandSeeds,
    FarmlandGrown,
    Water
}

public class TileData
{
    public TileType Type;
    public Tile Tile;

    public readonly WorldTileManager ParentTilemap;
    public readonly Vector3Int Position;

    public static Tile FarmlandGrownTile;

    public TileData(Tile tile, TileType type, Vector3Int position, WorldTileManager parentTilemap)
    {
        Tile = tile;
        Type = type;
        ParentTilemap = parentTilemap;
        Position = position;
    }

    public TileData(Tile tile, Vector3Int position, WorldTileManager parentTilemap) : this(tile, TileTypeFromName(tile.name), position, parentTilemap) { }

    public bool CanBecomeFarmland() => Type is TileType.Grass or TileType.FarmlandSeeds or TileType.FarmlandGrown;
    public bool CanBecomeFarmlandSeeds() => Type is TileType.Farmland;
    public bool CanBeHarvested() => Type is TileType.FarmlandGrown;

    public bool IsFarmland() => IsFarmland(Type);

    public virtual void Update()
    { }

    public static bool IsFarmland(TileType type) => type is TileType.Farmland or TileType.FarmlandSeeds or TileType.FarmlandGrown;

    public static TileType TileTypeFromName(string name)
    {
        return name switch
        {
            "Tilemap_Water" => TileType.Water,
            "Tilemap_Grass" => TileType.Grass,
            "Tilemap_Farmland" => TileType.Farmland,
            "Tilemap_FarmlandSeeds" => TileType.FarmlandSeeds,
            "Tilemap_FarmlandGrown" => TileType.FarmlandGrown,
            _ => TileType.None
        };
    }
}

public class FarmlandTileData : TileData
{
    public static readonly float DefaultGrowthTimeSeconds = 1 * 60;
    public const float DefaultGrowthScale = 1.0f;

    private static float s_growthScale = DefaultGrowthScale;
    public static float GrowthScale
    {
        get => s_growthScale; 
        set
        {
            s_growthScale = value;

            foreach (Timer timer in Timers)
            {
                timer.TimeScale = value;
            }
        }
    }

    private static readonly List<Timer> Timers = new List<Timer>();

    public static float GrowthTimeSeconds => DefaultGrowthTimeSeconds * GrowthScale;
    
    private Timer m_growthTimer;
    
    public FarmlandTileData(Tile tile, TileType type, Vector3Int position, WorldTileManager parentTilemap, float growthTimeSeconds) : base(tile, type, position, parentTilemap)
    {
        Debug.Assert(IsFarmland(type));
        m_growthTimer = new Timer(growthTimeSeconds, ToGrown, GrowthScale);
        Timers.Add(m_growthTimer);

        if (type == TileType.FarmlandSeeds)
        {
            m_growthTimer.Start();
        }
    }

    ~FarmlandTileData()
    {
        Timers.Remove(m_growthTimer);
    }
    
    public FarmlandTileData(Tile tile, Vector3Int position, WorldTileManager parentTilemap, float growthTimeSeconds) : this(tile, TileTypeFromName(tile.name), position, parentTilemap, growthTimeSeconds)
    { }

    public FarmlandTileData(TileData other, TileType newType, float growthTimeSeconds) : this(other.Tile, newType, other.Position,
        other.ParentTilemap, growthTimeSeconds)
    { }

    public void ToGrown()
    {
        ParentTilemap.tilemap.SetTile(Position, FarmlandGrownTile);
        Type = TileType.FarmlandGrown;
        Tile = FarmlandGrownTile;
    }

    public override void Update()
    {
        m_growthTimer.Update(Time.deltaTime * GameManager.GameTimeScale);
    }
}
