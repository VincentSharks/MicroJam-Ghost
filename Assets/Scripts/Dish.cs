using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public List<string> Ingredients;
    public CookedLevel CookedLevel;

    public List<Sprite> _dishVariations;

    public void OnCookingPotDropped(List<string> ingredientsInPot, CookedLevel cookedLvl)
    {
        Ingredients = new List<string>();
        Ingredients.AddRange(ingredientsInPot);
        CookedLevel = cookedLvl;

        var randomidx = Random.Range(1, _dishVariations.Count);
        GetComponent<SpriteRenderer>().sprite = _dishVariations[randomidx];
    }

    public void ResetValues()
    {
        Ingredients.Clear();
        CookedLevel = CookedLevel.UnderCooked;
        StartCoroutine(ChangeDishtextureToNormal());
    }

    private IEnumerator ChangeDishtextureToNormal()
    {
        yield return new WaitForSeconds(3);

        GetComponent<SpriteRenderer>().sprite = _dishVariations[0];
    }
}
