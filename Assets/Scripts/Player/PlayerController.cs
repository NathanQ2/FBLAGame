using Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Wilberforce;

public enum ControlMode
{
    None,
    FarmPlow,
    FarmPlant,
    FarmHarvest
}

public static class ControlModeUtils
{
    public static string ToString(ControlMode mode)
    {
        return mode switch
        {
            ControlMode.None => "None",
            ControlMode.FarmPlow => "Farm Plow",
            ControlMode.FarmPlant => "Farm Plant",
            ControlMode.FarmHarvest => "Farm Harvest",
            _ => ""
        };
    }
}


public class PlayerController : MonoBehaviour
{
    private int m_direction = 0;

    public Animator animator;
    public Rigidbody2D rb;
    public float maxSpeed = 10.0f;
    public new Camera camera;

    public Tilemap uiTilemap;
    public Tile highlightTile;
    public WorldTileManager tileManager;

    public ControlMode ActiveMode { get; private set; }

    public PlayerInventory PlayerInventory;
    public PlayerManager PlayerManager;

    public Colorblind Colorblind;

    private Color m_HighlightColorGreen => Colorblind.ModeType switch
    {
        ColorBlindType.Protanopia => Color.green, // red / green colorblindness
        ColorBlindType.Deuteranopia => Color.green, // red / green colorblindness
        ColorBlindType.Tritanopia => Color.red, // blue / yellow colorblindness
        _ => Color.green // None
    };
    private Color m_HighlightColorRed => Colorblind.ModeType switch
    {
        ColorBlindType.Protanopia => Color.blue, // red / green colorblindness
        ColorBlindType.Deuteranopia => Color.blue, // red / green colorblindness
        ColorBlindType.Tritanopia => Color.gray, // blue / yellow colorblindness
        _ => Color.red // None
    };

    private void Start()
    {
        ActiveMode = ControlMode.None;
    }

    void Update()
    {
        Move();
        Farm();
    }

    private void Move()
    {

        Vector2 desiredVelocity = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1) * maxSpeed;
        rb.linearVelocity = desiredVelocity;

        if (desiredVelocity.magnitude == 0)
            return;

        if (Mathf.Abs(desiredVelocity.x) >= Mathf.Abs(desiredVelocity.y))
        {
            m_direction = desiredVelocity.x > 0 ? 1 : 3;
        }
        else
        {
            m_direction = desiredVelocity.y > 0 ? 0 : 2;
        }
        
        animator.SetInteger("Direction", m_direction);
    }

    private static bool ControlModeIsFarm(ControlMode mode) => mode is ControlMode.FarmPlow or ControlMode.FarmPlant or ControlMode.FarmHarvest;
    
        
    private void Farm()
    {
        if (Input.GetButtonDown("ToggleFarmMode"))
        {
            uiTilemap.ClearAllTiles();
            ActiveMode = ControlModeIsFarm(ActiveMode) ? ControlMode.None : ControlMode.FarmPlow;
        }

        if (PlayerManager.UIManager.IsAnyOpen())
        {
            uiTilemap.ClearAllTiles();
            ActiveMode = ControlMode.None;
        }

        if (ControlModeIsFarm(ActiveMode))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ActiveMode = ControlMode.FarmPlow;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                ActiveMode = ControlMode.FarmPlant;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                ActiveMode = ControlMode.FarmHarvest;
            uiTilemap.ClearAllTiles();
        }

        if (ActiveMode == ControlMode.FarmPlow)
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int pos = uiTilemap.WorldToCell(world);

            uiTilemap.SetTile(pos, highlightTile);
            
            // Get world tile
            if (tileManager.CanBecomeFarmland(pos))
            {
                highlightTile.color = m_HighlightColorGreen;

                if (Input.GetButton("Fire1"))
                {
                    if (PlayerManager.CurrentMoney > GameplayConfig.PlowCost && tileManager.TryToFarmland(pos))
                        PlayerManager.RemoveMoney(GameplayConfig.PlowCost);
                }
            }
            else
            {
                highlightTile.color = m_HighlightColorRed;
            }
        }
        else if (ActiveMode == ControlMode.FarmPlant)
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int pos = uiTilemap.WorldToCell(world);

            uiTilemap.SetTile(pos, highlightTile);
            
            // Get world tile
            if (tileManager.CanBecomeFarmlandSeeds(pos) && PlayerInventory.GetCountForType<PlayerInventory.Seeds>() >= GameplayConfig.SeedsLostPerPlant)
            {
                highlightTile.color = m_HighlightColorGreen;

                if (Input.GetButton("Fire1")) 
                {
                    if (tileManager.TryToFarmlandSeeds(pos))
                        PlayerInventory.RemoveTypeByCount<PlayerInventory.Seeds>(GameplayConfig.SeedsLostPerPlant);
                }
            }
            else
            {
                highlightTile.color = m_HighlightColorRed;
            }
        }
        else if (ActiveMode == ControlMode.FarmHarvest)
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int pos = uiTilemap.WorldToCell(world);

            uiTilemap.SetTile(pos, highlightTile);
            
            // Get world tile
            if (tileManager.CanBeHarvested(pos))
            {
                highlightTile.color = m_HighlightColorGreen;

                if (Input.GetButton("Fire1"))
                {
                    // Harvest
                    tileManager.TryToFarmland(pos);
                    PlayerManager.AddMoney(GameplayConfig.MoneyPerHarvest);
                }
            }
            else
            {
                highlightTile.color = m_HighlightColorRed;
            }
        }
    }
}