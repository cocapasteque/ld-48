using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiggingManager : MonoBehaviour
{
    public float WinDuration = 60;

    public long Gems;
    public List<string> Suffixes;
    public long TotalMinedGems;
    public long LevelGems;
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

    public Button NextLevelButton;

    public TextMeshProUGUI GemText;
    public TextMeshProUGUI DwarfText;
    public TextMeshProUGUI QualityText;
    public TextMeshProUGUI SpeedText;

    public CanvasGroup ScreenFader;

    public CanvasGroup LoseScreen;
    public CanvasGroup WinScreen;

    public float FadeDuration;

    public GameObject CollectingText;
    public Vector2 StartCollectingPosition;
    public Transform MasterCanvas;

    public bool ActiveFader;

    private Vector2 CollectingWorldPos;
    private Image NextLevelImage;
    private TextMeshProUGUI faderText;
    private UIDrawer[] drawers;
    private bool mining;

    private float lastMiningTimestamp;

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
        drawers = FindObjectsOfType<UIDrawer>();
        faderText = ScreenFader.GetComponentInChildren<TextMeshProUGUI>();
        var faderTextCg = faderText.GetComponent<CanvasGroup>();
        NextLevelImage = NextLevelButton.GetComponent<Image>();
        Quality = 1f;
        Speed = .5f;
        Dwarves = 0;
        Depth = 1;

        StartCoroutine(InitialFade());
        
        IEnumerator InitialFade()
        {
            ActiveFader = true;
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
            ActiveFader = false;
            mining = true;
            StartCoroutine(Mining());
        }
    }

    private void Update()
    {
        GemText.text = FormatGemText(Gems);
        DwarfText.text = $"Dwarves: {Dwarves}";
        QualityText.text = $"Quality: {(int)Quality * 100}%";
        SpeedText.text = $"Motivation: {(int)(Speed * 100f)}%";

        NextLevelImage.fillAmount = (float)LevelGems / DepthCosts[Depth - 1];
        NextLevelButton.interactable = LevelGems >= DepthCosts[Depth - 1];

        if (TotalMinedGems >= 2 && mining)
        {
            if (Time.time - lastMiningTimestamp > WinDuration)
            {
                StartCoroutine(ShowWinScreen());
            }
        }
    }

    private string FormatGemText(long value)
    {
        int suffixIndex = 0;
        string gemText = value.ToString();
        while (gemText.ToString().Length > 5)
        {
            suffixIndex++;
            gemText = gemText.Substring(0, gemText.Length - 3);
        }
        return $"{gemText}{Suffixes[suffixIndex]}";
    }


    public void IncreaseDepth()
    {
        if (LevelGems >= DepthCosts[Depth - 1])
        {          
            if (Depth == DepthCosts.Count)
            {
                StartCoroutine(ShowLoseScreen());
            }
            else
                StartCoroutine(ChangeDepth());
        }

        IEnumerator ChangeDepth()
        {
            foreach (var drawer in drawers)
            {
                drawer.Close();
            }
            ActiveFader = true;
            Layout.GridSystem.Instance.SetBuilding(null);
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
            LevelGems = 0;
            Gems = 0;
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
            ActiveFader = false;
            lastMiningTimestamp = Time.time;
        }
    }

    private IEnumerator ShowLoseScreen()
    {
        mining = false;
        var t = 0f;
        var startTime = Time.time;
        LoseScreen.interactable = true;
        LoseScreen.blocksRaycasts = true;
        var loseText = LoseScreen.GetComponentInChildren<TextMeshProUGUI>();
        loseText.text = loseText.text.Replace("{GemCount}", TotalMinedGems.ToString());
        while (t < 1f)
        {
            t = (Time.time - startTime) / FadeDuration;
            LoseScreen.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
    }

    private IEnumerator ShowWinScreen()
    {
        mining = false;
        var t = 0f;
        var startTime = Time.time;
        WinScreen.interactable = true;
        WinScreen.blocksRaycasts = true;
        var winText = WinScreen.GetComponentInChildren<TextMeshProUGUI>();
        winText.text = winText.text.Replace("{GemCount}", TotalMinedGems.ToString());
        while (t < 1f)
        {
            t = (Time.time - startTime) / FadeDuration;
            WinScreen.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
    }

    public void PayGems(int amount)
    {
        Gems -= amount;
    }

    public void MineClicked()
    {
        var value = Mathf.RoundToInt(Quality * Mathf.Pow(5, Depth - 1));

        DisplayGemsCollected(value);
        
        Gems += value;
        TotalMinedGems += value;
        LevelGems += value;
    }

    public void DisplayGemsCollected(float value)
    {
        lastMiningTimestamp = Time.time;

        // Gems clicked text
        var instantiated = Instantiate(CollectingText, MasterCanvas);
        instantiated.transform.SetAsFirstSibling();
        instantiated.transform.localPosition = StartCollectingPosition;
        var tmp = instantiated.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = $"+{FormatGemText((long)value)}";
        tmp.DOFade(0, 1f);
        instantiated.GetComponentInChildren<Image>().DOFade(0, 1f);
        instantiated.transform.DOMoveY(CollectingWorldPos.y + 800, 1.2f);
        Destroy(instantiated, 2f);
    }

    public void IncreaseDwarves(int amount)
    {
        Dwarves += amount;
    }

    private IEnumerator Mining()
    {
        while(mining)
        {
            yield return new WaitForSeconds(1f / Speed);
            var value =  Mathf.RoundToInt(Dwarves * Mathf.Pow(5, Depth - 1));
            if(value > 0) DisplayGemsCollected(value);
            Gems += value;
            TotalMinedGems += value;
            LevelGems += value;
        }
    }
}