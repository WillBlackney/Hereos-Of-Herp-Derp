using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldEncounter : MonoBehaviour
{    
    public enum EncounterType { NoType, BasicEnemy, EliteEnemy, Home, CampSite, Shop, Treasure, Mystery};

    [Header("Properties")]
    public EncounterType myEncounterType;
    public int column;
    public int position;
    public bool encounterReached;
    public Color32 baseColour;
    public Color32 occupiedColour;

    [Header("Component References")]
    public Image myEncounterTypeImage;
    public Image myGraphicMask;
    public Animator myAnimator;

    [Header("Encounter Type Images")]
    public Sprite basicEncounterImage;
    public Sprite eliteEncounterImage;
    public Sprite homeEncounterImage;
    public Sprite campSiteEncounterImage;
    public Sprite shopEncounterImage;
    public Sprite treasureEncounterImage;
    public Sprite mysteryEncounterImage;

    // Initialization + Setup
    #region
    public void InitializeSetup()
    {
        myAnimator = GetComponent<Animator>();        
        baseColour = myGraphicMask.color;

        if (WorldMap.Instance.generateRandomWorld)
        {
            SetRandomTileType(this);
        }

        SetHexagonSprite();
    }
    public void SetRandomTileType(WorldEncounter encounter)
    {
        int randomNumber = Random.Range(1, 101);

        // REMOVE THIS LATER: FOR TESTING CAMP SITE STATE BUFFS
        /*
        if(column == 1)
        {
            encounter.myEncounterType = EncounterType.CampSite;
            return;
        }
        */
        if (WorldMap.Instance.OnlySpawnMysteryEncounters)
        {
            encounter.myEncounterType = EncounterType.Mystery;
            return;
        }

        if (WorldMap.Instance.OnlySpawnBasicEncounters)
        {
            encounter.myEncounterType = EncounterType.BasicEnemy;
            return;
        }
        if (WorldMap.Instance.OnlySpawnCampSiteEncounters)
        {
            encounter.myEncounterType = EncounterType.CampSite;
            return;
        }
        if (WorldMap.Instance.OnlySpawnShopEncounters)
        {
            encounter.myEncounterType = EncounterType.Shop;
            return;
        }
        if (WorldMap.Instance.OnlySpawnEliteEncounters)
        {
            encounter.myEncounterType = EncounterType.EliteEnemy;
            return;
        }
        // prevent randomizing the home hexagon location
        if (myEncounterType == EncounterType.Home)
        {
            return;
        }

        if (randomNumber >= 1 && randomNumber <= 65)
        {
            int randomNumber2 = Random.Range(1, 101);

            if (randomNumber2 >= 1 && randomNumber2 <= 60)
            {
                encounter.myEncounterType = EncounterType.BasicEnemy;
            }
            else
            {
                //encounter.myEncounterType = EncounterType.Mystery;
                encounter.myEncounterType = EncounterType.BasicEnemy;
            }
        }

        // Try create camp site or shop, else basic enemy
        else if (randomNumber > 65 && randomNumber <= 100)
        {
            int randomNumber3 = Random.Range(1, 101);

            // camp site
            if (randomNumber3 < 33)
            {
                bool validPlaceForCampSite = true;
                if (encounter.column > 2)
                {
                    // Prevent rest sites from spawning consecutively                
                    foreach (WorldEncounter enc in WorldMap.Instance.allWorldEncounters)
                    {
                        if (
                            ((enc.column == column - 1) || (enc.column == column + 1)) &&
                            enc.myEncounterType == EncounterType.CampSite
                            )
                        {
                            //encounter.myEncounterType = EncounterType.BasicEnemy;
                            validPlaceForCampSite = false;
                            break;
                        }
                    }
                    if (validPlaceForCampSite)
                    {
                        encounter.myEncounterType = EncounterType.CampSite;
                    }
                    else
                    {
                        encounter.myEncounterType = EncounterType.BasicEnemy;
                    }


                }

                else
                {
                    encounter.myEncounterType = EncounterType.BasicEnemy;
                }
            }

            // Shop
            else if (randomNumber3 >= 33 && randomNumber3 < 66)
            {
                bool validPlaceForShopSite = true;
                if (encounter.column > 2)
                {
                    // Prevent shops from spawning consecutively                
                    foreach (WorldEncounter enc in WorldMap.Instance.allWorldEncounters)
                    {
                        if (
                            ((enc.column == column - 1) || (enc.column == column + 1)) &&
                            enc.myEncounterType == EncounterType.Shop
                            )
                        {
                            validPlaceForShopSite = false;
                            break;
                        }
                    }
                    if (validPlaceForShopSite)
                    {
                        encounter.myEncounterType = EncounterType.Shop;
                    }
                    else
                    {
                        encounter.myEncounterType = EncounterType.BasicEnemy;
                    }


                }

                else
                {
                    encounter.myEncounterType = EncounterType.BasicEnemy;
                }
            }

            // Elite
            else if (randomNumber3 >= 66)
            {
                bool validPlaceForEliteSite = true;
                if (encounter.column > 2)
                {
                    // Prevent shops from spawning consecutively                
                    foreach (WorldEncounter enc in WorldMap.Instance.allWorldEncounters)
                    {
                        if (
                            ((enc.column == column - 1) || (enc.column == column + 1)) &&
                            enc.myEncounterType == EncounterType.EliteEnemy
                            )
                        {
                            validPlaceForEliteSite = false;
                            break;
                        }
                    }
                    if (validPlaceForEliteSite)
                    {
                        encounter.myEncounterType = EncounterType.EliteEnemy;
                    }
                    else
                    {
                        encounter.myEncounterType = EncounterType.BasicEnemy;
                    }


                }

                else
                {
                    encounter.myEncounterType = EncounterType.BasicEnemy;
                }
            }

            /*
            int randomNumber3 = Random.Range(1, 101);

            if (randomNumber3 >= 1 && randomNumber3 <= 25)
            {
                encounter.myEncounterType = EncounterType.Shop;
            }
            else if (randomNumber3 >= 26 && randomNumber3 <= 50)
            {
                encounter.myEncounterType = EncounterType.EliteEnemy;
            }
            else if (randomNumber3 >= 51 && randomNumber3 <= 90)
            {
                encounter.myEncounterType = EncounterType.CampSite;
            }
            else 
            {
                encounter.myEncounterType = EncounterType.Treasure;
            }
            */
        }


        /*
        if (randomNumber >= 1 && randomNumber <= 30)
        {
            encounter.myEncounterType = EncounterType.BasicEnemy;
        }

        else if(randomNumber >= 31 && randomNumber<= 40)
        {
            encounter.myEncounterType = EncounterType.EliteEnemy;
        }

        else if (randomNumber >= 41 && randomNumber <= 50)
        {
            encounter.myEncounterType = EncounterType.Shop;
        }

        else if (randomNumber >= 51 && randomNumber <= 60)
        {
            encounter.myEncounterType = EncounterType.CampSite;
        }

        else if (randomNumber >= 61 && randomNumber <= 90)
        {
            encounter.myEncounterType = EncounterType.Mystery;
        }
        else if (randomNumber >= 91 && randomNumber <= 100)
        {
            encounter.myEncounterType = EncounterType.Treasure;
        }
        */
    }
    #endregion

    // Visibility + View Logic
    #region
    public void HighlightHexagon()
    {
        myAnimator.SetBool("Highlight", true);
    }
    public void UnHighlightHexagon()
    {        
        myAnimator.SetBool("Highlight", false);   
        if(encounterReached == true)
        {
            SetGraphicMaskColour(occupiedColour);
        }
        else
        {
            SetGraphicMaskColour(baseColour);
        }        
    }
    public void SetHexagonSprite()
    {
        if (myEncounterType == EncounterType.BasicEnemy)
        {
            myEncounterTypeImage.sprite = basicEncounterImage;
        }

        else if (myEncounterType == EncounterType.Home)
        {
            myEncounterTypeImage.sprite = homeEncounterImage;
        }

        else if (myEncounterType == EncounterType.CampSite)
        {
            myEncounterTypeImage.sprite = campSiteEncounterImage;
        }

        else if (myEncounterType == EncounterType.EliteEnemy)
        {
            myEncounterTypeImage.sprite = eliteEncounterImage;
        }

        else if (myEncounterType == EncounterType.Shop)
        {
            myEncounterTypeImage.sprite = shopEncounterImage;
        }

        else if (myEncounterType == EncounterType.Treasure)
        {
            myEncounterTypeImage.sprite = treasureEncounterImage;
        }

        else if (myEncounterType == EncounterType.Mystery)
        {
            myEncounterTypeImage.sprite = mysteryEncounterImage;
        }
    }
    public void SetGraphicMaskColour(Color32 newColour)
    {
        Debug.Log("Setting hexagon color");
        myGraphicMask.color = newColour;
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnEncounterButtonClicked()
    {
        List<WorldEncounter> viableEncounters = WorldMap.Instance.GetNextViableEncounters(WorldMap.Instance.playerPosition);

        // if were in the middle of an unfinished encounter already, or the encounter clicked on is not valid, return
        if( WorldMap.Instance.canSelectNewEncounter == false ||
            viableEncounters.Contains(this) == false)
        {
            return;
        }

        else if(myEncounterType == EncounterType.BasicEnemy)
        {
            WorldMap.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new basic enemy encounter event");            
            EventManager.Instance.StartNewBasicEncounterEvent();
        }

        else if (myEncounterType == EncounterType.CampSite)
        {
            WorldMap.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new rest site event");
            EventManager.Instance.StartNewRestSiteEncounterEvent();
        }

        else if (myEncounterType == EncounterType.EliteEnemy)
        {
            WorldMap.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new elite event");
            EventManager.Instance.StartNewEliteEncounterEvent();
        }

        else if (myEncounterType == EncounterType.Shop)
        {
            WorldMap.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new Shop event");
            EventManager.Instance.StartShopEncounterEvent();
        }

        else if (myEncounterType == EncounterType.Treasure)
        {
            WorldMap.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new Treasure Room event");
            EventManager.Instance.StartNewTreasureRoomEncounterEvent();
        }

        else if (myEncounterType == EncounterType.Mystery)
        {
            WorldMap.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new Mystery Room event");
            EventManager.Instance.StartNewMysteryEncounterEvent();
        }
    }
    #endregion







}
