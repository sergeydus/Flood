using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance{get;private set;}
    public GameObject Menu;
    public GameObject attachButton;
    public CameraController playerCamera;
    public Player player;
    public List<ConstructionItem> items;
    public List<ConstructionItem> Connecteditems;
    public int money;   
    public Text MoneyText;
    public GameObject Water;
    public bool gamestarted=false;
    public bool isOver=false;
    public CommandProcessor commandProcessor;
    public event Action startGameActions;
    public GameObject startButton;
    public GameObject ShopButton;
    public List<EnemyBoat> enemyBoats;
    public bool isPaused=false;
    public List<ConstructionItem> enemies;

    // Start is called before the first frame update
    void Awake() {
        //singleton stuff
        if(Instance==null){
            Instance=this;
            commandProcessor=GetComponent<CommandProcessor>();
            items=new List<ConstructionItem>();
            Connecteditems=new List<ConstructionItem>();
            Physics2D.IgnoreLayerCollision(9,10,true);
            MoneyText.text="Money:"+money+'$';
            Menu.SetActive(false);
            attachButton.SetActive(false);   
        }
        else{
            Destroy(gameObject);
        }

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gamestarted){
            Water.transform.position =Vector2.MoveTowards(Water.transform.position,
                                                            Vector2.zero,
                                                            Vector2.Distance(Water.transform.position,Vector2.zero)*Time.deltaTime/10
                                                        );
        }
        else{            
            CheckAttachables();
            if(Input.GetKeyDown(KeyCode.Tab)){
                Menu.SetActive(true);
                Debug.Log("Tab pressed");
            }
            if(Input.GetKeyUp(KeyCode.Tab)){
                Menu.SetActive(false);  
                Debug.Log("Tab Released");
            }
            //we find our dragged item, then
            //we find every
            if(Input.GetKeyUp(KeyCode.X)){
                attach();
            }
            if (Input.GetKeyDown(KeyCode.Z) && !Input.GetMouseButton(0)) {
                Debug.Log("Undo!!");
                commandProcessor.Undo();
            }
        }
    }
    public void attach(){
        var attachCommand = new AttachItemsCommand(items);
        commandProcessor.ExecuteCommand(attachCommand);
        /*
        // ConstructionItem dragged=items.Find(x=>x.gameObject.layer==10);
        //     if(dragged!=null){
        //         int counter=0;// counts collisions
        //         // Connecteditems.Add(dragged);
        //         items.ForEach(delegate(ConstructionItem item){
        //             if(dragged.itemcollider2d.bounds.Intersects(item.itemcollider2d.bounds) && !item.Equals(dragged)){
        //                 counter++;
        //                 // if(item.transform.parent!=null)
        //                 //     item.transform.parent.parent=dragged.transform;
        //                 // item.transform.SetParent(dragged.transform);
        //                 dragged.gameObject.AddComponent<FixedJoint2D>();
        //                 item.gameObject.AddComponent<FixedJoint2D>();
        //                 var jointcompoinent=dragged.gameObject.GetComponents<FixedJoint2D>()[dragged.gameObject.GetComponents<FixedJoint2D>().Length-1];
        //                 jointcompoinent.enableCollision=false;
        //                 jointcompoinent.connectedBody=item.rb;
        //                 Connecteditems.Add(item);
        //                 var jointcompoinent2=item.gameObject.GetComponents<FixedJoint2D>()[item.gameObject.GetComponents<FixedJoint2D>().Length-1];
        //                 jointcompoinent2.connectedBody=dragged.rb;
        //                 jointcompoinent.enableCollision=false;
        //             }
        //         });
        //         if(counter>0){
        //             Connecteditems.Add(dragged);
        //         }
        //     }
        for(int i=0;i<items.Count;i++){
            var item=items[i];
            for(int j=0;j<items.Count;j++){
                var item2=items[j];
            // }
        // }
        // items.ForEach(item=>{
        //     items.ForEach(item2=>{
                if(item.itemcollider2d.IsTouching(item2.itemcollider2d) && !item2.Equals(item)){
                    var joints = item.GetComponents<FixedJoint2D>();
                    bool hasconnection=false;
                    if(joints!=null){
                        for(int k=0;k<joints.Length;k++){
                            if(joints[k].connectedBody==(item2.rb)){
                                Debug.Log("CONNECTED JOINT!!!!");
                                hasconnection=true;
                            }
                        }
                    }
                    if(!hasconnection){
                        Debug.Log("not connected joint!, i have "+((joints!=null)?(joints.Length+" Joints"):("0 joints")));
                        item.gameObject.AddComponent<FixedJoint2D>();
                        item2.gameObject.AddComponent<FixedJoint2D>();
                        var jointcompoinent=item.gameObject.GetComponents<FixedJoint2D>()[item.gameObject.GetComponents<FixedJoint2D>().Length-1];
                        jointcompoinent.enableCollision=false;
                        jointcompoinent.connectedBody=item2.rb;
                        // Connecteditems.Add(item);
                        var jointcompoinent2=item2.gameObject.GetComponents<FixedJoint2D>()[item2.gameObject.GetComponents<FixedJoint2D>().Length-1];
                        jointcompoinent2.connectedBody=item.rb;
                        jointcompoinent.enableCollision=false;
                    }
                }
            };
        };
        Debug.Log("Number of connected to item 0:"+GetConnectedToItem(items[0]).Count.ToString());
         */
    }

    internal void SellItem(ConstructionItem item)
    {
        money+=item.Price;
        // items.Remove(item);
            Destroy(item.gameObject);
        Debug.Log("ITEM SOLD");
        RefreshMoney();
    }

    internal void BuyItem(ConstructionItem item,Vector2 spawnPosition)
    {
        Debug.Log("Bought item:"+item.name);
        if(item.Price<=money){
            money-=item.Price;
            // ConstructionItem newitem= Instantiate(item,spawnPosition,Quaternion.identity);
            // item.levelManager=this;
            // items.Add(item);
        }
        RefreshMoney();
    }
    void RefreshMoney(){
        MoneyText.text="Money:"+money.ToString()+'$';
    }

    public void CheckAttachables(){
        bool toShowAttachButton=false;
        items.ForEach(item=>{
            items.ForEach(item2=>{
                if(item.itemcollider2d.IsTouching(item2.itemcollider2d) && !item2.Equals(item)){
                    var joints=item.GetComponents<FixedJoint2D>();
                    if(joints!=null){
                        bool isconnected=false;
                        for(int i=0;i<joints.Length;i++){
                            if(joints[i].connectedBody==(item2.rb))
                                isconnected=true;
                        }
                        if(!isconnected){
                            Debug.Log("Found collision between "+item.name+" and "+item2.name);
                            toShowAttachButton=true;
                        }
                    }
                    else{
                        Debug.Log("No joints, showing button");
                        toShowAttachButton=true;
                    }   
                }
            });
        });
        Debug.Log((toShowAttachButton)?("ATTACH BUTTON ACTIVATE"):("NO ATTACH"));
        attachButton.SetActive(toShowAttachButton);
    }
    public void CloseShop(){
        Debug.Log("Closing shop");
        Menu.SetActive(false);   
    }
    public void asd(int price){
        MoneyText.text="Money:"+(--money)+'$';
        Debug.Log("ASD!");
    }
    //this function makes so the item is the parent of all of its connected items.
    //connected items= connected through joints
    public List<ConstructionItem> GetConnectedToItem(ConstructionItem item){
        List<ConstructionItem> visited = new List<ConstructionItem>();
        visited.Add(item);
        return GetConnectedToItemRecursion(visited);
    }
    public List<ConstructionItem> GetConnectedToItemRecursion(List<ConstructionItem> visited){
        for(int i=0;i<visited.Count;i++){
            var joints=visited[i].GetComponents<FixedJoint2D>();
            if(joints!=null){
                for(int j=0;j<joints.Length;j++){
                    var curitem=items.Find(x=>x.rb==joints[j].connectedBody);
                    if(!visited.Contains(curitem)){
                        visited.Add(curitem);
                        visited=GetConnectedToItemRecursion(visited);
                    }
                    
                }
            }
        }
        return visited;
    }
    public void StartGame(){
        player= FindObjectOfType<Player>();
        if(!player){
            Debug.Log("No player!");
            return;
        }
        // player.gameObject.SetActive(false);
        gamestarted=true;
        if(startGameActions!=null){
            startGameActions();
        }
        Menu.SetActive(false);
        ShopButton.SetActive(false);
        // List<ConstructionItem> PlayerItems = items.FindAll(x=>x.owner==Owner.Player);
        // if(PlayerItems.Count>0){
        //     Vector3 averagePositions= new Vector3(0,0,0);
        //     float highestY = PlayerItems.Max(t=>t.transform.position.y);
        //     ConstructionItem highestItem=PlayerItems.Find(item=>item.transform.position.y==highestY);

        //     PlayerItems.ForEach(curItem=>averagePositions+=curItem.transform.position);
        //     averagePositions/= PlayerItems.Count;
        //     averagePositions.y=highestItem.itemcollider2d.bounds.size.y+highestY;
        //     player.transform.position=averagePositions;
        // }
        player.healthBar.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        player.isactive=true;
        playerCamera.isactive=true;
        items.ForEach(x=>x.rb.constraints = RigidbodyConstraints2D.None);
        gamestarted=true;
        startButton.gameObject.SetActive(false);
        /*
        playerPosition = GetCharacterItemsAveragePosition(Owner.Player);
        var enemies = items.FindAll(item=>item.name=="Enemy");
        for(int i=0;i<enemies.Count;i++){
            aiPositions.Add(new Vector2(enemies[i].gameObject.transform.position.x,enemies[i].gameObject.transform.position.y));
        }
         */
        //get average player item position;
        
        // Water.transform.position =Vector2.MoveTowards(Water.transform.position,Vector2.zero,0.5f);
    }
    public void Restart(){
        SceneManager.LoadScene("lvl1 - copy"); //Load scene called Game
    }

}
