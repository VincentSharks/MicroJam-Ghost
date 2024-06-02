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

    private bool _canPlayFoodDoneSFX = false;

    public List<string> IngredientsInPot = new List<string>();
    public CookedLevel CookedLvl;

    public List<Sprite> StoveLights;
    public List<Image> StoveLightImages;

    [SerializeField] private GameObject _boilingFxObj;
    [SerializeField] private GameObject _fireFXObj;
    [SerializeField] private GameObject _waterSplashFXObj;

    private Vector3 _originalPos;

    [SerializeField] private StudioEventEmitter _boilingEmitter;
    [SerializeField] private StudioEventEmitter _foodDoneEmitter;

    [SerializeField] private Animator _cookingPotAnimator;

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
                if (!_canPlayFoodDoneSFX)
                {
                    _canPlayFoodDoneSFX = true;
                    _foodDoneEmitter.Play();
                } 
                
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
                if (!_fireFXObj.activeInHierarchy) _fireFXObj.SetActive(true);

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

        StartCoroutine(PlaySplashFX());

        if (IngredientsInPot.Count < GameManager.Instance.ActiveCustomer.IngredientsCount)
        {
            IngredientsInPot.Add(name);
        }
        if (IngredientsInPot.Count >= GameManager.Instance.ActiveCustomer.IngredientsCount)
        {
            _cookingTimer = 0;
            IsCooking = true;
            _cookingPotAnimator.SetBool("Cooking", true);
            _boilingFxObj.SetActive(true);
            _boilingEmitter.Play();
            this.gameObject.layer = 0;
        }

        GameManager.Instance.UpdateCookingIcon(name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.GameStarted || IngredientsInPot.Count != GameManager.Instance.ActiveCustomer.IngredientsCount) return;

        var splashRenderer = _waterSplashFXObj.GetComponent<SpriteRenderer>();
        splashRenderer.sprite = null;

        IsCooking = false;
        _cookingPotAnimator.SetBool("Cooking", false);
        _cookingPotAnimator.enabled = false;
        _boilingFxObj.SetActive(false);
        _fireFXObj.SetActive(false);
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
                hit.collider.gameObject.GetComponent<Dish>().OnCookingPotDropped(IngredientsInPot, CookedLvl, _boilingEmitter);
                IngredientsInPot.Clear();
                IsCooking = false;
                _cookingPotAnimator.SetBool("Cooking", false);
                _boilingFxObj.SetActive(false);
                _boilingEmitter.Stop();
                this.gameObject.layer = 0;
                _canPlayFoodDoneSFX = false;
                GameManager.Instance.DeleteCookingIcons();
            }
            else
            {
                IsCooking = true;
                _cookingPotAnimator.SetBool("Cooking", true);
                _boilingFxObj.SetActive(true);
                _boilingEmitter.Play();
                this.gameObject.layer = 0;
            }
        }
        else
        {
            IsCooking = true;
            _cookingPotAnimator.SetBool("Cooking", true);
            _boilingFxObj.SetActive(true);
            _boilingEmitter.Play();
            this.gameObject.layer = 0;
        }
        transform.localPosition = _originalPos;
        _cookingPotAnimator.enabled = true;
    }

    public void ResetValues()
    {
        IngredientsInPot = new List<string>();
        _cookingTimer = 0;
        IsCooking = false;
        _cookingPotAnimator.enabled = true;
        _cookingPotAnimator.SetBool("Cooking", false);
        CookedLvl = CookedLevel.UnderCooked;

        foreach(var light in StoveLightImages)
        {
            light.sprite = StoveLights[0];
        }

        GameManager.Instance.DeleteCookingIcons();
    }

    private IEnumerator PlaySplashFX()
    {
        yield return new WaitForSeconds(0.5f);
        var anim = _waterSplashFXObj.GetComponent<Animator>();
        anim.SetTrigger("ItemDropped");
    }
}

public enum CookedLevel
{
    UnderCooked,
    Cooked,
    OverCooked
}
