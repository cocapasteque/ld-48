using Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public string Name;
    public int Cost;
    public Building UpgradedBuilding;
    [HideInInspector] public Cell cell;

    protected virtual void Start()
    {
        cell = GetComponent<Cell>();
    }

    public void Upgrade()
    {
        if (UpgradedBuilding != null)
        {
            DiggingManager.Instance.PayGems(UpgradedBuilding.Cost);
            GridSystem.Instance.grid.SetCell(UpgradedBuilding.gameObject, cell.x, cell.y);
        }
    }

    public void Sell()
    {
        DiggingManager.Instance.PayGems(-Cost / 2);
        GridSystem.Instance.grid.RemoveBuilding(cell.x, cell.y);
    }
}