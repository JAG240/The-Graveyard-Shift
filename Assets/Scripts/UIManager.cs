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
    [SerializeField] private GameObject BodyProgress;

    private static UIManager _instance;
    public static UIManager Instance { get{ return _instance; }}

    private LevelManager _levelManager;
    private UIDocument _uiDoc;
    private GameSceneManager _sceneManager;
    private SoundManager _soundManager;

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

    private void Start()
    {
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void EndDay()
    {
        _uiDoc.visualTreeAsset = nextDayUI;

        VisualElement root = _uiDoc.rootVisualElement;

        Button nextDay = root.Q<Button>("NextDay");
        Label text = root.Q<Label>("Text");

        switch (_levelManager.GetDay())
        {
            case 1:
                text.text = "The first piece of your new body reminds me of all the time we spent together. I don't know what to do now that your gone but, I know that the only time I've enjoyed is our time together.";
                break;
            case 2:
                text.text = "The security on the grave seems to be increasing as word spreads about the grave sites turned upside down. I hear that people say about me. They say I'm sick, that I need help. I know that none of them can replace you. They cannot help me.";
                break;
            case 3:
                text.text = "These parts of you are not as you were. I hope that you can forgive me. I only want to make you happy, I only want to keep you to myself. I only want us together. Now and forever.";
                break;
            case 4:
                text.text = "Together again soon. I can hear your voice now. I can smell you and see you. With only 2 more parts left, I can see our future together.";
                break;
            case 5:
                text.text = "The smell has become horrible. I need you to know that I would do anything to have us together. I would do anything to have us together. I would do anything to have us together. I would do anything to have us together. I would do anything to have us together. ";
                break;
            case 6:
                text.text = "Finally I see you. Completed but, not as you were. I know you are confused, I know that you cannot understand. What I do not know, is why this pit in me is not filled. Was it a mistake to? Should I have let you rest?";
                break;
        }


        if (_levelManager.GetDay() == 6)
        {
            nextDay.visible = false;
        }

        DisplayBody();

        nextDay.clicked += LoadNextLevel;

    }

    private void DisplayBody()
    {
        foreach(string part in _levelManager.bodyParts)
        {
            GameObject check = BodyProgress.transform.Find(part).gameObject;

            if (check)
                check.SetActive(true);
        }

        BodyProgress.SetActive(true);
    }

    private void StartDay(int day)
    {
        if(day == 0)
        {
            _uiDoc.visualTreeAsset = nextDayUI;

            VisualElement root = _uiDoc.rootVisualElement;

            Button nextDay = root.Q<Button>("NextDay");

            Label text = root.Q<Label>("Text");
            text.text = "They all call me mad but, I'll show them. No! We'll show them! I will get the body parts I need from this graveyard and I will bring you back to life. Together we will show them all! I just need you! \nI will need to carry one body part back per night and use my shovel to dig up the body pieces to put you back together. \nLeft Click: Use Shovel / WASD: Move / STAY OUT OF SIGHT!";

            _levelManager.OverrideDay();

            nextDay.clicked += LoadNextLevel;
            return;
        }

        _uiDoc.visualTreeAsset = null;
    }

    private void LoadNextLevel()
    {
        VisualElement root = _uiDoc.rootVisualElement;

        Button nextDay = root.Q<Button>("NextDay");

        BodyProgress.SetActive(false);

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
            _soundManager.PlayGameMusic();
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
