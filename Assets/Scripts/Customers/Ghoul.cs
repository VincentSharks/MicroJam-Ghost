using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : Customer
{
    private void Awake()
    {
        Name = "Ghoul";
        IngredientsCount = 4;

        Likes = new Dictionary<string, bool>();
        Likes.Add("Hand", false);
        Likes.Add("Blood", false);
        Likes.Add("Eye", false);

        Dislikes = new Dictionary<string, bool>();
        Dislikes.Add("SoulsBottle", false);
        Dislikes.Add("Teeth", false);
        Dislikes.Add("Bone", false);
    }
}
