using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiggingManager : MonoBehaviour
{
    public int Gems;
    public int Dwarves;
    
    public int Depth;
    public List<int> DepthCosts;
    public List<GameObject> DepthTiles;

    public GameObject Mine;

    //Forge Upgrades
    public float Quality = 1f;
    //Tavern Upgrades
    public float Speed = 1f;

    public TextMeshProUGUI GemText;
    public TextMeshProUGUI DwarfText;
    public TextMeshProUGUI QualityText;
    public TextMeshProUGUI SpeedText;

    public CanvasGroup ScreenFader;
    public float FadeDuration;

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
        GemText.text = $"{Gems}";
        DwarfText.text = $"Dwarves: {Dwarves}";
        QualityText.text = $"Quality: {(int)Quality * 100}%";
        SpeedText.text = $"Motivation: {(int)(Speed * 100f)}%";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseDepth();
        }
    }

    public void IncreaseDepth()
    {
        if (Gems >= DepthCosts[Depth - 1] && Depth < DepthCosts.Count)
        {
            Gems -= DepthCosts[Depth - 1];
            StartCoroutine(ChangeDepth());
        }

        IEnumerator ChangeDepth()
        {
            var t = 0f;
            var startTime = Time.time;
            ScreenFader.interactable = true;
            ScreenFader.blocksRaycasts = true;
            while (t < 1f)
            {
                t = (Time.time - startTime) / FadeDuration;
                ScreenFader.alpha = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }

            DepthTiles[Depth - 1].SetActive(false);
            Depth++;
            DepthTiles[Depth - 1].SetActive(true);
            Layout.GridSystem.Instance.ClearGrid();

            yield return new WaitForSeconds(0.25f);

            t = 0f;
            startTime = Time.time;
            while (t < 1f)
            {
                t = (Time.time - startTime) / FadeDuration;
                ScreenFader.alpha = Mathf.Lerp(1f, 0f, t);
                yield return null;
            }
            ScreenFader.interactable = false;
            ScreenFader.blocksRaycasts = false;
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