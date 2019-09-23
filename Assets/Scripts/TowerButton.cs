using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int goldPrice;
    [SerializeField] private Text priceText;

    private void Start()
    {
        //priceText.text = GoldPrice + "$".ToString();
    }

    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }       
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }        
    }

    public int GoldPrice
    {
        get
        {
            return goldPrice;
        }        
    }
}
