using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform content;
    public ShopSlot shopSlotPrefab; 
    public GameObject dragParent;
    public GameObject origParent;
    LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager=LevelManager.Instance;
        if(inventory){
            Display(inventory);
        }
    }
    public void Display(Inventory inventory){
        this.inventory=inventory;
        Refresh();
    }

    private void Refresh()
    {
        foreach(ConstructionItem i in inventory.items){
            ShopSlot slot = ShopSlot.Instantiate(shopSlotPrefab,content);
            slot.Display(i,dragParent,slot.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
