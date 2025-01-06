using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public static float Timescale = 1.0f;
    public Dictionary<Vector3Int, TileData> tiles = new Dictionary<Vector3Int, TileData>();

    public Tile farmlandTile;
    public Tile farmlandSeedsTile;
    public Tile farmlandGrownTile;

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
                tiles.Add(pos, new TileData(tile, pos, this));
            }
        }

        TileData.FarmlandGrownTile = farmlandGrownTile;
    }

    public void Update()
    {
        foreach ((Vector3Int pos, TileData data) in tiles)
        {
            data.Update();
        }
    }

    public bool CanBecomeFarmland(Vector3Int pos) => tiles[pos].CanBecomeFarmland();
    public bool CanBecomeFarmlandSeeds(Vector3Int pos) => tiles[pos].CanBecomeFarmlandSeeds();
    public bool CanBeHarvested(Vector3Int pos) => tiles[pos].CanBeHarvested();
    
    
    public bool TryToFarmland(Vector3Int pos)
    {
        if (!CanBecomeFarmland(pos)) 
            return false;

        tilemap.SetTile(pos, farmlandTile);
        FarmlandTileData tile = new FarmlandTileData(tiles[pos], TileType.Farmland, FarmlandTileData.DefaultGrowthTimeSeconds);
        tiles[pos] = tile;

        return true;
    }

    public bool TryToFarmlandSeeds(Vector3Int pos)
    {
        if (!CanBecomeFarmlandSeeds(pos))
            return false;
        
        tilemap.SetTile(pos, farmlandSeedsTile);
        FarmlandTileData tile = new FarmlandTileData(tiles[pos], TileType.FarmlandSeeds, FarmlandTileData.DefaultGrowthTimeSeconds);
        tiles[pos] = tile;

        return true;
    }
}
