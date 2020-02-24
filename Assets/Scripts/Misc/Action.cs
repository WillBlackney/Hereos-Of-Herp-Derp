using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    // This class is used by IEnumerator/Coroutines to send 'yield wait until' instructions back up the stack
    public bool combatAction;

    public bool actionResolved;
    public bool ActionResolved()
    {
        if(actionResolved == true)
        {
            ActionManager.Instance.RemoveActionFromQueue(this);
            return true;
        }
        else
        {
            return false;
        }
    }

    public Action(bool _combatAction = false)
    {
        combatAction = _combatAction;
        if (combatAction)
        {
            ActionManager.Instance.AddActionToQueue(this);
        }

    }
}
