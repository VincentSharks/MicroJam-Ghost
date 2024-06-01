using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Customer> PossibleCustomers = new List<Customer>();
    private Customer _activeCustomer;

    public OrderSlot OrderSlot;

    public Customer GenerateOrder()
    {
        var randomIdx = Random.Range(0, PossibleCustomers.Count);
        var nextCustomer = PossibleCustomers[randomIdx];

        _activeCustomer = nextCustomer;
        OrderSlot.GenerateNewOrderVisual(nextCustomer);

        return _activeCustomer;
    }

    void Start()
    {
        //_reaper.Likes["Bone"] = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
