using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public List<string> StoryTexts;

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

    public GameObject CollectingText;
    public Vector2 StartCollectingPosition;
    public Transform MasterCanvas;
    private Vector2 CollectingWorldPos;
    
    private TextMeshProUGUI faderText;

    public static DiggingManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);

        Initialize();
        CollectingWorldPos = Camera.main.ScreenToWorldPoint(StartCollectingPosition);
    }

    private void Initialize()
    {
        faderText = ScreenFader.GetComponentInChildren<TextMeshProUGUI>();
        var faderTextCg = faderText.GetComponent<CanvasGroup>();
        Quality = 1f;
        Speed = 1f;
        Dwarves = 0;
        Depth = 1;

        StartCoroutine(InitialFade());
        
        IEnumerator InitialFade()
        {
            yield return new WaitForSeconds(1f);

            float t = 0f;
            float startTime = Time.time;
            faderText.text = StoryTexts[0];

            while (t < 1)
            {
                t = (Time.time - startTime) / FadeDuration;
                faderTextCg.alpha = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }

            yield return new WaitForSeconds(3f);
            
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

            StartCoroutine(Mining());
        }
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
            faderText.text = StoryTexts[Depth];
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

            yield return new WaitForSeconds(3f);

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
        var value = Mathf.RoundToInt(Quality * Depth);

        DisplayGemsCollected(value);
        
        Gems += value;
    }

    public void DisplayGemsCollected(float value)
    {
        // Gems clicked text
        var instantiated = Instantiate(CollectingText, MasterCanvas);
        instantiated.transform.localPosition = StartCollectingPosition;
        var tmp = instantiated.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = $"+{value}";
        tmp.DOFade(0, 1f);
        instantiated.GetComponentInChildren<Image>().DOFade(0, 1f);
        instantiated.transform.DOMoveY(CollectingWorldPos.y + 800, 1.2f);
        Destroy(instantiated, 1f);
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
            var value =  Mathf.RoundToInt(Dwarves * Depth);
            DisplayGemsCollected(value);
            Gems += value;

        }
    }
}