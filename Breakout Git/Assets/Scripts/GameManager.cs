using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text highscoreText;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelGameOver;
    public GameObject panelCompleted;

    public GameObject[] levels;

    public static GameManager Instance {get; private set;}

    public enum State {MENU, INIT, PLAY, LEVELCOMPLETED, LOAD, GAMEOVER}
    State state;
    GameObject currentBall;
    GameObject currentLevel;
    bool isSwitching;

    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; 
            scoreText.text = "SCORE: "+ score;
        }

    }

    private int level;
    public int Level
    {
        get { return level; }
        set { level = value;
            levelText.text = "LEVEL: " + level;
         }
    }

    private int balls;
    public int Balls
    {
        get { return balls; }
        set { balls = value;
            ballsText.text = "BALLS: " + balls;
         }
    }
    
    
    

    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if(currentBall == null)
                {
                    if(Balls > 0)
                    {
                        currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }
                if(currentLevel != null && currentLevel.transform.childCount == 0 && !isSwitching)
                {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOAD:
                break; 
            case State.GAMEOVER:
                if(Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;                  
        }
    }

    public void SwitchState(State newState, float delay=0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        isSwitching = true;
        yield return new WaitForSeconds(delay);
        EndState();
        state = newState;
        BeginState(newState);
        isSwitching = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                highscoreText.text = "HIGHSCORE: "+ PlayerPrefs.GetInt("highscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Balls = 3;
                if(currentLevel != null)
                {
                    Destroy(currentLevel);
                }
                Instantiate(playerPrefab);
                SwitchState(State.LOAD);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(currentBall);
                Destroy(currentLevel);
                Level++;
                panelCompleted.SetActive(true);
                SwitchState(State.LOAD, 2f); 
                break;
            case State.LOAD:
                if(Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break; 
            case State.GAMEOVER:
                if(Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                panelGameOver.SetActive(true);
                break;                  
        }

    }

    void EndState()
    {
        switch (state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelCompleted.SetActive(false);
                break;
            case State.LOAD:
                break; 
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;                  
        }
    }
}
