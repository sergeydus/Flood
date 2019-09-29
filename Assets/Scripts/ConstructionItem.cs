using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Owner{Player,AI};
public class ConstructionItem : MonoBehaviour
{
    public string ItemName;
    public string ItemDescription;
    // public float ItemWeight;
    // public int Floatiness;
    public int Price;
    public float hp;
    public Sprite sprite;
    private Vector2 MousePos;
    public Rigidbody2D rb=null;
    // List<Collider2D> connecteditems;    
    [SerializeField]
    private Collider2D mycollider2D=null;
    public LevelManager levelManager;
    protected Color startcolor;// used for highlighting objects.
    public Owner owner;
    // Start is called before the first frame update
    // private List<ConstructionItem> connecteditems;
    void Start()
    {
        Debug.Log("START:"+name);
        levelManager=LevelManager.Instance;
        levelManager.items.Add(this);
        // connecteditems= new List<Collider2D>();
        startcolor=GetComponent<SpriteRenderer>().color;
        // rb=GetComponent<Rigidbody2D>();
        // mycollider2D=GetComponent<BoxCollider2D>();
        // Debug.Log(mycollider2D);
        // Debug.Log("gravity is:"+rb.gravityScale);
        rb.constraints = RigidbodyConstraints2D.FreezePosition|RigidbodyConstraints2D.FreezeRotation;
        // levelManager.items.Add(this);
        // Debug.Log((levelManager==null)?("Null"):("Not null"));
        // Debug.Log("Added this.");
    }
    public Collider2D itemcollider2d{
        get { return mycollider2D; }
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

    }
    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    public void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color=new Color(startcolor.r,startcolor.g,startcolor.b,.5f);
        transform.gameObject.layer=10;
        Debug.Log("Collision disabled");
        Debug.Log("Clicked me sexy!");
        // levelManager.GetConnectedToItem(this).ForEach()
        Debug.Log(levelManager.GetConnectedToItem(this).Count+"UNIQUE CONNECTIONS!!");
        levelManager.GetConnectedToItem(this).ForEach(x=>x.transform.SetParent(null));
        levelManager.GetConnectedToItem(this).ForEach(x=>x.transform.SetParent(transform));

    }
    /// <summary>
    /// OnMouseDrag is called when the user has clicked on a GUIElement or Collider
    /// and is still holding down the mouse.
    /// </summary>
    public void OnMouseDrag()
    { 
        Vector3 objpos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 diffvector=(new Vector3(objpos.x,objpos.y,transform.position.z))-transform.position;
        Debug.Log(diffvector);
        transform.position = Vector2.Lerp(transform.position, objpos, 0.5f);
        // rb.velocity=new Vector2(0,0);
        // rb.gravityScale=0;
        // rb.AddForce(new Vector2(diffvector.x*1000,1000*diffvector.y));
        // rb.velocity=new Vector2(diffvector.x*10,0);
        // rb.freezeRotation=true;
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            Debug.Log("MOSE WHEEEL");
            // rb.MoveRotation(rb.rotation+20f);
            // transform.Rotate (Vector3.forward*6f);
            var RotateCommand = new RotateItemsLeftCommand(levelManager.GetConnectedToItem(this),this);
            levelManager.commandProcessor.ExecuteCommand(RotateCommand);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            Debug.Log("mOUSE DOWN");
            // transform.Rotate(Vector3.back * 6f);
            var RotateCommand = new RotateItemsRightCommand(levelManager.GetConnectedToItem(this),this);
            levelManager.commandProcessor.ExecuteCommand(RotateCommand);
            // rb.rotation=rb.rotation-20f;
        }
        // if (Input.GetKeyDown(KeyCode.Mouse1)) {
        //     Debug.Log("Freeze!");
        //     rb.constraints = RigidbodyConstraints2D.FreezePosition|RigidbodyConstraints2D.FreezeRotation;
        // }
        // Debug.Log("X VELOCITY IS:"+rb.velocity.x);
        // rb.velocity=new Vector3(0,0,0);
        // rb.angularVelocity=new Vector3(100,100,0);
        // transform.position=new Vector3(objpos.x,objpos.y,transform.position.z);
    }
    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    public void OnMouseUp()
    {
        
        // var RemoveNeighbors = new EndMovementCommand(levelManager.GetConnectedToItem(this),this);
        // levelManager.commandProcessor.ExecuteCommand(RemoveNeighbors);
        Debug.Log("Mouse up");
        GetComponent<SpriteRenderer>().color=startcolor;
        transform.gameObject.layer=9;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("COLLIDED !!!");
    }
    public void BuyItem(){
        Debug.Log("bought");
    }
    private void OnDestroy() {
        Debug.Log("destroed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        levelManager.items.Remove(this);
        var myjoints=GetComponents<FixedJoint2D>();
        if(myjoints!=null){
            for (int i = 0; i < myjoints.Length; i++)
            {
                FixedJoint2D v = myjoints[i];
                Debug.Log("destroying joint:"+i+", connected component has:"+v.connectedBody.gameObject.GetComponents<FixedJoint2D>().Length+" joints.");
                var other =Array.Find(v.connectedBody.gameObject.GetComponents<FixedJoint2D>(),joint=>joint.connectedBody==this.rb);
                Debug.Log(other.gameObject.name + " connection destroyed");
                Destroy(Array.Find(v.connectedBody.gameObject.GetComponents<FixedJoint2D>(),joint=>joint.connectedBody==this.rb));
                Destroy(myjoints[i]);
            }
        }
        // levelManager.items.Remove(this);
    }
    
}
