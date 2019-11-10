using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    private IEnumerator routine;
    private MoveQueue mq;
    public IEnumerator currentMovement;
    private bool ready = true;
    public void Start()
    {
        mq = gameObject.GetComponent<MoveQueue>();
        Debug.Log(mq);
    }
    public void Update()
    {
        if (!ready)    
        {
            return;
        }   
        if (mq.q.Count == 0) {
            return;
        }
        Vector3 target = mq.ConsumeMove();
        ready = false;
        StartCoroutine(MoveOverSpeed(gameObject, target, 1));
    }



    public IEnumerator MoveOverSpeed (GameObject objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        ready = true;
    }
    public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        ready = true;
    }
}
