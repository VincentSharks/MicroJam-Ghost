using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tag : MonoBehaviour
{
    [SerializeField] private Image _photo;
    [SerializeField] private Text _description;
    [SerializeField] private GameObject _tagObj;
    public void SetTagInfo()
    {
        if (!GameManager.Instance.GameStarted) return;

        _tagObj.SetActive(true);
        _photo.sprite = GameManager.Instance.ActiveCustomer.Photo;
        _description.text = GameManager.Instance.ActiveCustomer.ExplanationText;
    }
}
