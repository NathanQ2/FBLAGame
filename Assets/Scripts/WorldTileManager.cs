using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public static float Timescale = 1.0f;
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
                m_tiles.Add(pos, new TileData(tile, pos, tilemap));
            }
        }
    }

    public void Update()
    {
        foreach ((Vector3Int pos, TileData data) in m_tiles)
        {
            data.Update();
        }
    }

    public bool CanBecomeFarmland(Vector3Int pos) => m_tiles[pos].CanBecomeFarmland();
    public bool CanBecomeFarmlandSeeds(Vector3Int pos) => m_tiles[pos].CanBecomeFarmlandSeeds();
    
    
    public bool TryToFarmland(Vector3Int pos)
    {
        if (!CanBecomeFarmland(pos)) 
            return false;

        tilemap.SetTile(pos, farmlandTile);
        FarmlandTileData tile = new FarmlandTileData(m_tiles[pos], TileType.Farmland, FarmlandTileData.DefaultGrowthTimeSeconds);
        m_tiles[pos] = tile;

        return true;
    }

    public bool TryToFarmlandSeeds(Vector3Int pos)
    {
        if (!CanBecomeFarmlandSeeds(pos))
            return false;
        
        tilemap.SetTile(pos, farmlandSeedsTile);
        FarmlandTileData tile = new FarmlandTileData(m_tiles[pos], TileType.FarmlandSeeds, FarmlandTileData.DefaultGrowthTimeSeconds);
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
