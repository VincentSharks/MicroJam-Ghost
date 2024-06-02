using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reaper : Customer
{
    private void Awake()
    {
        Name = "Reaper";
        IngredientsCount = 4;

        Likes = new Dictionary<string, bool>();
        Likes.Add("SoulsBottle", false);
        Likes.Add("Bone", false);

        Dislikes = new Dictionary<string, bool>();
        Dislikes.Add("Eye", false);
        Dislikes.Add("Hand", false);
    }
}
