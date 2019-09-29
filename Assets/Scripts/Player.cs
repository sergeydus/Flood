using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : ConstructionItem
{
    public float speed;
    public float jumpspeed;
    // public Rigidbody2D rb;
    public Vector2 moveVelocity;
    Collider2D isgrounded;
    public Transform groundcheckpoint;
    public float groundcheckradius;
    public LayerMask groundlayer;
    public bool isactive;
    public Slider healthBar;
    public Text healthBarText;

    // Start is called before the first frame update
// void Start()
//     {
//         var hpObject = GameObject.Find("Healthbar");
//         healthBar = hpObject.GetComponent<Slider>();
//         healthBarText=hpObject.GetComponentInChildren<Text>();
//         hpObject.SetActive(false);
//         Debug.Log("START:"+name);
//         levelManager=LevelManager.Instance;
//         levelManager.items.Add(this);
//         startcolor=GetComponent<SpriteRenderer>().color;
//         rb.constraints = RigidbodyConstraints2D.FreezePosition|RigidbodyConstraints2D.FreezeRotation;
//     }
    // Update is called once per frame
    void Update()
    {
        // isgrounded=Physics2D.OverlapCircle(groundcheckpoint.position,groundcheckradius,groundlayer);
        // Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        Debug.Log("MOVEINPUT:"+moveInput.ToString());
        // moveVelocity=moveInput.normalized * speed;
        moveVelocity=moveInput* speed;
        // if(isgrounded){
        //     var floor=isgrounded.GetComponent<ConstructionItem>();
        //     if(floor){
        //         if(floor.owner==Owner.AI)
        //             reduceHP(10*Time.deltaTime);
        //     }

        // }
        Debug.Log(moveVelocity);
    }
    void FixedUpdate()
    {
        if(isactive && hp>0){
            if(moveVelocity.x!=0){
                Debug.Log(moveVelocity);
                var connecteditems = LevelManager.Instance.GetConnectedToItem(this);
                connecteditems.ForEach(item => {if(item!=this)
                                                item.GetComponent<Rigidbody2D>().AddForce(moveVelocity/connecteditems.Count);
                                                });
                // rb.velocity= new Vector2(moveVelocity.x*speed,rb.velocity.y);
                if(moveVelocity.x>0)
                    transform.localScale=new Vector2(Mathf.Abs(transform.localScale.x),transform.localScale.y);
                else{
                    transform.localScale=new Vector2(-Mathf.Abs(transform.localScale.x),transform.localScale.y);
                }

            }
            // else{
            //     rb.velocity=new Vector2(0,rb.velocity.y);
            // }
            // if(Input.GetButtonDown("Jump")&&isgrounded){
            //     rb.velocity=new Vector2(rb.velocity.x,jumpspeed);
            //     var floor =isgrounded.GetComponent<Rigidbody2D>();
            //     if(floor !=null){
            //         floor.AddForce(Vector2.down*300);
            //     }
            // }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log("player collision");
        if(other.gameObject.tag=="Water"){
            reduceHP(10*Time.deltaTime);
        }
    }
    private void reduceHP(float hp){
        if(hp>0){
            this.hp-=hp;
            healthBarText.text=Mathf.RoundToInt(this.hp).ToString();
            healthBar.value=Mathf.RoundToInt(this.hp);
            // Debug.Log("hp.value:"+healthBar.value);
            if(healthBar.value==0){
                Debug.Log("Player died");
                this.hp=0;
                LevelManager.Instance.isOver=true;
            }
        }
        else{
            hp=0;
            healthBarText.text=Mathf.RoundToInt(this.hp).ToString();
            healthBar.value=Mathf.RoundToInt(this.hp);
            // Debug.Log("hp.value:"+healthBar.value);
        }
    }
    // public void OnStartGame(){
    //     Debug.Log("GAME START!!!");
    //     healthBar.gameObject.SetActive(true);
    //     gameObject.SetActive(true);
    //     isactive=true;
    //     Debug.Log("ISACTIVE = "+isactive +" And hp is:"+hp.ToString());
    // }


    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Eyyy");
    //     Destroy(gameObject);

    // }
    
    
}
