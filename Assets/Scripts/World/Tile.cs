using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public enum TileType { None, Dirt, Grass, Forest, Rock, Water };
    public TileType myTileType;
    public Point GridPosition { get; set; }
    public bool IsEmpty;
    public bool IsWalkable;

    public Color32 originalColor;
    public Color32 highlightedColor = Color.white;

    private SpriteRenderer spriteRenderer;
    public Animator myAnimator;
    public bool Debugging { get; set; }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;
    }

    public Vector2 WorldPosition
    {
        get
        {
            // Gets the centre point of the tile
            // return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
            return new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f);
        }
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
        else if(myTileType== TileType.Water)
        {
            RunWaterTileSetup();
        }

        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    
    private void OnMouseOver()
    {
        LevelManager.Instance.mousedOverTile = this;
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

    public void ColorTile(Color newColor)
    {
        //Debug.Log("Coloring tile...");
        spriteRenderer.color = newColor;
    }

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
}
