using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoat : MonoBehaviour
{
    [SerializeField] List<ConstructionItem> items=null;
    // Start is called before the first frame update
    void Start()
    {
        items[0].itemcollider2d.enabled=false;
        items[0].itemcollider2d.enabled=true;
        attach();
        
    }

    // Update is called once per frame
    void Update()
    {
        // attach();   
    }
    private void attach()
    {
        Debug.Log("ENBEMY ITEMS:"+items.Count);
        for(int i=0;i<items.Count;i++){
            var item=items[i];
            for(int j=0;j<items.Count;j++){
                var item2=items[j];
            // }
        // }
        // items.ForEach(item=>{
        //     items.ForEach(item2=>{
                Debug.Log((item.itemcollider2d==item2.itemcollider2d)?("SAME COLLIDER!"):("NOT SAME COLLIDER!"));
                if(item.itemcollider2d.IsTouching(item2.itemcollider2d) && !item2.Equals(item)){
                    Debug.Log("ENEMY: TOUCHING");
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

                        // bodies.Add(item.game Object.GetComponents<FixedJoint2D>()[item.gameObject.GetComponents<FixedJoint2D>().Length-1]);
                        // bodies.Add(item2.gameObject.GetComponents<FixedJoint2D>()[item2.gameObject.GetComponents<FixedJoint2D>().Length-1]);

                    }
                }
                else{
                    Debug.Log("ENEMY: NO ITEMS ARE TOUCHING!");
                }
            };
        };
    }

}
