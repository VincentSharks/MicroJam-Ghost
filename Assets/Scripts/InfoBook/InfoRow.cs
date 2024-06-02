using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    public void SetValues(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
