using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    private GameObject canvasGO;
    private List<AudioSource> audioSources = new List<AudioSource>();

    void Start()
    {
        audioSources.Add(GenerateAudioSource("Sounds/click1", "Audio Click Source"));
        audioSources.Add(GenerateAudioSource("Sounds/rollover1", "Audio Enter Source"));

        canvasGO = GenerateCanvasGO("Main Menu Canvas");

        GenerateBtn("Level Select", 0, 150, SceneHandler.LoadLevelSelectScene);
        GenerateBtn("Arcade", 0, -150, SceneHandler.LoadArcadeGameScene);

        GenerateTitleText();
    }

    public static GameObject GenerateCanvasGO(string canvasName)
    {
        GameObject tempCanvasGO = new GameObject();
        tempCanvasGO.name = canvasName;
        tempCanvasGO.layer = 5;

        // Rendering options
        Canvas canvas = tempCanvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        CanvasScaler cs = tempCanvasGO.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        cs.matchWidthOrHeight = 0.5f;
        tempCanvasGO.AddComponent<GraphicRaycaster>();

        return tempCanvasGO;
    }

    void GenerateBtn(string txt, int posX, int posY, UnityAction onClickFunc)
    {
        GameObject buttonGO = new GameObject();
        buttonGO.transform.parent = canvasGO.transform;
        buttonGO.layer = canvasGO.layer;
        buttonGO.name = txt.Replace(" ", "");

        // Image
        Button buttonComp = buttonGO.AddComponent<Button>();
        buttonGO.AddComponent<CanvasRenderer>();
        Image buttonImg = buttonGO.AddComponent<Image>();
        buttonImg.sprite = Resources.Load<Sprite>("UI/tileBlue_01");
        buttonImg.color = new Color(100, 249, 255, 255);
        buttonComp.targetGraphic = buttonImg;

        // Button Component position
        RectTransform transform = buttonComp.GetComponent<RectTransform>();
        transform.localPosition = new Vector3(posX, posY, 0);
        transform.sizeDelta = new Vector2(160, 30);
        transform.localScale = new Vector3(4.5f, 4.5f, 0);

        // Listeners
        buttonComp.onClick.AddListener(onClickFunc);
        buttonComp.onClick.AddListener(audioSources[0].Play);

        // Event Trigger - Mouse enter
        EventTrigger trigger = buttonGO.AddComponent<EventTrigger>();
        EventTrigger.Entry triggerEntry = new EventTrigger.Entry();
        triggerEntry.eventID = EventTriggerType.PointerEnter;
        triggerEntry.callback.AddListener((data) => OnMouseEnterDelegate((PointerEventData)data));
        trigger.triggers.Add(triggerEntry);

        // Button Text
        GameObject textGO = new GameObject();
        textGO.transform.parent = buttonGO.transform;
        textGO.layer = buttonGO.layer;
        textGO.name = "Text";

        TextMeshProUGUI textComp = textGO.AddComponent<TextMeshProUGUI>();
        textComp.text = txt;
        textComp.fontSize = 20;
        textComp.alignment = TextAlignmentOptions.Center;
        textComp.enableWordWrapping = false;
        textComp.color = Color.black;

        // Button Text Component relative position
        transform = textComp.GetComponent<RectTransform>();
        transform.localPosition = new Vector3(0, 0, 0);
        transform.sizeDelta = new Vector2(0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void GenerateTitleText()
    {
        // Title Text
        GameObject titleGO = new GameObject();
        titleGO.transform.parent = canvasGO.transform;
        titleGO.layer = canvasGO.layer;
        titleGO.name = "Plumber";

        TextMeshProUGUI textComp = titleGO.AddComponent<TextMeshProUGUI>();
        textComp.text = "Plumber";
        textComp.font = (TMP_FontAsset)Resources.Load("UI/Electronic Highway Sign SDF");
        textComp.fontSize = 40;
        textComp.fontStyle = FontStyles.Bold;
        textComp.alignment = TextAlignmentOptions.Center;
        textComp.enableWordWrapping = false;
        textComp.color = Color.white;

        // Title text position
        RectTransform transform = textComp.GetComponent<RectTransform>();
        transform.localPosition = new Vector3(0, 400, 0);
        transform.sizeDelta = new Vector2(200, 50);
        transform.localScale = new Vector3(2.5f, 2.5f, 0);
    }

    public void OnMouseEnterDelegate(PointerEventData data)
    {
        audioSources[1].Play();
    }

    public static AudioSource GenerateAudioSource(string audioPath, string GOName)
    {
        GameObject audioGO = new GameObject();
        audioGO.name = GOName;

        AudioSource audioSrc = audioGO.AddComponent<AudioSource>();
        audioSrc.clip = (AudioClip)Resources.Load(audioPath);

        return audioSrc;
    }
}
