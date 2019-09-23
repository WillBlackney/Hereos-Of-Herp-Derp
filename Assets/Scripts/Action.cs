using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    public bool actionResolved;
    public bool ActionResolved()
    {
        if(actionResolved == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
