using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset nextDayUI;
    [SerializeField] private VisualTreeAsset mainMenuUI;
    [SerializeField] private VisualTreeAsset pauseMenuUI;

    private static UIManager _instance;
    public static UIManager Instance { get{ return _instance; }}

    private LevelManager _levelManager;
    private UIDocument _uiDoc;
    private GameSceneManager _sceneManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        _uiDoc = GetComponentInChildren<UIDocument>();
        SceneManager.sceneLoaded += CheckForGameScene;
    }

    private void EndDay()
    {
        _uiDoc.visualTreeAsset = nextDayUI;

        VisualElement root = _uiDoc.rootVisualElement;

        Button nextDay = root.Q<Button>("NextDay");

        nextDay.clicked += LoadNextLevel;

    }

    private void StartDay(int day)
    {
        _uiDoc.visualTreeAsset = null;
    }

    private void LoadNextLevel()
    {
        VisualElement root = _uiDoc.rootVisualElement;

        Button nextDay = root.Q<Button>("NextDay");

        nextDay.clicked -= LoadNextLevel;

        _levelManager.StartDay();
    }

    private void PauseDay(bool state)
    {
        if (state)
        {
            _uiDoc.visualTreeAsset = pauseMenuUI;
            VisualElement root = _uiDoc.rootVisualElement;
            Button resume = root.Q<Button>("Resume");
            Button quit = root.Q<Button>("Quit");
            quit.clicked += Application.Quit;
            resume.clicked += Pause;
        }
        else
        {
            VisualElement root = _uiDoc.rootVisualElement;
            Button resume = root.Q<Button>("Resume");
            Button quit = root.Q<Button>("Quit");
            resume.clicked -= Pause;
            quit.clicked -= Application.Quit;
            _uiDoc.visualTreeAsset = null;
        }
    }

    private void ExitToMainMenu()
    {
        VisualElement root = _uiDoc.rootVisualElement;
        Button quit = root.Q<Button>("Quit");
        quit.clicked -= ExitToMainMenu;
        _levelManager.endDay -= EndDay;
        _levelManager.startDay -= StartDay;
        _levelManager.pauseDay -= PauseDay;
        _sceneManager.LoadScene("Main Menu");
    }

    private void Pause()
    {
        _levelManager.PauseDay();
    }

    private void CheckForGameScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Graveyard")
        {
            _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            _levelManager.endDay += EndDay;
            _levelManager.startDay += StartDay;
            _levelManager.pauseDay += PauseDay;
        }
        else if(scene.name == "Main Menu")
        {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<GameSceneManager>();

        _uiDoc.visualTreeAsset = mainMenuUI;

        VisualElement root = _uiDoc.rootVisualElement;

        Button play = root.Q<Button>("Play");
        Button quit = root.Q<Button>("Quit");

        play.clicked += LoadGraveyard;
        quit.clicked += Application.Quit;
    }

    private void LoadGraveyard()
    {
        VisualElement root = _uiDoc.rootVisualElement;

        Button play = root.Q<Button>("Play");
        Button quit = root.Q<Button>("Quit");
        play.clicked -= LoadGraveyard;
        quit.clicked -= Application.Quit;
        _uiDoc.visualTreeAsset = null;

        _sceneManager.LoadScene("Graveyard");
    }
}
