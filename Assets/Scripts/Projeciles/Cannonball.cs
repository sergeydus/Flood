using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var velocity = GetComponent<Rigidbody2D>().velocity.magnitude;
        Debug.Log("cannonball flying with sleed:"+velocity);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        var constructionItem = other.gameObject.GetComponent<ConstructionItem>();

        if(constructionItem){
            constructionItem.hp-=this.damage;
        }
        Destroy(this.gameObject,3);
    }
}
