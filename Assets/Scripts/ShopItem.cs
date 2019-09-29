using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // public GameObject item;
    public Item item;
    public Text ItemName;
    public Text ItemDescription;
    public Image Icon;
     
    // Start is called before the first frame update
    void Start()
    {
        this.Icon.sprite=item.sprite;
        this.Icon.material=item.texture;
        this.ItemName.text=item.name;
    }

    public void BuyItem(){
        item.Print();
    }
}
