using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSlot : MonoBehaviour
{
    public Text TagText;
    public SpriteRenderer VisualRenderer;
    public Tag Tag;

    public List<Sprite> OrderVisuals;

    public void GenerateNewOrderVisual(Customer customer)
    {
        var randomIdx = Random.Range(0, OrderVisuals.Count);

        VisualRenderer.sprite = OrderVisuals[randomIdx];
        VisualRenderer.enabled = true;
    }
}
