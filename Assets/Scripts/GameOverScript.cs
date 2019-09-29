using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Player player=null;
    public GameObject GameOverUI;
    LevelManager levelManager;
    void Start()
    {
        levelManager=LevelManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!levelManager.isOver && player.hp==0){
            levelManager.isOver=true;
            StartCoroutine(Example());
        }    
    }
    IEnumerator Example()
    {
        player.rb.mass=5;
        print(Time.time);
        yield return new WaitForSeconds(1);
        Time.timeScale=0f;
        GameOverUI.SetActive(true);
        print(Time.time);
    }


    public void Quit(){
        Application.Quit();
    }
    public void Restart(){
        Time.timeScale=1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        // Application.LoadLevel(Application.loadedLevel);
    }
}
