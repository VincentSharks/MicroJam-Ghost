using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer: MonoBehaviour
{
    public string ExplanationText;
    public string CategoryText;
    public Dictionary<string, bool> Likes = new Dictionary<string, bool>(); //name of ingredient + revealed or not
    public Dictionary<string, bool> Dislikes = new Dictionary<string, bool>();  //name of ingredient + revealed or not 
    public int IngredientsCount;

    //public GameObject OrderObjPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
