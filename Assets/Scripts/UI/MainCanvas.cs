using NoGround.Buildings;
using UnityEngine;

namespace NoGround.UI
{
    /// <summary>
    /// Manages screens used in the game.
    /// Add here your screens so GameManager and stays clean.
    /// </summary>
    public class MainCanvas : MonoBehaviour
    {
        public BuildScreen PlaceBuildingScreen;
        public GameOverScreen GameOverScreen;
        public GameplayScreen GameplayScreen;
    }
}