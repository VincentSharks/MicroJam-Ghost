using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitsune : Customer
{
    private void Awake()
    {
        Name = "Kitsune";
        IngredientsCount = 5;

        Likes = new Dictionary<string, bool>();
        Likes.Add("Bone", false);
        Likes.Add("Blood", false);

        Dislikes = new Dictionary<string, bool>();
        Dislikes.Add("Eye", false);
    }
}
