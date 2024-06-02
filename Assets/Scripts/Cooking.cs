using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cooking : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler 
{ 
    public bool IsCooking = false;
    private float _cookingTime = 10f;
    private float _cookingTimer = 0;
    private float _cookingTolerance = 5f;

    public List<string> IngredientsInPot = new List<string>();
    public CookedLevel CookedLvl;

    public List<Sprite> StoveLights;
    public List<Image> StoveLightImages;

    private Vector3 _originalPos;

    [SerializeField] private StudioEventEmitter _boilingEmitter;

    private void Awake()
    {
        _originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (IsCooking && IngredientsInPot.Count == GameManager.Instance.ActiveCustomer.IngredientsCount)
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

            var oneThird = _cookingTime * 0.33f;
            var twoThirds = _cookingTime * .66f;

            if (_cookingTimer > 0 && _cookingTimer <= oneThird) StoveLightImages[0].sprite = StoveLights[1];
            else if (_cookingTimer > oneThird && _cookingTimer <= twoThirds) StoveLightImages[1].sprite = StoveLights[1];
            else if (_cookingTimer > twoThirds && _cookingTimer <= _cookingTime) StoveLightImages[2].sprite = StoveLights[1];
            else if (_cookingTimer >= _cookingTime && _cookingTimer < _cookingTime + _cookingTolerance/2)
            {
                for (int i = 0; i < StoveLightImages.Count; i++)
                {
                    StoveLightImages[i].sprite = StoveLights[0];
                }
            }
            else if (_cookingTimer >= _cookingTime + _cookingTolerance / 2 && _cookingTimer < _cookingTime + _cookingTolerance)
            {
                for (int i = 0; i < StoveLightImages.Count; i++)
                {
                    StoveLightImages[i].sprite = StoveLights[2];
                }
            }
            else if (_cookingTimer >= _cookingTime + _cookingTolerance)
            {
                for (int i = 0; i < StoveLightImages.Count; i++)
                {
                    StoveLightImages[i].sprite = StoveLights[3];
                }
            }
        }
    }

    public void OnIngredientDropped(string name)
    {
        if (IsCooking) return;

        if (IngredientsInPot.Count < GameManager.Instance.ActiveCustomer.IngredientsCount)
        {
            IngredientsInPot.Add(name);
        }
        if (IngredientsInPot.Count >= GameManager.Instance.ActiveCustomer.IngredientsCount)
        {
            _cookingTimer = 0;
            IsCooking = true;
            _boilingEmitter.Play();
            this.gameObject.layer = 0;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.GameStarted || IngredientsInPot.Count != GameManager.Instance.ActiveCustomer.IngredientsCount) return;

        IsCooking = false;
        _boilingEmitter.Stop();
        this.gameObject.layer = 2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.GameStarted || IngredientsInPot.Count != GameManager.Instance.ActiveCustomer.IngredientsCount) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.Translate(mousePosition);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.Instance.GameStarted || IngredientsInPot.Count != GameManager.Instance.ActiveCustomer.IngredientsCount) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.back);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Dish>() != null)
            {
                hit.collider.gameObject.GetComponent<Dish>().OnCookingPotDropped(IngredientsInPot, CookedLvl);
                IsCooking = false;
                _boilingEmitter.Stop();
                this.gameObject.layer = 0;
            }
            else
            {
                IsCooking = true;
                _boilingEmitter.Play();
                this.gameObject.layer = 0;
            }
        }
        else
        {
            IsCooking = true;
            _boilingEmitter.Play();
            this.gameObject.layer = 0;
        }
        transform.localPosition = _originalPos;
    }

    public void ResetValues()
    {
        IngredientsInPot = new List<string>();
        _cookingTimer = 0;
        IsCooking = false;
        CookedLvl = CookedLevel.UnderCooked;

        foreach(var light in StoveLightImages)
        {
            light.sprite = StoveLights[0];
        }
    }
}

public enum CookedLevel
{
    UnderCooked,
    Cooked,
    OverCooked
}
