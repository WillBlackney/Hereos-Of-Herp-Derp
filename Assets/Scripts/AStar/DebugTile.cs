using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTile : MonoBehaviour
{
    [SerializeField] private Text f;
    [SerializeField] private Text g;
    [SerializeField] private Text h;

    public Text F
    {
        get
        {
            f.gameObject.SetActive(true);
            return f;
        }
        set { this.f = value; }
    }

    public Text G
    {
        get
        {
            g.gameObject.SetActive(true);
            return g;
        }
        set { this.g = value; }
    }

    public Text H
    {
        get
        {
            h.gameObject.SetActive(true);
            return h;
        }
        set { this.h = value; }
    }

}
