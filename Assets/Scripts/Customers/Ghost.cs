using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Customer
{
    private void Awake()
    {
        Name = "Ghost";
        IngredientsCount = 3;

        Likes = new Dictionary<string, bool>();
        Likes.Add("Teeth", false);

        Dislikes = new Dictionary<string, bool>();
        Dislikes.Add("SoulsBottle", false);
    }
}
