using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
    public GameObject prefab;
    public int NumbersToCreate; 
    // Start is called before the first frame update
    void Start()
    {
        // populate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void populate(){
        GameObject newObj;
        for(int i=0;i<NumbersToCreate;i++){
            newObj= (GameObject)Instantiate(prefab,transform);//creates new instances of our prefab till we got as many as numberstocreate
            newObj.GetComponent<Image>().color= Random.ColorHSV();//assign random color
        }
    }
}
