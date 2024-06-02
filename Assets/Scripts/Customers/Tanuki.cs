using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanuki : Customer
{
    private void Awake()
    {
        IngredientsCount = 5;

        Likes = new Dictionary<string, bool>();
        Likes.Add("Hand", false);
        Likes.Add("Bone", false);

        Dislikes = new Dictionary<string, bool>();
        Dislikes.Add("Teeth", false);
        Dislikes.Add("BloodVial", false);
    }
}
