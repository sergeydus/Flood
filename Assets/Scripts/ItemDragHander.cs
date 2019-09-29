using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/* 
public class ItemDragHander : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    // Start is called before the first frame update
    public ConstructionItem item;
    private ConstructionItem newitem=null;
    bool isdragged;
    public LevelManager levelManager;
    void Start()
    {
        
    }
    public void OnBeginDrag(PointerEventData data){
        isdragged=true;
        Debug.Log("Creating new item");
        newitem= Instantiate(item,Camera.main.ScreenToWorldPoint(Input.mousePosition),new Quaternion());
        newitem.name="Item_"+levelManager.items.Count;
        newitem.levelManager=levelManager;
    }
    public void OnDrag(PointerEventData data){
        if(isdragged){
            Debug.Log("Drag started");
            newitem.transform.position = Vector2.Lerp(newitem.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.5f);
        }
    }
    public void OnEndDrag(PointerEventData data){
        if(isdragged){
            Debug.Log("Drag ended");
            if(newitem){
                if(IsMouseOverUi()){
                    Debug.Log("over ui, DESTROYED");
                    Destroy(newitem.gameObject);
                }
                else{
                    newitem.OnMouseUp();
                    // newitem.rb.gravityScale=1;
                }
            }
            isdragged=false;
            // transform.localPosition=Vector3.zero;
        }
    }
    private bool IsMouseOverUi(){
        return EventSystem.current.IsPointerOverGameObject();
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Debug.Log("Disabled");
        // if(isdragged){
        //     isdragged=false;
        //     newitem.OnMouseUp();
        //     newitem=null;
        // }
        OnEndDrag(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
public class ItemDragHander : MonoBehaviour,IDragHandler,IEndDragHandler
{
    // Start is called before the first frame update
    ShopSlot shopSlot;
    public LevelManager levelManager;
    void Start()
    {
        shopSlot=GetComponent<ShopSlot>();
        Debug.Log("ItemDragHandler Onlinee!");
    }

    public void OnDrag(PointerEventData data){
        Debug.Log("dragging");
        shopSlot.Icon.transform.position=Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData data){
        if(!IsMouseOverUi()){
            Vector3 objpos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objpos.z=0;
            ConstructionItem newitem= Instantiate(shopSlot.item,objpos,Quaternion.identity);
            // newitem.levelManager=
            shopSlot.Icon.transform.localPosition=Vector3.zero;
        }
        else{
            shopSlot.Icon.transform.localPosition=Vector3.zero;
        }
    }
    private bool IsMouseOverUi(){
        return EventSystem.current.IsPointerOverGameObject();
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Debug.Log("Disabled");
        // if(isdragged){
        //     isdragged=false;
        //     newitem.OnMouseUp();
        //     newitem=null;
        // }
        OnEndDrag(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}