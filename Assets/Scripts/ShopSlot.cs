using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public ConstructionItem item;
    public Text ItemName;
    public Text ItemDescription;
    public Text ItemPrice;
    public Image Icon;
    public LevelManager levelManager;
    public GameObject dragParent;
    public GameObject originalParent;
    private bool isdragged;
    public short stock;
    // Start is called before the first frame update
    void Start()
    {
        levelManager=LevelManager.Instance;
        if(item && dragParent && originalParent)
            Display(item,dragParent,originalParent);
        // ItemName.text=item.ItemName;
        // ItemDescription.text=item.ItemDescription;
        // ItemPrice.text=item.Price.ToString();
        // Icon.sprite=item.sprite;
    }
    public void Display(ConstructionItem item,GameObject dragParent,GameObject originalParent){
        this.item=item;
        Icon.sprite=item.sprite;
        Vector2 size = new Vector2(item.sprite.bounds.size.x,item.sprite.bounds.size.y);
        Icon.rectTransform.sizeDelta=size*100;
        ItemName.text=item.name;
        this.dragParent=dragParent;
        this.originalParent=originalParent;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMouseOverUi()){
            Debug.Log("OVER UI");
        }
    }
    public void Click(){
        if(!isdragged){
            Debug.Log("name:"+item.ItemName+" Clicked!");
            // ConstructionItem newitem= Instantiate(item,new Vector3(0,0,0),new Quaternion());
            // newitem.name="Item_"+levelManager.items.Count;
            // newitem.levelManager=levelManager;
            Vector3 objpos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objpos.z=0;
            if(LevelManager.Instance.money>=item.Price){
                Debug.Log("Money:"+LevelManager.Instance.money.ToString()+", and the item cost:"+item.Price.ToString());
                var BuyCommand = new BuyItemCommand(item,levelManager,objpos);
                levelManager.commandProcessor.ExecuteCommand(BuyCommand);
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isdragged=true;
        Icon.color =  new Color32(255,255,225,100);
        Icon.transform.SetParent(dragParent.transform, true);
        Debug.Log("Begin drag");
    }

    public void OnDrag(PointerEventData data){
        Debug.Log("dragging");
        Icon.transform.position=Input.mousePosition;
        
    }
    public void OnEndDrag(PointerEventData data){
        Debug.Log("UI ELEMENTS:"+data.hovered.Count);
        Icon.transform.SetParent(originalParent.transform, true);
        Icon.transform.localPosition=Vector3.zero;
        isdragged=false;
        Debug.Log("EndDrag, Image at:"+Icon.transform.position+" Mouse at:"+Input.mousePosition);
        if(!IsMouseOverUi()){
            Debug.Log("CREATING ITME!!!");
            Vector3 objpos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objpos.z=0;
            if(LevelManager.Instance.money>=item.Price){
                var BuyCommand = new BuyItemCommand(item,levelManager,objpos);
                levelManager.commandProcessor.ExecuteCommand(BuyCommand);
            }
        }
        else{
            Debug.Log("Mouse is over ui");// THIS HAPPENS
        }
    }
    private bool IsMouseOverUi(){
        return EventSystem.current.IsPointerOverGameObject();
    }
    void OnDisable()
    {
            Debug.Log("Disabled");

    }

}
