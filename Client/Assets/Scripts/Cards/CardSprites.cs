using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardSprites
{
    static CardSprites()
    {
        foreach(Sprite s in Resources.LoadAll <Sprite> ("Cards"))
        {
            sprites.Add(s.name,s);
        }
    }
    public static Dictionary<string, Sprite> sprites=new Dictionary<string, Sprite>();
}
