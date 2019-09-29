using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIndicatorScript : MonoBehaviour
{
    public static DragIndicatorScript Instance{get;private set;}
    Vector3 startPos;
    Vector3 endPos;
    new Camera camera;
    LineRenderer lineRenderer=null;
    Vector3 camOffset = new Vector3(0,0,10);
    [SerializeField] AnimationCurve animationCurve=null;
    public int numCapVertices;// how round is the drag indicatior
    float distLimit=3;
    public Vector3 dragStrength;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance==null){
            Instance=this;
            camera=Camera.main;
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start(){
        levelManager=LevelManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!levelManager.isOver && levelManager.gamestarted && !levelManager.isPaused){

            if(lineRenderer==null){
                lineRenderer=gameObject.AddComponent<LineRenderer>();
            }
            if(Input.GetMouseButtonDown(0)){//when mouse clicked
                lineRenderer.enabled=true;
                lineRenderer.positionCount=2;
                startPos=camera.ScreenToWorldPoint(Input.mousePosition)+camOffset;
                lineRenderer.SetPosition(0,startPos);   
                lineRenderer.useWorldSpace=true;
                lineRenderer.numCapVertices=numCapVertices;
                lineRenderer.widthCurve=animationCurve;
            }
            if(Input.GetMouseButton(0)){//when mouse held
                endPos=camera.ScreenToWorldPoint(Input.mousePosition)+camOffset;
                float distance=Vector3.Distance(endPos,startPos);
                if(distance>distLimit){
                    Debug.Log("Distance limit!");
                    endPos=startPos+Vector3.ClampMagnitude(endPos-startPos,distLimit);
                }
                dragStrength=startPos-endPos;
                lineRenderer.SetPosition(1,endPos);


            }
            if(Input.GetMouseButtonUp(0)){
                lineRenderer.enabled=false;
            }
        }
    }
}
