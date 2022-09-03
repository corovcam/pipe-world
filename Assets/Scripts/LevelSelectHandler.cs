using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelSelectHandler : MonoBehaviour
{
    public Sprite levelBtnBackground;
    public Sprite[] numbers;

    [SerializeField]
    private int levelsCount = 5; // There are currently 5 levels in Resources
    private GameObject canvasGO;
    private List<AudioSource> audioSources = new List<AudioSource>();

    private GameObject levelsGridGO;

    void Start()
    {
        audioSources.Add(MenuHandler.GenerateAudioSource("Sounds/click1", "Audio Click Source"));
        audioSources.Add(MenuHandler.GenerateAudioSource("Sounds/rollover1", "Audio Enter Source"));

        canvasGO = MenuHandler.GenerateCanvasGO("Level Select Canvas");

        GenerateTitleText();
        GenerateLevelsGrid();

        // Generate buttons equal to the number of levels in Resources
        for (int i = 1; i <= levelsCount; i++)
        {
            GenerateLevelButton(i);
        }
    }

    /// <summary>
    /// Used to generate and configure title in LevelSelect scene
    /// </summary>
    void GenerateTitleText()
    {
        // Title Text
        GameObject titleGO = new GameObject();
        titleGO.transform.parent = canvasGO.transform;
        titleGO.layer = canvasGO.layer;
        titleGO.name = "Level Select";

        TextMeshProUGUI textComp = titleGO.AddComponent<TextMeshProUGUI>();
        textComp.text = "LEVEL SELECT";
        textComp.font = (TMP_FontAsset)Resources.Load("UI/Electronic Highway Sign SDF");
        textComp.fontSize = 100;
        textComp.fontStyle = FontStyles.Bold;
        textComp.alignment = TextAlignmentOptions.Center;
        textComp.color = Color.white;

        // Title text position
        RectTransform transform = textComp.GetComponent<RectTransform>();
        transform.anchorMin = new Vector2(0, 0);
        transform.anchorMax = new Vector2(1, 1);
        transform.SetLeft(487);
        transform.SetTop(42);
        transform.SetRight(487);
        transform.SetBottom(842);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void GenerateLevelsGrid()
    {
        // Grid GO
        GameObject gridGO = new GameObject();
        gridGO.transform.parent = canvasGO.transform;
        gridGO.layer = canvasGO.layer;
        gridGO.name = "Levels Grid";
        levelsGridGO = gridGO;

        var grid = gridGO.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(200, 200);
        grid.spacing = new Vector2(150, 150);
        grid.childAlignment = TextAnchor.MiddleCenter;

        RectTransform transform = grid.GetComponent<RectTransform>();
        transform.anchorMin = new Vector2(0, 0);
        transform.anchorMax = new Vector2(1, 1);
        transform.SetLeft(315);
        transform.SetTop(289);
        transform.SetRight(315);
        transform.SetBottom(59);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void GenerateLevelButton(int levelNumber)
    {
        GameObject buttonGO = new GameObject();
        buttonGO.transform.parent = levelsGridGO.transform;
        buttonGO.layer = levelsGridGO.layer;
        buttonGO.name = levelNumber.ToString();

        // Image
        Image buttonImg = buttonGO.AddComponent<Image>();
        buttonImg.sprite = levelBtnBackground;
        buttonImg.color = new Color(214, 214, 214, 255);

        // Button
        Button buttonComp = buttonGO.AddComponent<Button>();
        buttonComp.targetGraphic = buttonImg;

        // Button Component position
        RectTransform transform = buttonComp.GetComponent<RectTransform>();
        transform.localScale = new Vector3(1, 1, 1);

        // Listeners
        buttonComp.onClick.AddListener(() => SelectLevelWithNumber(levelNumber));
        buttonComp.onClick.AddListener(audioSources[0].Play);

        // Event Trigger - Mouse enter
        EventTrigger trigger = buttonGO.AddComponent<EventTrigger>();
        EventTrigger.Entry triggerEntry = new EventTrigger.Entry();
        triggerEntry.eventID = EventTriggerType.PointerEnter;
        triggerEntry.callback.AddListener((data) => OnMouseEnterDelegate((PointerEventData)data));
        trigger.triggers.Add(triggerEntry);


        // Button Image Number
        GameObject imageNumberGO = new GameObject();
        imageNumberGO.transform.parent = buttonGO.transform;
        imageNumberGO.layer = buttonGO.layer;
        imageNumberGO.name = "Number" + levelNumber;

        Image numberImg = imageNumberGO.AddComponent<Image>();
        numberImg.sprite = numbers[levelNumber];
        numberImg.color = Color.white;

        // Button Text Component relative position
        transform = numberImg.GetComponent<RectTransform>();
        transform.localPosition = new Vector3(0, 0, 0);
        transform.sizeDelta = new Vector2(129, 159);
        transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Delegate to be fired when the mouse enters the corresponding area. Plays a Click sound.
    /// </summary>
    /// <param name="data">Unused/Not required parameter</param>
    public void OnMouseEnterDelegate(PointerEventData data)
    {
        audioSources[1].Play();
    }

    public void SelectLevelWithNumber(int number)
    {
        SceneHandler.LoadLevel(number);
    }
}
