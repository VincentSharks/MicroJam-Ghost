using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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
    public int CurrentCustomerIdx = -1;
    public bool GameStarted;
    public int CurrentDay = 1;

    [SerializeField] private Animation _serveDish;
    [SerializeField] private GameObject MainMenuUiObj;

    [SerializeField] private GameObject EndOfDayUiObj;
    [SerializeField] private Text Day;

    [SerializeField] private Transform _ingredientsInPotIconsContainer;
    [SerializeField] private GameObject _IngredientIconPrefab;

    [SerializeField] private Animation _soulsEarnedPopupAnim;
    [SerializeField] private Text _soulsEarnedPopUpText;
    [SerializeField] private Text _soulsEarnedTodaytext;

    [SerializeField] private Animation _orderReminderPopUp;

    public Dictionary<string, Sprite> Icons = new Dictionary<string, Sprite>();

    [SerializeField] private StudioEventEmitter _quitEmitter;
    [SerializeField] private StudioEventEmitter _radioEmitter;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        Icons.Clear();
        Icons.Add("Unknown", InfoBook.Icons[0]);
        Icons.Add("Eye", InfoBook.Icons[1]);
        Icons.Add("Teeth", InfoBook.Icons[2]);
        Icons.Add("BloodVial", InfoBook.Icons[3]);
        Icons.Add("SoulsBottle", InfoBook.Icons[4]);
        Icons.Add("Bone", InfoBook.Icons[5]);
        Icons.Add("Hand", InfoBook.Icons[6]);
        Icons.Add("UnknownWhite", InfoBook.Icons[7]);
    }

    public void StartGame()
    {
        MainMenuUiObj.SetActive(false);
        
        ActiveCustomer = CustomerManager.GenerateOrder();
        CreateCookingIcons();
        GameStarted = true;
        CurrentCustomerIdx = 0;
        _soulsEarnedTodaytext.text = SoulsEarnedThisDay.ToString();

        InfoBook.SetPageValues();
    }

    private IEnumerator DelayQuitGame()
    {
        _quitEmitter.Play();

        yield return new WaitForSeconds(6f);

        Application.OpenURL("https://itch.io/jam/micro-jam-015/rate/2745568");
        _radioEmitter.Stop();
    }

    public void QuitGame()
    {
        StartCoroutine(DelayQuitGame());
    }

    public void CreateCookingIcons()
    {
        for (int i = 0; i < ActiveCustomer.IngredientsCount; i++)
        {
            Instantiate(_IngredientIconPrefab, _ingredientsInPotIconsContainer).GetComponent<Image>().sprite = Icons["UnknownWhite"]; 
        }
    }

    public void DeleteCookingIcons()
    {
        foreach (Transform trans in _ingredientsInPotIconsContainer)
        {
            Destroy(trans.gameObject);
        }
    }

    public void UpdateCookingIcon(string ingredient)
    {
        var iconToAdd = Icons[ingredient];
        _ingredientsInPotIconsContainer.GetChild(Cooking.IngredientsInPot.Count-1).GetComponent<Image>().sprite = iconToAdd;
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

        StartCoroutine(UpdateSoulsEarnedVisual(soulsEarnedFromOrder.ToString()));
        InfoBook.SetPageValues();
        _serveDish.Play();
        Cooking.ResetValues();
        Dish.ResetValues();

        ActiveCustomer = CustomerManager.GenerateOrder();
        CreateCookingIcons();

        if (CurrentCustomerIdx == 5)
        {
            StartCoroutine(EndOfDay());
        }
    }

    public IEnumerator ReminderAfter5Seconds()
    {
        yield return new WaitForSeconds(5);

        if (Dish.Ingredients.Count != 0) _orderReminderPopUp.Play();
    }

    private IEnumerator UpdateSoulsEarnedVisual(string soulsearned)
    {
        _soulsEarnedPopUpText.text = "+" + soulsearned;
        _soulsEarnedPopupAnim.Play();

        yield return new WaitForSeconds(_soulsEarnedPopupAnim.clip.length);

        _soulsEarnedTodaytext.text = SoulsEarnedThisDay.ToString();
    }

    private IEnumerator EndOfDay()
    {
        yield return new WaitForSeconds(1.5f);

        EndOfDayUiObj.SetActive(true);
        EndOfDayUiObj.GetComponent<EndShiftManager>().SetValues(CurrentDay, SoulsEarnedThisDay, TotalSoulsEarned);

        SoulsEarnedThisDay = 0;
        CurrentCustomerIdx = 0;
        CurrentDay++;

        CustomerManager.CostumersServedText.text = "customers served: " + (GameManager.Instance.CurrentCustomerIdx).ToString() + "/5";
        Day.text = "Day: " + CurrentDay.ToString();
    }
}
