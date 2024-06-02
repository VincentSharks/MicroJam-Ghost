using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBook : MonoBehaviour
{
    [SerializeField] private GameObject _likeordislikeRowPrefab;
    [SerializeField] private GameObject _likesContainer;
    [SerializeField] private GameObject _dislikesContainer;
    [SerializeField] private Text _header;

    public List<Sprite> Icons;

    private int _currentPage = 0;

    public void NextPage()
    {
        if (_currentPage == GameManager.Instance.CustomerManager.PossibleCustomers.Count-1) _currentPage = 0;
        else _currentPage++;

        SetPageValues();
    }

    public void PreviousPage()
    {
        if (_currentPage == 0) _currentPage = GameManager.Instance.CustomerManager.PossibleCustomers.Count-1;
        else _currentPage--;

        SetPageValues();
    }

    public void SetPageValues() //page == idx of possiblecustomers
    {
        foreach (Transform child in _likesContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _dislikesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        var customers = GameManager.Instance.CustomerManager.PossibleCustomers;
        var chosenCustomer = customers[_currentPage];

        foreach(var tmp in chosenCustomer.Likes)
        {
            //if revealed
            if (tmp.Value)
            {
                var icon = GameManager.Instance.Icons[tmp.Key];

                InfoRow rowInfo = Instantiate(_likeordislikeRowPrefab, _likesContainer.transform).GetComponent<InfoRow>();
                rowInfo.SetValues(icon);
            }
            else
            {
                var icon = GameManager.Instance.Icons["Unknown"];

                InfoRow rowInfo = Instantiate(_likeordislikeRowPrefab, _likesContainer.transform).GetComponent<InfoRow>();
                rowInfo.SetValues(icon);
            }
        }

        foreach (var tmp in chosenCustomer.Dislikes)
        {
            //if revealed
            if (tmp.Value)
            {
                var icon = GameManager.Instance.Icons[tmp.Key];

                InfoRow rowInfo = Instantiate(_likeordislikeRowPrefab, _dislikesContainer.transform).GetComponent<InfoRow>();
                rowInfo.SetValues(icon);
            }
            else
            {
                var icon = GameManager.Instance.Icons["Unknown"];

                InfoRow rowInfo = Instantiate(_likeordislikeRowPrefab, _dislikesContainer.transform).GetComponent<InfoRow>();
                rowInfo.SetValues(icon);
            }
        }

        _header.text = chosenCustomer.Name;
    }
}
