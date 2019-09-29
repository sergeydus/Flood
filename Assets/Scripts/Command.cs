using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICommand {
    public abstract void Execute();
    public abstract void Undo();
}
public class RotateItemsLeftCommand : ICommand
{
    List<ConstructionItem> neighbors;
    ConstructionItem item;
    public RotateItemsLeftCommand(List<ConstructionItem> neighbors,ConstructionItem item)
    {
        this.item=item;
        this.neighbors=neighbors;
    }


    public override void Execute()
    {
        neighbors.ForEach(x=>x.transform.SetParent(null));
        neighbors.ForEach(x=>x.transform.SetParent(item.transform));
        item.transform.Rotate(Vector3.forward * 6f);
    }

    public override void Undo()
    {
        neighbors.ForEach(x=>x.transform.SetParent(null));
        neighbors.ForEach(x=>x.transform.SetParent(item.transform));
        item.transform.Rotate(Vector3.back * 6f);
        neighbors.ForEach(x=>x.transform.SetParent(null));
    }
}
public class RotateItemsRightCommand : ICommand
{
    List<ConstructionItem> neighbors;
    ConstructionItem item;
    public RotateItemsRightCommand(List<ConstructionItem> neighbors,ConstructionItem item)
    {
        this.item=item;
        this.neighbors=neighbors;
    }


    public override void Execute()
    {
        Debug.Log("Command rotatu rightu");
        neighbors.ForEach(x=>x.transform.SetParent(null));
        neighbors.ForEach(x=>x.transform.SetParent(item.transform));
        item.transform.Rotate(Vector3.back * 6f);
    }

    public override void Undo()
    {
        neighbors.ForEach(x=>x.transform.SetParent(null));
        neighbors.ForEach(x=>x.transform.SetParent(item.transform));
        item.transform.Rotate(Vector3.forward * 6f);
        neighbors.ForEach(x=>x.transform.SetParent(null));
    }
}
public class MoveItemsCommand : ICommand
{
    List<ConstructionItem> neighbors;
    ConstructionItem item;
    Vector2 newPosition;
    Vector2 oldPosition;
    public MoveItemsCommand(List<ConstructionItem> neighbors,ConstructionItem item,Vector2 Position)
    {
        this.item=item;
        this.neighbors=neighbors;
        newPosition=Position;
        oldPosition=item.transform.position;
    }
    public override void Execute()
    {
        neighbors.ForEach(x=>x.transform.SetParent(null));
        neighbors.ForEach(x=>x.transform.SetParent(item.transform));
        item.transform.Rotate(Vector3.back * 6f);
    }

    public override void Undo()
    {
        neighbors.ForEach(x=>x.transform.SetParent(null));
        neighbors.ForEach(x=>x.transform.SetParent(item.transform));
        item.transform.Rotate(Vector3.right * 6f);
        
    }
}
public class BuyItemCommand : ICommand
{
    ConstructionItem item;
    LevelManager levelManager;
    Vector2 spawnPosition;
    public BuyItemCommand(ConstructionItem item,LevelManager levelManager,Vector2 spawnPosition)
    {
        this.item=GameObject.Instantiate(item,spawnPosition,Quaternion.identity);
        this.levelManager=levelManager;
        this.spawnPosition=spawnPosition;
        item.owner=Owner.Player;
    }
    public override void Execute()
    {
        levelManager.BuyItem(item,spawnPosition);
    }

    public override void Undo()
    {
        levelManager.SellItem(item);
    }
}
public class EndMovementCommand : ICommand
{
    List<ConstructionItem> neighbors;
    ConstructionItem item;
    public EndMovementCommand(List<ConstructionItem> neighbors,ConstructionItem item)
    {
        this.item=item;
        this.neighbors=neighbors;
    }


    public override void Execute()
    {
        neighbors.ForEach(x=>x.transform.SetParent(null));
    }

    public override void Undo()
    {
        neighbors.ForEach(x=>x.transform.SetParent(item.transform));

    }
}
public class AttachItemsCommand : ICommand
{
    List<ConstructionItem> items;
    List<FixedJoint2D> bodies;
    public AttachItemsCommand(List<ConstructionItem> items)
    {
        this.items=items;
        bodies=new List<FixedJoint2D>();
    }


    public override void Execute()
    {

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

                        bodies.Add(item.gameObject.GetComponents<FixedJoint2D>()[item.gameObject.GetComponents<FixedJoint2D>().Length-1]);
                        bodies.Add(item2.gameObject.GetComponents<FixedJoint2D>()[item2.gameObject.GetComponents<FixedJoint2D>().Length-1]);

                    }
                }
            };
        };
    }

    public override void Undo()
    {
        bodies.ForEach(x=>GameObject.Destroy(x));
        items[0].itemcollider2d.enabled=false;
        items[0].itemcollider2d.enabled=true;
    }
}