using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float offset;
    public float offsetlerp;
    private Vector3 playerposition;
    public bool isactive;
    LevelManager levelManager;
    Vector2 averagePosition;
    List<ConstructionItem> enemies;
    List<Vector2> aiPositions;
    public float zoomLevel;
    // Start is called before the first frame update
    void Start()
    {
        levelManager=LevelManager.Instance;
        isactive=false;
        averagePosition=Vector2.zero;
        enemies= new List<ConstructionItem>();
        aiPositions=new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isactive){
            /*
            playerposition= new Vector3(player.transform.position.x,player.transform.position.y,transform.position.z);
            if(player.transform.localScale.x>0f){
                playerposition= new Vector3(player.transform.position.x + offset,player.transform.position.y,transform.position.z);
            }
            else{
                playerposition= new Vector3(player.transform.position.x - offset,player.transform.position.y,transform.position.z);
            }
            transform.position=Vector3.Lerp(transform.position,playerposition,offsetlerp * Time.deltaTime);
             */
            averagePosition=Vector2.zero;
            enemies = levelManager.items.FindAll(item=>item.name=="Enemy");
            aiPositions.Clear();
            for(int i=0;i<enemies.Count;i++){
                // Debug.Log("inside for");
                aiPositions.Add(new Vector2(enemies[i].gameObject.transform.position.x,enemies[i].gameObject.transform.position.y));
                // Debug.Log("enemy position["+i+"]:"+enemies[i].gameObject.transform.position.ToString());
            }
            // Debug.Log("Enemies:"+enemies.Count);
            aiPositions.ForEach(aiPosition=>averagePosition+=aiPosition);
            averagePosition+=new Vector2(player.transform.position.x,player.transform.position.y);
            // Debug.Log("Player position:"+player.transform.position);
            averagePosition=averagePosition/(aiPositions.Count+1);
            // Debug.Log("average position:"+averagePosition);
            transform.position=Vector3.Lerp(transform.position,new Vector3(averagePosition.x,averagePosition.y,transform.position.z),0.6f * Time.deltaTime);
            //now we calculate how much to zoom out, we find max distance between every two items.
            float maxDistance=0;
            aiPositions.ForEach(item1=>{
                aiPositions.ForEach(item2=>{
                    float vdistance=Vector2.Distance(item1,item2);
                    if(item1!=item2 && vdistance>maxDistance){
                        maxDistance=vdistance;
                    }
                });
                float distance= Vector2.Distance(item1,new Vector2(player.transform.position.x,player.transform.position.y));
                if(distance>maxDistance)
                    maxDistance=distance;
            });
            if(maxDistance/zoomLevel<6){
                Camera.main.orthographicSize=6;
            }
            else Camera.main.orthographicSize=maxDistance/zoomLevel;

        }
        
    }
    Vector2 GetCharacterItemsAveragePosition(Owner owner){
        Vector2 average = new Vector2(0,0);
        levelManager.items.ForEach(item=>{
            if(item.owner==owner){
                average+=new Vector2(item.transform.position.x,item.transform.position.y);
            }
        });
        average/=levelManager.items.Count;
        return average;
    }
}
