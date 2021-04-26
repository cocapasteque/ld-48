using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Building BuildingPrefab;
    public TextMeshProUGUI ButtonText;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();       
        button.onClick.AddListener(SelectBuilding);     
    }

    private void OnEnable()
    {
        ButtonText.text = $"{BuildingPrefab.Name} ({BuildingPrefab.Cost * DiggingManager.Instance.Depth})";
    }

    private void Update()
    {
        button.interactable = DiggingManager.Instance.Gems >= BuildingPrefab.Cost * DiggingManager.Instance.Depth;
    }

    private void SelectBuilding()
    {
        Layout.GridSystem.Instance.SetBuilding(BuildingPrefab);
        GetComponentInParent<UIDrawer>().Toggle();
    }
}