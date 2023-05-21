using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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

        //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        //Debug.Log(Input.mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(Input.mousePosition));
        //Debug.Log(ray.origin + "; " + ray.direction);
        //if (Physics.Raycast(ray, out RaycastHit hit, 10000f))
        //{
        //    Vector3 worldPoint = hit.point;

        //    Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        //    Debug.Log(worldPoint);
        //    Debug.Log(hit.transform.gameObject.name);
        //}
    }
}
