using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{

    private int state = Constants.IDLE;

    public void SetState(int state)
    {
        this.state = state;
    }
    public int GetState()
    {
        return state;
    }
}
