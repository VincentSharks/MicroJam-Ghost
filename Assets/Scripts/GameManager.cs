using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CustomerManager CustomerManager;
    public Customer ActiveCustomer;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ActiveCustomer = CustomerManager.GenerateOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
