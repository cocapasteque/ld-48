using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiggingManager : MonoBehaviour
{
    public int Gems;
    public int Dwarves;
    
    public int Depth;
    public List<int> DepthCosts;

    public GameObject Mine;

    //Forge Upgrades
    public float Quality = 1f;
    //Tavern Upgrades
    public float Speed = 1f;

    public Text GemText;
    public Text DwarfText;
    public Text QualityText;
    public Text SpeedText;

    public static DiggingManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);

        Initialize();
    }

    private void Initialize()
    {
        Quality = 1f;
        Speed = 1f;
        Dwarves = 0;
        Depth = 1;
        StartCoroutine(Mining());
    }

    private void Update()
    {
        GemText.text = $"Gems: {Gems}";
        QualityText.text = $"Quality: {(int)Quality * 100}%";
        SpeedText.text = $"Motivation: {(int)Speed * 100}%";
    }

    public void IncreaseDepth()
    {
        if (Gems >= DepthCosts[Depth] && Depth < DepthCosts.Count - 1)
        {
            Gems -= DepthCosts[Depth];
            Depth++;
        }
    }

    public void PayGems(int amount)
    {
        Gems -= amount;
    }

    public void MineClicked()
    {
        Gems += Mathf.RoundToInt(Quality * Depth);
    }

    public void IncreaseDwarves(int amount)
    {
        Dwarves += amount;
    }

    private IEnumerator Mining()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f / Speed);
            //Todo: Balancing
            Gems += Mathf.RoundToInt(Dwarves * Depth);
        }
    }
}