using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private Color startcolor;
    private Color currentcolor;
    private bool selected;
    public GameObject rallyPoint;
    public void Start()
    {
        startcolor = gameObject.GetComponent<Renderer>().material.color;
    }
    public void Select()
    {
        if (selected)
        {
            selected = false;
            Unhighlight();
        }
        else
        {
            selected = true;
            SetHighlight();
        }
    }

    public void Deselect()
    {
        selected = false;
        Unhighlight();
    }

    private void SetHighlight()
    {
        Color highlight = startcolor;
        highlight.g += 0.2f;
        highlight.b += 0.2f;
        gameObject.GetComponent<Renderer>().material.color = highlight;
    }

    private void Unhighlight()
    {
        gameObject.GetComponent<Renderer>().material.color = startcolor;
    }

    public void PlaceFlag(int x, int z, GameObject flag)
    {
        if (rallyPoint)
        {
            Destroy(rallyPoint);
        }

        GameObject clone = UnityEngine.Object.Instantiate(flag, flag.transform.position + new Vector3(x, 0, z), Quaternion.identity);

        rallyPoint = clone;
    }

    public void RemoveFlag(Vector3 pos, GameObject flag)
    {
        Destroy(rallyPoint);
    }

    public void HideFlag()
    {
        if(rallyPoint)
        {
            rallyPoint.SetActive(false);
        }
    }
    public void ShowFlag()
    {
        if(rallyPoint)
        {
            rallyPoint.SetActive(true);
        }
    }
}
