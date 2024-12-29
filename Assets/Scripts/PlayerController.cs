using UnityEngine;
using UnityEngine.Tilemaps;

public enum ControlMode
{
    None,
    Farm
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
        
    private void Farm()
    {
        if (Input.GetButtonDown("ToggleFarmMode"))
        {
            uiTilemap.ClearAllTiles();
            m_activeMode = m_activeMode == ControlMode.Farm ? ControlMode.None : ControlMode.Farm;
        }

        if (m_activeMode == ControlMode.Farm)
        {
            uiTilemap.ClearAllTiles();

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
    }
}