using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector]
    public Transform parentAfterDrag;
    
    public GameObject pipePrefab;
    public Pipe pipeType;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        var rt = GetComponent<RectTransform>();
        rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0);
        image.raycastTarget = true;

        // Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);

        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        Debug.Log(Input.mousePosition);
        List<RaycastResult> touches = new();
        EventSystem.current.RaycastAll(eventData, touches);
        //Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(Input.mousePosition));
        //Debug.Log(ray.origin + "; " + ray.direction);
        //RaycastHit2D[] touches = Physics2D.RaycastAll(
        //    Camera.main.WorldToScreenPoint(Input.mousePosition),
        //    Camera.main.transform.forward);
        //Debug.DrawRay(Camera.main.WorldToScreenPoint(Input.mousePosition), Camera.main.transform.forward, Color.red);
        foreach (var hit in touches)
        {
            if (hit.gameObject.CompareTag("Pipe"))
            {
                Debug.Log(hit.gameObject.name);
                var pipe = hit.gameObject.GetComponent<PipeHandler>();
                if (pipe.pipeType.Type == PipeType.EMPTY)
                {
                    var pipeGO = Instantiate(pipePrefab, pipe.transform);
                    pipeGO.transform.localPosition = Vector3.zero;
                    //pipeGO = pipe.GetComponent<Pipe>();
                    //pipe.pipe.pipeType = pipeType;
                    Destroy(pipe.gameObject);
                }
            }
            Debug.Log(hit);
            Debug.Log(hit.gameObject.name);
        }
    }
}
