using Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public enum ControlMode
{
    None,
    FarmPlow,
    FarmPlant,
    FarmHarvest
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

    private ControlMode m_activeMode;

    public PlayerInventory PlayerInventory;
    public PlayerManager PlayerManager;

    private void Start()
    {
        m_activeMode = ControlMode.None;
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
            m_activeMode = ControlModeIsFarm(m_activeMode) ? ControlMode.None : ControlMode.FarmPlow;
        }

        if (ControlModeIsFarm(m_activeMode))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                m_activeMode = ControlMode.FarmPlow;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                m_activeMode = ControlMode.FarmPlant;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                m_activeMode = ControlMode.FarmHarvest;
            uiTilemap.ClearAllTiles();
        }

        if (m_activeMode == ControlMode.FarmPlow)
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int pos = uiTilemap.WorldToCell(world);

            uiTilemap.SetTile(pos, highlightTile);
            
            // Get world tile
            if (tileManager.CanBecomeFarmland(pos))
            {
                highlightTile.color = Color.green;

                if (Input.GetButton("Fire1"))
                {
                    tileManager.TryToFarmland(pos);
                }
            }
            else
            {
                highlightTile.color = Color.red;
            }
        }
        else if (m_activeMode == ControlMode.FarmPlant)
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int pos = uiTilemap.WorldToCell(world);

            uiTilemap.SetTile(pos, highlightTile);
            
            // Get world tile
            if (tileManager.CanBecomeFarmlandSeeds(pos) && PlayerInventory.GetCountForType<PlayerInventory.Seeds>() > 0)
            {
                highlightTile.color = Color.green;

                if (Input.GetButton("Fire1") && PlayerInventory.GetCountForType<PlayerInventory.Seeds>() >= 5)
                {
                    if (tileManager.TryToFarmlandSeeds(pos))
                        PlayerInventory.RemoveTypeByCount<PlayerInventory.Seeds>(5);
                }
            }
            else
            {
                highlightTile.color = Color.red;
            }
        }
        else if (m_activeMode == ControlMode.FarmHarvest)
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int pos = uiTilemap.WorldToCell(world);

            uiTilemap.SetTile(pos, highlightTile);
            
            // Get world tile
            if (tileManager.CanBeHarvested(pos))
            {
                highlightTile.color = Color.green;

                if (Input.GetButton("Fire1"))
                {
                    // Harvest
                    tileManager.TryToFarmland(pos);
                    PlayerManager.AddMoney(15);
                }
            }
            else
            {
                highlightTile.color = Color.red;
            }
        }
    }
}