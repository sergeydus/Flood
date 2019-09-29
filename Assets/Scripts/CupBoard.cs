using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupBoard : ConstructionItem
{
    public Slider healthBar;
    public Text healthBarText;
    // Start is called before the first frame update
    void Update()
    {

        if(hp==0 && owner==Owner.AI){
            Debug.Log("game over");
            Destroy(this.gameObject);
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
}
