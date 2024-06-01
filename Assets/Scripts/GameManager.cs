using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CustomerManager CustomerManager;
    public Customer ActiveCustomer;

    public Dish Dish;
    public Cooking Cooking;
    public int SoulsEarned;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ActiveCustomer = CustomerManager.GenerateOrder();
    }

    public void OnBellClicked()
    {
        Debug.Log("bell clicked");

        foreach(var ingredient in Dish.Ingredients)
        {
            if (ActiveCustomer.Likes.ContainsKey(ingredient)) SoulsEarned += 20;
            else if (ActiveCustomer.Dislikes.ContainsKey(ingredient)) SoulsEarned -= 10;
            else SoulsEarned += 5;
        }

        if (Dish.CookedLevel == CookedLevel.Cooked) SoulsEarned += 10;
        else SoulsEarned -= 10;

        if (SoulsEarned < 0) SoulsEarned = 0;

        //pop up saying u earned this many souls
        //update display current souls count

        Cooking.ResetValues();
        Dish.ResetValues();
        ActiveCustomer = CustomerManager.GenerateOrder();
        
    }
}
