using NoGround.Character;
using System;
using TMPro;
using UnityEngine;

namespace NoGround.UI
{
    public class GameplayScreen : Screen
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private TextMeshProUGUI proceedText;
        [SerializeField]
        private TextMeshProUGUI healthText;

        protected override void Awake()
        {
            base.Awake();
            proceedText.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateScoreText();
            UpdateProceedText();
            UpdateHealthText();
        }

        private void UpdateHealthText()
        {
            healthText.text = $"HP {PlayerCharacter.Instance.HitPoints.Value}";
        }

        private void UpdateScoreText()
        {
            int stage = GameManager.Instance.CurrentStage + 1;
            ulong score = GameManager.Instance.ThisLevelScore;
            ulong scoreCap = GameManager.Instance.ScoreRequiredForNextStage[GameManager.Instance.CurrentStage];
            scoreText.text = $"[Stage {stage}] {score}/{scoreCap} points";
        }

        private void UpdateProceedText()
        {
            if (proceedText.gameObject.activeInHierarchy != GameManager.Instance.IsAtTheEndOfStage)
                proceedText.gameObject.SetActive(GameManager.Instance.IsAtTheEndOfStage);
        }
    }
}