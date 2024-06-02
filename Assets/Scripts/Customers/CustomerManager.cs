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
        GameManager.Instance.CurrentCustomerIdx++;

        return _activeCustomer;
    }
}
