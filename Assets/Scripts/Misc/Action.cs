using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    // This class is used by IEnumerator/Coroutines to send 'yield wait until' instructions back up the stack

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
