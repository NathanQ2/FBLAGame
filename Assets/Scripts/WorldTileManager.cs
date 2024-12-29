using System;
using System.Collections.Generic;
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

public struct TileData
{
    public TileType Type;
    public Tile Tile;

    public TileData(Tile tile, TileType type)
    {
        Tile = tile;
        Type = type;
    }

    public TileData(Tile tile) : this(tile, WorldTileManager.TileTypeFromName(tile.name)) { }

    public bool CanBecomeFarmland() => Type is TileType.Grass or TileType.FarmlandSeeds;
    public bool CanBecomeFarmlandSeeds() => Type is TileType.Farmland;
}

public class WorldTileManager : MonoBehaviour
{
    public Tilemap tilemap;
    private Dictionary<Vector3Int, TileData> m_tiles = new Dictionary<Vector3Int, TileData>();

    public Tile farmlandTile;
    public Tile farmlandSeedsTile;

    public void Start()
    {
        Vector3 min = tilemap.localBounds.min;
        Vector3 max = tilemap.localBounds.max;

        for (float x = min.x; x < max.x; x++)
        {
            for (float y = min.y; y < max.y; y++)
            {
                Vector3Int pos = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0);
                Tile tile = tilemap.GetTile<Tile>(pos);
                m_tiles.Add(pos, new TileData(tile));
            }
        }
    }

    public bool CanBecomeFarmland(Vector3Int pos) => m_tiles[pos].CanBecomeFarmland();
    public bool CanBecomeFarmlandSeeds(Vector3Int pos) => m_tiles[pos].CanBecomeFarmlandSeeds();
    
    
    public bool TryToFarmland(Vector3Int pos)
    {
        if (!CanBecomeFarmland(pos)) 
            return false;

        tilemap.SetTile(pos, farmlandTile);
        var tile = m_tiles[pos];
        tile.Type = TileType.Farmland;
        m_tiles[pos] = tile;

        return true;
    }

    public bool TryToFarmlandSeeds(Vector3Int pos)
    {
        if (!CanBecomeFarmlandSeeds(pos))
            return false;
        
        tilemap.SetTile(pos, farmlandSeedsTile);
        var tile = m_tiles[pos];
        tile.Type = TileType.FarmlandSeeds;
        m_tiles[pos] = tile;

        return true;
    }

    public static TileType TileTypeFromName(string name)
    {
        return name switch
        {
            "TileMap_Water" => TileType.Water,
            "TileMap_Grass" => TileType.Grass,
            "TileMap_Farmland" => TileType.Farmland,
            "TileMap_FarmlandSeeds" => TileType.FarmlandSeeds,
            _ => TileType.None
        };
    }
}
