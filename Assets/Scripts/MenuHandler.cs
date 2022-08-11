using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    private GameObject canvasGO;

    void Start()
    {
        // Generate Canvas
        canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.layer = 5;

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        GenerateBtn("Level Select", 0, 150, SceneHandler.LoadLevelSelectScene);
        GenerateBtn("Arcade", 0, -150, SceneHandler.LoadArcadeGameScene);

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

    void GenerateBtn(string txt, int posX, int posY, UnityAction onClickFunc)
    {
        GameObject buttonGO = new GameObject();
        buttonGO.transform.parent = canvasGO.transform;
        buttonGO.layer = canvasGO.layer;
        buttonGO.name = txt.Replace(" ", "");

        Button buttonComp = buttonGO.AddComponent<Button>();
        buttonGO.AddComponent<CanvasRenderer>();
        Image buttonImg = buttonGO.AddComponent<Image>();
        buttonImg.sprite = Resources.Load<Sprite>("UI/tileBlue_01");
        buttonImg.color = new Color(100, 249, 255, 255);
        buttonComp.targetGraphic = buttonImg;

        RectTransform transform = buttonComp.GetComponent<RectTransform>();
        transform.localPosition = new Vector3(posX, posY, 0);
        transform.sizeDelta = new Vector2(160, 30);
        transform.localScale = new Vector3(4.5f, 4.5f, 0);

        buttonComp.onClick.AddListener(onClickFunc);

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

        transform = textComp.GetComponent<RectTransform>();
        transform.localPosition = new Vector3(0, 0, 0);
        transform.sizeDelta = new Vector2(0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }
}
