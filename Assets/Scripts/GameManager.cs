using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CustomerManager CustomerManager;
    public Customer ActiveCustomer;
    public InfoBook InfoBook;

    public Dish Dish;
    public Cooking Cooking;
    public int TotalSoulsEarned = 0;
    public int SoulsEarnedThisDay = 0;
    public int CurrentCustomerIdx;
    public bool GameStarted;
    public int CurrentDay = 1;

    [SerializeField] private Animation _serveDish;
    [SerializeField] private GameObject MainMenuUiObj;

    [SerializeField] private GameObject EndOfDayUiObj;
    [SerializeField] private Text Day;

    public Dictionary<string, Sprite> Icons = new Dictionary<string, Sprite>();

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        MainMenuUiObj.SetActive(false);
        
        ActiveCustomer = CustomerManager.GenerateOrder();
        GameStarted = true;
        CurrentCustomerIdx = 0;

        Icons.Clear();
        Icons.Add("Unknown", InfoBook.Icons[0]);
        Icons.Add("Eye", InfoBook.Icons[1]);
        Icons.Add("Teeth", InfoBook.Icons[2]);
        Icons.Add("BloodVial", InfoBook.Icons[3]);
        Icons.Add("SoulsBottle", InfoBook.Icons[4]);
        Icons.Add("Bone", InfoBook.Icons[5]);
        Icons.Add("Hand", InfoBook.Icons[6]);

        InfoBook.SetPageValues();
    }

    public void OnBellClicked()
    {
        Debug.Log("bell clicked");

        if (!GameStarted) return;
        if (Dish.Ingredients.Count != ActiveCustomer.IngredientsCount) return;

        var soulsEarnedFromOrder = 0;

        foreach(var ingredient in Dish.Ingredients)
        {
            if (ActiveCustomer.Likes.ContainsKey(ingredient)) soulsEarnedFromOrder += 20;
            else if (ActiveCustomer.Dislikes.ContainsKey(ingredient)) soulsEarnedFromOrder -= 10;
            else soulsEarnedFromOrder += 5;
        }

        if (Dish.CookedLevel == CookedLevel.Cooked) soulsEarnedFromOrder += 10;
        else soulsEarnedFromOrder -= 10;

        if (soulsEarnedFromOrder < 0) soulsEarnedFromOrder = 0;

        SoulsEarnedThisDay += soulsEarnedFromOrder;
        TotalSoulsEarned += soulsEarnedFromOrder;

        foreach (var ingredient in Dish.Ingredients)
        {
            if (ActiveCustomer.Likes.ContainsKey(ingredient)) ActiveCustomer.Likes[ingredient] = true;
            else if (ActiveCustomer.Dislikes.ContainsKey(ingredient)) ActiveCustomer.Dislikes[ingredient] = true;
        }

        InfoBook.SetPageValues();

        //pop up saying u earned this many souls
        //update display current souls count
        _serveDish.Play();
        Cooking.ResetValues();
        Dish.ResetValues();

        ActiveCustomer = CustomerManager.GenerateOrder();

        if (CurrentCustomerIdx == 5)
        {
            StartCoroutine(EndOfDay());
        }
    }

    private IEnumerator EndOfDay()
    {
        yield return new WaitForSeconds(1.5f);

        EndOfDayUiObj.SetActive(true);
        EndOfDayUiObj.GetComponent<EndShiftManager>().SetValues(CurrentDay, SoulsEarnedThisDay, TotalSoulsEarned);

        SoulsEarnedThisDay = 0;
        CurrentCustomerIdx = 0;
        CurrentDay++;
        Day.text = "Day: " + CurrentDay.ToString();
    }
}
