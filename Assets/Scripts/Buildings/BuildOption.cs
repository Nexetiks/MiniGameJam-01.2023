using System;
using NoGround.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildOption : MonoBehaviour
    {
        [FormerlySerializedAs("OptionButton")]
        [SerializeField]
        private Button optionButton;
        [SerializeField]
        TextMeshProUGUI text;
        private Action<int> callback;
        private Building data;
        private int id;

        private void OnEnable()
        {
            optionButton.onClick.AddListener(OptionButton_OnClick);
        }

        private void OnDisable()
        {
            optionButton.onClick.RemoveListener(OptionButton_OnClick);
        }

        public void Select()
        {
            optionButton.Select();
        }

        public void Setup(Building data, int id, Action<int> callback)
        {
            this.data = data;
            this.callback = callback;
            this.id = id;
            text.text = data.name;
        }

        private void OptionButton_OnClick()
        {
            Debug.Log("Button selected");
            callback?.Invoke(id);
        }
    }
}