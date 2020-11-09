using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject card;
    private List<GameObject> cards=new List<GameObject>();
    public List<string> cardString;

    void Start()
    {
        for(int i=0;i<13;i++)
        {
            GameObject temp = Instantiate(card,new Vector3(0,0,0),Quaternion.AngleAxis(-90,Vector3.forward));
            temp.transform.SetParent(this.gameObject.transform);
            temp.gameObject.GetComponent<Image>().sprite = CardSprites.sprites["back"];
            cards.Add(temp);
        }
    }
}
