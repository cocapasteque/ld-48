using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BuildingCanvas : MonoBehaviour
{
    public CanvasGroup LeftButton;
    public float Distance;
    
    private float t = 0;
    private Building building;
    private IEnumerator coroutine;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        building = GetComponentInParent<Building>();

        LeftButton.transform.localPosition = Vector3.zero;
        LeftButton.GetComponent<CanvasGroup>().alpha = 0;
        LeftButton.interactable = false;

        if (building.UpgradedBuilding != null)
        {
            LeftButton.GetComponent<Button>().onClick.AddListener(Upgrade);
        }
        else
        {
            LeftButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (building.UpgradedBuilding != null)
        {
            LeftButton.GetComponent<Button>().interactable = DiggingManager.Instance.Gems >= building.UpgradedBuilding.Cost;
        }
    }

    private void Upgrade()
    {
        building.Upgrade();
    }

    public void Show(bool show)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = Move(show);
        StartCoroutine(coroutine);
    }

    private IEnumerator Move(bool show)
    {
        if (!show)
        {
            LeftButton.interactable = false;
        }
        t = Mathf.InverseLerp(show ? 0f : -Distance, show ? -Distance : 0f, LeftButton.transform.localPosition.x);

        while (t <= 1)
        {
            t += Layout.GridSystem.Instance.CanvasMoveSpeed * Time.deltaTime;
            LeftButton.transform.localPosition = new Vector3(Mathf.Lerp(show ? 0f : -Distance, show ? -Distance : 0f, t), 0f, 0f);
            LeftButton.alpha = show ? t : (1 - t);
            yield return null;
        }
        if (show)
        {
            LeftButton.interactable = true;
        }
    }
}