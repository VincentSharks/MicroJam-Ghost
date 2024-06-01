using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cooking : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
    private bool _cooking;
    private float _cookingTime = 10f;
    private float _cookingTimer = 0;
    private float _cookingTolerance = 5f;

    public List<string> IngredientsInPot = new List<string>();
    public CookedLevel CookedLvl;

    private Vector3 _originalPos;

    private void Awake()
    {
        _originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (_cooking)
        {
            Debug.Log("Cooking");
            _cookingTimer += Time.deltaTime;

            //if timer is between 10 and 15
            if (_cookingTimer >= _cookingTime && _cookingTimer <= _cookingTime + _cookingTolerance)
            {
                Debug.Log("Cooked");
                CookedLvl = CookedLevel.Cooked;
            }
            else if (_cookingTimer > _cookingTime + _cookingTolerance)
            {
                Debug.Log("overCooked");
                CookedLvl = CookedLevel.OverCooked;
            }
        }
    }

    public void OnIngredientDropped(string name)
    {
        if (_cooking) return;

        if (IngredientsInPot.Count < GameManager.Instance.ActiveCustomer.IngredientsCount)
        {
            IngredientsInPot.Add(name);
        }
        if (IngredientsInPot.Count >= GameManager.Instance.ActiveCustomer.IngredientsCount)
        {
            _cookingTimer = 0;
            _cooking = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cooking = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.Translate(mousePosition);
    }

    public void OnDrop(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.back);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Dish>() != null)
            {
                hit.collider.gameObject.GetComponent<Dish>().OnCookingPotDropped(IngredientsInPot, CookedLvl);
            }
            else
            {
                _cooking = true;
            }
        }
        else
        {
            _cooking = true;
        }
        transform.localPosition = _originalPos;
    }
}

public enum CookedLevel
{
    UnderCooked,
    Cooked,
    OverCooked
}
