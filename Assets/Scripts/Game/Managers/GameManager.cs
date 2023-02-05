using System;
using System.Collections;
using System.Collections.Generic;
using Roots;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("App")]
    public AppData appData;

    [Header("Managers")]
    public InputManager inputManager;
    public SoundManager soundManager;
    public CardsManager cardsManager;
    public TreeManager treeManager;
    
    // [HideInInspector] public new CameraScript camera;

    [HideInInspector] public bool isGameStarted;
    [HideInInspector] public bool isGameCompleted;
    [HideInInspector] public bool isGamePaused;
    [HideInInspector] public bool isGameStopped;

    private float previousTimeScale;

    private bool playerTurnFinished;

    private enum EndState
    {
        Won,
        Failed
    }
    
	private void Awake()
    {
        // player.onDead += OnPlayerDeath;
        //
        // enemyManager.onEnemyDied += HandleEnemyDrop;
        // enemyMwanager.onEnemyDied += powerUpManager.OnEnemyDeathPowerupSpawn;
        // enemyManager.isTimerPaused += () => { return isGamePaused || isGameStopped; };
        //
        // timeManager.isGameStopped += () => { return isGameStopped; };

        // player.deathAllowedFuncs += IsPlayerAllowedToDie;
    }

    // Start is called before the first frame update
    void Start()
    {
        cardsManager.OnCardUsed += r => EndPlayerTurn();
        cardsManager.IsInputLocked += () => playerTurnFinished;

        cardsManager.OnShowEndPointsRequested += treeManager.ShowEndPoints;
        cardsManager.OnHideEndPointsRequested += treeManager.HideEndPoints;
        cardsManager.OnPreviewEndPointsRequested += treeManager.PreviewEndPoints;
        cardsManager.OnStopPreviewEndPointsRequested += treeManager.StopPreviewEndPoints;
        
        
        // camera = App.instance.camera.GetComponent<CameraScript>();

        // Gameplay
        /*mapManager.onMapReady += r => { StartCoroutine(SetupGame()); };
        mapManager.OnTileSelected += r => Events.singleton.ChangeDeselectVisibility.Invoke(true);
        mapManager.OnTileDeselected += r => Events.singleton.ChangeDeselectVisibility.Invoke(false);

        inputManager.IsInputLocked += () => playerTurnFinished;
        
        // Sfx
        inputManager.OnActionDenided += () => soundManager.PlaySound(SoundManager.SFXType.ActionDenied);
        inputManager.OnMoveOrder += () => soundManager.PlaySound(SoundManager.SFXType.MoveOrder);
        inputManager.OnSelected += () => soundManager.PlaySound(SoundManager.SFXType.Select);
        mapManager.OnTileDeselected += r => soundManager.PlaySound(SoundManager.SFXType.Deselect);
        teamsManager.OnDigStart += r => soundManager.PlaySound(SoundManager.SFXType.Dig);
        teamsManager.OnDigResult += r =>
        {
            if (r == null || r.Count == 0)
            {
                soundManager.PlaySound(SoundManager.SFXType.DigFail);
            }
            else
            {
                soundManager.PlaySound(SoundManager.SFXType.GotArtifact);
            }
        };
        teamsManager.OnTeamPathStepStarted += r => soundManager.PlaySound(SoundManager.SFXType.Land);
        
        // UI Raised
        Events.singleton.OnUIDeselectRequest += mapManager.DeselectAll;*/

        StartCoroutine(SetupGame());
    }

    IEnumerator SetupGame()
    {
        ResetState();
        
        // teamsManager.CreateStartingTeam();
        
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RunGameTurns());
    }

    IEnumerator RunGameTurns()
    {
        EndState endState;
        while (!GameEndCondition(out endState))
        {
            OnPlayerTurnStart();
            yield return PlayerActions();
            OnPlayerTurnFinished();
            
            // yield return new WaitForSeconds(2f);

            yield return TreeGrowth();
            // yield return TeamDigging();
        }
        
        Debug.Log("Game finished with state: " + endState);
    }

    private void OnPlayerTurnStart()
    {
        playerTurnFinished = false;
        
        cardsManager.PreviewActualSelection();
    }

    private void OnPlayerTurnFinished()
    {
    }


    private bool GameEndCondition(out EndState endState)
    {
        endState = EndState.Won;
        
        return false;
    }

    private IEnumerator PlayerActions()
    {
        yield return new WaitUntil(() => playerTurnFinished);
    }
    
    private IEnumerator TreeGrowth()
    {
        yield return new WaitUntil(() => !treeManager.IsWorking);
    }
    
    /*private IEnumerator TeamDigging()
    {
        yield return teamsManager.HandleDigging(SpendTurn);
    }*/

    public void ResetState()
    {
		isGameStarted = false;
        isGameCompleted = false;
        isGamePaused = false;
        isGameStopped = false;

        playerTurnFinished = true;
        // enemyManager.ResetState();
    }
    

    public void CompleteLevel()
    {
        if (isGameCompleted)
            return;

        StartCoroutine(CompleteLevelInternal());
    }

    private IEnumerator CompleteLevelInternal()
    {
        yield return new WaitForSeconds(1);

        Events.singleton.OnLevelCompleted.Invoke();
    }
    
    public void SetStopped(bool isStopped)
    {
        if (isStopped && !isGameStopped)
        {
            previousTimeScale = Time.timeScale;
            isGameStopped = true;
            Time.timeScale = 0;
        } 
        else if (!isStopped && isGameStopped)
        {
            Time.timeScale = previousTimeScale;
            isGameStopped = false;
        }
    }

    public void SetStared()
    {
        isGameStarted = true;
    }

    public void OnTreeExtended()
    {
        // teamsManager.ScheduleMove(teamTile, path);
        
        EndPlayerTurn();
    }


    private void EndPlayerTurn()
    {
        playerTurnFinished = true;
    }

}
