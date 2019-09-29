using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenuUI;
    LevelManager levelManager;
    void Start()
    {
        levelManager=LevelManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(levelManager.isPaused){
                Resume();
            }
            else{
                Pause();
            }
            Debug.Log("Escape pressed");
        }   
    }
    public void Pause()
    {
        if(levelManager.isOver==false){
            pauseMenuUI.SetActive(true);
            Time.timeScale=0f;
            levelManager.isPaused=true;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale=1f;
        levelManager.isPaused=false;
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
