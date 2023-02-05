using Roots;
using UnityEngine;

/**
 * App is used to connect UI and other events with GameManager and govern it.
 */
[DisallowMultipleComponent]
public class App : MonoBehaviour
{
    public static App instance { get; set; }
    
    public AppData appData;
    public GameManager game;
    public UIManager ui;
    public Transform field;
    
    public new CameraController camera;
    
    private bool readyToStart;
    
    private void Awake()
    {
        if (App.instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        App.instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject
    }

    private void Start()
    {
        ui.cards.OnCardSelected += game.cardsManager.StartSelection;
        ui.cards.OnCardUnselected += game.cardsManager.CancelSelection;

        game.cardsManager.OnCardUsed += ui.cards.UseCard;
        game.cardsManager.OnCardAdded += ui.cards.AddCard;
        
        
        // Events.singleton.onUIStartGame.action += StartNormalGame;

        // game.inputManager.onTouchStartAction += (data, vec3) => { ResumeGame(); };
        // game.inputManager.onTouchStopAction += (data, vec3) => { PauseGame(); };

        // game.player.onDead += OnPlayerDeath;
        
        // Events.singleton.onUIRequestReset.action += () => { ResetState(); };

        // Events.singleton.addCoinFromUIEvent.action += (amount) => { game.AddCoins(amount); };
        
        readyToStart = true;
    }

    private void ResetState()
    {
        game.ResetState();
    }

    void PauseGame()
    {
        if (!game.isGameStarted || game.isGameCompleted)
            return;

        game.isGamePaused = true;

        // Events.singleton.onPauseStart.Raise();
    }

    void ResumeGame()
    {
        if (!game.isGameStarted || game.isGameCompleted)
            return;
        
        game.isGamePaused = false;

        // Events.singleton.onPauseEnd.Raise();
    }
    
    void Update()
    {


    }

    void StartGameInternal()
    {
        if (!readyToStart || game.isGameCompleted || game.isGameStarted)
            return;
        
        // game.energy.ApplyChange(-game.config.energyConfig.energyCostPerRun);

        game.SetStared();

        Events.singleton.OnGameStart.Invoke();
    }
    
    
}
