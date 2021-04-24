using Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public string Name;
    public int Cost;
    public int Level;
    [HideInInspector] public Cell cell;

    protected virtual void Start()
    {
        cell = GetComponent<Cell>();
    }
}