using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;      //Static instance of GameManager which allows it to be accessed by any other script.
    private StarField starfield;
    private GameObject canvasMain;
    private GameObject canvasStats;

    //Start
    void Start()
    {
        canvasMain = GameObject.Find("Canvas_Main");
        canvasMain.SetActive(false);
        canvasStats = GameObject.Find("Canvas_Stats");
        canvasStats.SetActive(false);
    }

    //Update is called every frame.
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (canvasMain.activeSelf)
            {
                canvasMain.SetActive(false);
            }
            else
            {
                canvasMain.SetActive(true);
            }
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (canvasStats.activeSelf)
            {
                canvasStats.SetActive(false);
            }
            else
            {
                canvasStats.SetActive(true);
            }
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            starfield.SetMsgNew(true);
            canvasMain.SetActive(false);
            canvasStats.SetActive(false);
        }
    }

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

        //if not, set instance to this
        instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

        //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
        Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference
        starfield = GetComponent<StarField>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    public void InitGame()
    {
        starfield.SetupScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}