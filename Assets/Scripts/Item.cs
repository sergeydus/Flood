using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Flood/Item", order = 0)]
public class Item : ScriptableObject {
    public new string name;
    public string description;
    public string weight;
    public string hp;
    public string floatiness;
    public Sprite sprite;
    public Material texture;
    
    public void Print(){
        Debug.Log("name:"+name+" Description:"+description);
    }
    
}
