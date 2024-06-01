using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public List<string> Ingredients = new List<string>();
    public CookedLevel CookedLevel;

    public void OnCookingPotDropped(List<string> ingredientsInPot, CookedLevel cookedLvl)
    {
        Ingredients = ingredientsInPot;
        CookedLevel = cookedLvl;
        //update texture visual
    }

    public void ResetValues()
    {
        Ingredients.Clear();
        CookedLevel = CookedLevel.UnderCooked;
    }
}
