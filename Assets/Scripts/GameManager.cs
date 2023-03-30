using System;
using System.Collections.Generic;
using Core.DecisionMaking.StateMachine;
using NoGround.Buildings;
using NoGround.Character;
using NoGround.GameStates;
using NoGround.Pulses;
using NoGround.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace NoGround
{
    public class GameManager : Singleton<GameManager> //zmiany na maxie 
    {
        public event Action OnStageCompleted;

        public List<ITakePulse> TakePulseList = new List<ITakePulse>();
        public HashSet<Building> AllBuildings = new();
        [FormerlySerializedAs("IsAtTheEndOfSTage")]
        public bool IsAtTheEndOfStage = false;
        // Group for all screens
        public MainCanvas MainCanvas;

        [field: SerializeField]
        public ulong Score { get; private set; } = 0;
        [field: SerializeField]
        public ulong ThisLevelScore { get; private set; }
        [field: SerializeField]
        public int CurrentStage { get; private set; } = 0;

        public List<ulong> ScoreRequiredForNextStage = new List<ulong>();

        private FiniteStateMachine fsm;
        // Transitions used as triggers to switch the states
        Transition startGameTransition = new Transition("Start game");
        Transition enterBuildModeTransition = new Transition("Enter build mode");
        Transition exitBuildModeTransition = new Transition("Exit build mode");
        Transition endGameTransition = new Transition("End game");
        Transition restartGameTransition = new Transition("Restart game");

        private void Awake()
        {
            ScoreRequiredForNextStage.Add(2);
            ScoreRequiredForNextStage.Add(3);

            for (int i = 2; i <= 100; i++)
            {
                ScoreRequiredForNextStage.Add(ScoreRequiredForNextStage[i - 1] + ScoreRequiredForNextStage[i - 2]);
            }

            SetupStates();
        }

        /// <summary>
        /// Setups the states and transitions between them.
        /// </summary>
        private void SetupStates()
        {
            fsm = new();

            // Setup the game
            State setupState = new State("Setup State");
            State gameplayState = new State("Gameplay State");
            // Pause the game
            // Show the UI
            State buildingState = new State("Building State");
            // Disable character controls
            // Disable counting the points
            // Show endgame ui
            State gameOverState = new State("Game Over State");

            startGameTransition.SetTargetState(gameplayState);
            enterBuildModeTransition.SetTargetState(buildingState);
            exitBuildModeTransition.SetTargetState(gameplayState);
            endGameTransition.SetTargetState(gameOverState);
            restartGameTransition.SetTargetState(setupState);

            setupState.AddEntryAction(() => PauseGame(true));
            setupState.AddUpdateAction(() => startGameTransition.Trigger());
            setupState.AddExitAction(() => PauseGame(false));
            setupState.AddTransition(startGameTransition);

            gameplayState.AddEntryAction(() => MainCanvas.GameplayScreen.Show());
            PlayerCharacter.Instance.HitPoints.OnHitPointsDepleted += () => endGameTransition.Trigger();
            gameplayState.AddExitAction(() => MainCanvas.GameplayScreen.Hide());
            gameplayState.AddTransition(enterBuildModeTransition);
            gameplayState.AddTransition(endGameTransition);

            buildingState.AddEntryAction(() => PauseGame(true));
            buildingState.AddExitAction(() => PauseGame(false));
            buildingState.AddTransition(exitBuildModeTransition);

            gameOverState.AddTransition(restartGameTransition);
            gameOverState.AddEntryAction(() => PauseGame(true));
            gameOverState.AddEntryAction(() => MainCanvas.GameOverScreen.Setup(restartGameTransition.Trigger));
            gameOverState.AddEntryAction(() =>
            {
                MainCanvas.GameplayScreen.Show();
                MainCanvas.GameOverScreen.Show();
            });
            gameOverState.AddExitAction(() => PauseGame(false));
            gameOverState.AddExitAction(ResetGame);

            fsm.SetInitialState(setupState);
            fsm.ExecuteActions();
        }

        private void Update()
        {
            fsm.ExecuteActions();
        }

        private void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void PauseGame(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
            // Disable the character input so we don't trigger character actions during the pause
            PlayerCharacter.Instance.EnableCharacterInput(!pause);
        }

        public void AddScore(ulong points)
        {
            if (IsAtTheEndOfStage)
            {
                return;
            }

            Score += points;
            ThisLevelScore += points;
            CheckStage();
        }

        private void CheckStage()
        {
            if (ThisLevelScore == ScoreRequiredForNextStage[CurrentStage])
            {
                IsAtTheEndOfStage = true;
            }

            if (ThisLevelScore > ScoreRequiredForNextStage[CurrentStage])
            {
                ThisLevelScore = 0;
                CurrentStage++;
                OnStageCompleted?.Invoke();
            }
        }

        public void StartBuildingProcess(Vector3 position)
        {
            enterBuildModeTransition.Trigger();
            MainCanvas.PlaceBuildingScreen.Setup(position, PlacingBuildingFinished);
            MainCanvas.PlaceBuildingScreen.Show();
        }

        public void InterruptBuildingProcess()
        {
        }

        public void PlacingBuildingFinished()
        {
            IsAtTheEndOfStage = false;
            exitBuildModeTransition.Trigger();
        }

        public void BuildBuildingAtEndOfState()
        {
            IsAtTheEndOfStage = false;
            AddScore(1);
        }
    }
}