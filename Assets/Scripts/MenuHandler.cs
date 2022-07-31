using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
        canvasGO.AddComponent<Canvas>();

        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        GenerateBtn("Level Select", 0, 150);
        GenerateBtn("Arcade", 0, -150);

        //// Text
        //myText = new GameObject();
        //myText.transform.parent = canvasGO.transform;
        //myText.name = "wibble";

        //text = myText.AddComponent<Text>();
        //text.font = (Font)Resources.Load("MyFont");
        //text.text = "wobble";
        //text.fontSize = 100;

        //// Text position
        //rectTransform = text.GetComponent<RectTransform>();
        //rectTransform.localPosition = new Vector3(0, 0, 0);
        //rectTransform.sizeDelta = new Vector2(400, 200);
    }

    void GenerateBtn(string txt, int posX, int posY)
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

        // Text
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
        textGO.AddComponent<CanvasRenderer>();

        transform = textComp.GetComponent<RectTransform>();
        transform.localPosition = new Vector3(0, 0, 0);
        transform.sizeDelta = new Vector2(0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }
}
