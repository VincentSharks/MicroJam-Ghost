using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Vector3 _originalPos;
    [SerializeField] private Animation _dropAnimation;
    [SerializeField] StudioEventEmitter _waterSplashEmitter;
    private bool interactible = true;

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

        if (interactible && !GameManager.Instance.Cooking.IsCooking)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
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

            StartCoroutine(PlayAnimation());
        }
        transform.localPosition = _originalPos;
    }

    private IEnumerator PlayAnimation()
    {
        _dropAnimation.Play();
        interactible = false;

        yield return new WaitForSeconds(_dropAnimation.clip.length);

        interactible = true;
        _waterSplashEmitter.Play();
        _dropAnimation.Stop();
        transform.localPosition = _originalPos;
    }
}
