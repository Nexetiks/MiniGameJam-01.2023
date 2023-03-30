using System;
using Buildings;
using UnityEngine;
using Screen = NoGround.UI.Screen;

namespace NoGround.Buildings
{
    public class BuildScreen : Screen
    {
        [SerializeField]
        private BuildOption firstOption;
        [SerializeField]
        private BuildOption secondOption;
        [SerializeField]
        private BuildScreenLogic logic;

        protected override void Awake()
        {
            base.Awake();
            logic.Screen = this;
        }

        public void Setup(Vector3 buildPosition, Action operationFinishedCallback)
        {
            logic.Setup(buildPosition, operationFinishedCallback);
            SetupOptions();
        }

        private void SetupOptions()
        {
            var options = logic.GetRandomBuildings();
            firstOption.Setup(options[0], 0, BuildOption_OnSelected);
            secondOption.Setup(options[1], 1, BuildOption_OnSelected);
            // Select the UI to gain the focus for keyboard input
            firstOption.Select();
        }

        private void BuildOption_OnSelected(int data)
        {
            logic.BuildOption(data);
        }
    }
}