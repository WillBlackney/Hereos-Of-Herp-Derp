using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public enum TileType { None, Dirt, Grass, Forest, Rock, Water };

    [Header("Component References")]
    private SpriteRenderer mySpriteRenderer;
    public Animator myAnimator;

    [Header("Properties")]
    public TileType myTileType;
    public bool IsEmpty;
    public bool IsWalkable;
    public Color32 originalColor;
    public Color32 highlightedColor = Color.white;
    public Point GridPosition { get; set; }
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f);
        }
    }

    // Initialization + Setup
    #region
    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = mySpriteRenderer.color;
    }
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        if (myTileType == TileType.Dirt)
        {
            RunDirtTileSetup();
        }
        else if (myTileType == TileType.Grass)
        {
            RunGrassTileSetup();
        }
        else if (myTileType == TileType.Rock)
        {
            RunRockTileSetup();
        }
        else if (myTileType == TileType.Water)
        {
            RunWaterTileSetup();
        }

        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }
    #endregion

    // Set Tile Type
    #region
    public void RunDirtTileSetup()
    {
        IsWalkable = true;
        IsEmpty = true;
    }
    public void RunGrassTileSetup()
    {
        IsWalkable = true;
        IsEmpty = true;
    }
    public void RunRockTileSetup()
    {
        IsWalkable = true;
        IsEmpty = true;
    }
    public void RunForestTileSetup()
    {
        IsWalkable = false;
        IsEmpty = false;
    }
    public void RunWaterTileSetup()
    {
        IsWalkable = false;
        IsEmpty = true;
    }
    #endregion

    // Mouse + Click Events
    #region
    private void OnMouseOver()
    {
        LevelManager.Instance.mousedOverTile = this;
        OnTileMousedOver();
    }
    public void OnTileMousedOver()
    {
        if (PathRenderer.Instance.active && DefenderManager.Instance.selectedDefender != null)
        {
            Defender selectedDefender = DefenderManager.Instance.selectedDefender;

            if (selectedDefender.awaitingMoveOrder ||
                selectedDefender.awaitingChargeLocationOrder ||
                selectedDefender.awaitingDashOrder ||
                selectedDefender.awaitingGetDownOrder)
            {
                PathRenderer.Instance.DrawPath();
            }
        }
        


    }
    public void OnMouseDown()
    {
        LevelManager.Instance.selectedTile = this;
        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        if (selectedDefender != null && selectedDefender.awaitingMoveOrder == true)
        {
            Debug.Log("Starting Movement Process...");
            selectedDefender.StartMoveAbilityProcess(this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingChargeLocationOrder == true)
        {
            selectedDefender.StartChargeProcess(this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingMeteorOrder == true)
        {
            selectedDefender.StartMeteorProcess(this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingTelekinesisLocationOrder == true)
        {
            selectedDefender.StartTelekinesisProcess(selectedDefender.myCurrentTarget, this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingDashOrder == true)
        {
            selectedDefender.StartDashProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingGetDownOrder == true)
        {
            selectedDefender.StartGetDownProcess(this);
        }

    }
    #endregion

    
}
