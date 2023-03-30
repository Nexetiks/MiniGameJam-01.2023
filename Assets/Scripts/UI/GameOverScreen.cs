using System;
using UnityEngine;
using UnityEngine.UI;

namespace NoGround.UI
{
    public class GameOverScreen : Screen
    {
        [SerializeField]
        private Button restartGameButton;

        private Action resetSelectedCallback;

        protected void OnEnable()
        {
            restartGameButton.onClick.AddListener(RestartGameButton_OnClick);
            // Select to enable keyboard ui input
            restartGameButton.Select();
        }

        protected void OnDisable()
        {
            restartGameButton.onClick.RemoveListener(RestartGameButton_OnClick);
        }

        public void Setup(Action resetSelectedCallback)
        {
            this.resetSelectedCallback = resetSelectedCallback;
        }

        private void RestartGameButton_OnClick()
        {
            resetSelectedCallback?.Invoke();
            Hide();
        }
    }
}