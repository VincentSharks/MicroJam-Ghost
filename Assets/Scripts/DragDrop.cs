using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Vector3 _originalPos;

    private void Awake()
    {
        _originalPos = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begindrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("ondrag");

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.Translate(mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("enddrag");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.back);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.back);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Cooking>() != null) hit.collider.gameObject.GetComponent<Cooking>().OnIngredientDropped(this.gameObject.name);

            //Debug.Log(hit.collider.gameObject.name);
        }
        transform.localPosition = _originalPos;
    }
}
