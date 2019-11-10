using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveQueue : MonoBehaviour
{
    public Queue<Vector3> q = new Queue<Vector3>();

    public void AddMove(Vector3 target)
    {
        q.Enqueue(target);
    }

    public Vector3 ConsumeMove()
    {
        return q.Dequeue();
    }
}
