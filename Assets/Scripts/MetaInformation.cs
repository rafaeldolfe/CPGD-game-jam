using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaInformation : MonoBehaviour
{
    public int x { get; set; }
    public int z { get; set; }

    public void init(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
