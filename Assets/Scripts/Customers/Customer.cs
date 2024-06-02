using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer: MonoBehaviour
{
    public string Name;
    public string ExplanationText;
    public Dictionary<string, bool> Likes = new Dictionary<string, bool>(); //name of ingredient + revealed or not
    public Dictionary<string, bool> Dislikes = new Dictionary<string, bool>();  //name of ingredient + revealed or not 
    public int IngredientsCount;
    public Sprite Photo;
}
