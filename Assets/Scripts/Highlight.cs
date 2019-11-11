﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private Color startcolor;
    private Color currentcolor;
    private bool selected;
    public RallyPoint rallyPoint;
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

    public void PlaceFlag(int x, float y, int z, GameObject flag)
    {
        if (rallyPoint != null)
        {
            Destroy(rallyPoint.go);
        }

<<<<<<< HEAD
        GameObject clone = UnityEngine.Object.Instantiate(flag, flag.transform.position + new Vector3(x, 0, z), flag.transform.rotation);
=======
        GameObject clone = UnityEngine.Object.Instantiate(flag, flag.transform.position + new Vector3(x, y, z), Quaternion.identity);
>>>>>>> eee5841dfb7c6da07ff1eccb17d3dbf58ed44099
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