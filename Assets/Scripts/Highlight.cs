using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private Color startcolor;
    private List<ColorAndRenderer> startcolors;
    private Color currentcolor;
    private bool selected;
    public RallyPoint rallyPoint;
    public void Start()
    {
        startcolors = new List<ColorAndRenderer>();
        startcolors.Add(new ColorAndRenderer(gameObject.GetComponent<Renderer>().material.color, gameObject.GetComponent<Renderer>()));
        foreach (Transform child in transform)
        {
            startcolors.Add(new ColorAndRenderer(child.gameObject.GetComponent<Renderer>().material.color, child.gameObject.GetComponent<Renderer>()));
        }
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
        foreach (ColorAndRenderer cor in startcolors)
        {
            Color highlight = cor.color;
            highlight.g += 0.2f;
            highlight.b += 0.2f;
            cor.rend.material.color = highlight;
        }
    }

    private void Unhighlight()
    {
        foreach (ColorAndRenderer cor in startcolors)
        {
            cor.rend.material.color = cor.color;
        }
    }

    public void PlaceFlag(int x, float y, int z, GameObject flag)
    {
        if (rallyPoint != null)
        {
            Destroy(rallyPoint.go);
        }

        GameObject clone = UnityEngine.Object.Instantiate(flag, flag.transform.position + new Vector3(x, y, z), flag.transform.rotation);
        //GameObject clone = UnityEngine.Object.Instantiate(flag, flag.transform.position + new Vector3(x, y, z), Quaternion.identity);
        RallyPoint flagObj = new RallyPoint(clone, x, z);
        rallyPoint = flagObj;
    }

    public void RemoveFlag()
    {
        if (rallyPoint != null)
        {
            Destroy(rallyPoint.go);
            rallyPoint = null;
        }
    }

    public void HideFlag()
    {
        if(rallyPoint != null)
        {
            rallyPoint.go.SetActive(false);
        }
    }
    public void ShowFlag()
    {
        if(rallyPoint != null)
        {
            rallyPoint.go.SetActive(true);
        }
    }
}


public class ColorAndRenderer
{
    public Color color;
    public Renderer rend;

    public ColorAndRenderer(Color color, Renderer rend)
    {
        this.color = color;
        this.rend = rend;
    }
}

public class RallyPoint
{
    public GameObject go;
    public int x;
    public int z;

    public RallyPoint(GameObject go, int x, int z)
    {
        this.go = go;
        this.x = x;
        this.z = z;
    }
}