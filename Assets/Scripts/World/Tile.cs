using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public enum TileType { None, Dirt, Grass, Tree, Water };

    [Header("Component References")]
    private SpriteRenderer mySpriteRenderer;
    public Animator myAnimator;

    [Header("Properties")]
    public TileType myTileType;
    public bool IsEmpty;
    public bool IsWalkable;
    public bool BlocksLoS;
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
        BlocksLoS = false;
    }
    public void RunGrassTileSetup()
    {
        IsWalkable = true;
        IsEmpty = true;
        BlocksLoS = false;
    }   
    public void RunTreeTileSetup()
    {
        IsWalkable = false;
        IsEmpty = false;
        BlocksLoS = true;
    }
    public void RunWaterTileSetup()
    {
        IsWalkable = false;
        IsEmpty = false;
        BlocksLoS = false;
    }
    #endregion

    // Mouse + Click Events
    #region
    private void OnMouseOver()
    {
        //LevelManager.Instance.mousedOverTile = this;
        //OnTileMouseEnter();
    }
    private void OnMouseEnter()
    {
        LevelManager.Instance.mousedOverTile = this;
        OnTileMouseEnter();
    }
    public void OnTileMouseEnter()
    {       

        if (DefenderManager.Instance.selectedDefender != null)
        {
            Defender selectedDefender = DefenderManager.Instance.selectedDefender;

            if (selectedDefender.awaitingMoveOrder ||
                selectedDefender.awaitingChargeLocationOrder ||
                selectedDefender.awaitingDashOrder ||
                selectedDefender.awaitingGetDownOrder)
            {
                if (PathRenderer.Instance.active)
                {
                    PathRenderer.Instance.DrawPath();
                }
                
            }
            else if(selectedDefender.awaitingAnOrder)
            {
                TargetingPathRenderer.Instance.DrawPath();
            }
        }
        


    }
    public void OnMouseDown()
    {
        LevelManager.Instance.selectedTile = this;
        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        // check consumables first
        if (ConsumableManager.Instance.awaitingFireBombTarget ||
            ConsumableManager.Instance.awaitingDynamiteTarget ||
            ConsumableManager.Instance.awaitingPoisonGrenadeTarget ||
            ConsumableManager.Instance.awaitingBottledFrostTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(this);
        }

        else if (ConsumableManager.Instance.awaitingBlinkPotionDestinationTarget &&
            IsWalkable &&
            IsEmpty &&
            LevelManager.Instance.GetTilesWithinRange(2, ConsumableManager.Instance.blinkPotionTarget.tile).Contains(this))
        {
            ConsumableManager.Instance.PerformBlinkPotion(this);
        }

        // Check abilities second
        else if (selectedDefender != null && selectedDefender.awaitingMoveOrder == true)
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
            selectedDefender.StartMeteorProcess(LevelManager.Instance.selectedTile);
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlizzardOrder == true)
        {
            selectedDefender.StartBlizzardProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingToxicEruptionOrder == true)
        {
            selectedDefender.StartToxicEruptionProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlindingLightOrder == true)
        {
            selectedDefender.StartBlindingLightProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingConcealingCloudsOrder == true)
        {
            selectedDefender.StartConcealingCloudsProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingDragonBreathOrder == true)
        {
            selectedDefender.StartDragonBreathProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingRainOfChaosOrder == true)
        {
            selectedDefender.StartRainOfChaosProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingThunderStormOrder == true)
        {
            selectedDefender.StartThunderStormProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingTelekinesisLocationOrder == true)
        {
            selectedDefender.StartTelekinesisProcess(selectedDefender.myCurrentTarget, this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlinkOrder == true)
        {
            selectedDefender.StartBlinkProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingTreeLeapOrder == true)
        {
            selectedDefender.StartTreeLeapProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingPhoenixDiveOrder == true)
        {
            selectedDefender.StartPhoenixDiveProcess(this);
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
