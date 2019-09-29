using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ConstructionItem
{
    // Start is called before the first frame update
    [SerializeField] GameObject bodyjoint;
    [SerializeField] public GameObject cannon;
    [SerializeField] GameObject mouth=null;
    bool mouseHeld=false;
    Vector3 mouseStartPosition;
    Vector3 mouseEndPosition;
    LineRenderer lineRenderer;
    [SerializeField] List<Projectile> projectiles=null;
    public DragIndicatorScript dragIndicatorScript{
         get;set;
     }
    public int shootingStrength;
    int selectedProjectileType=0;//0-2, 0 - cannon, 1- bomb ,2- dunno
    public int enemyShootingStrength;
    void Start()
    {
        levelManager=LevelManager.Instance;
        levelManager.items.Add(this);
        startcolor=GetComponent<SpriteRenderer>().color;
        rb.constraints = RigidbodyConstraints2D.FreezePosition|RigidbodyConstraints2D.FreezeRotation;
        dragIndicatorScript=DragIndicatorScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("Unfreeze!");
            rb.constraints = RigidbodyConstraints2D.None;
        }
        if(hp==0){
            Destroy(this.gameObject);
        }
        if(levelManager.gamestarted && !levelManager.isOver && !levelManager.isPaused){
            if(owner==Owner.Player){
                if(Input.GetMouseButton(0)){
                    // var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var pos = (Input.mousePosition);
                    if(mouseHeld==false){
                        mouseStartPosition=pos;
                        mouseHeld=true;
                    }
                    else{
                        var dir = Input.mousePosition - mouseStartPosition;//direction
                        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        if(Input.mousePosition.x>mouseStartPosition.x){
                            cannon.transform.localScale=new Vector3(1,1,1);
                            Debug.Log("LOOK LEFT");
                            cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
                            Debug.Log("Follow mouse");
                            // cannon
                        }
                        else{
                            cannon.transform.localScale=new Vector3(-1,1,1);
                            Debug.Log("LOOK R IGHT");
                            cannon.transform.rotation = Quaternion.AngleAxis(angle+180, Vector3.forward); 
                            Debug.Log("Follow mouse");

                            // cannon.transform.localPosition=new Vector3(1,1,1);
                        }
                    }
                }
                else{
                    if(mouseHeld){
                        Projectile proj=Instantiate(projectiles[selectedProjectileType],mouth.transform.position,cannon.transform.rotation);
                        proj.GetComponent<Rigidbody2D>().AddForce(dragIndicatorScript.dragStrength*shootingStrength);
                        Debug.Log("Shoot!!!");
                    }
                    mouseHeld=false;
                }
            }
            else{//how ai aims

            }
        }
    }
    //
}
