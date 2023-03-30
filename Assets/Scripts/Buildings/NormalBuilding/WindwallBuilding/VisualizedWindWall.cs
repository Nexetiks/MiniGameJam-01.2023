using System;
using System.Collections;
using Buildings.WindWallBuildings;
using UnityEngine;

namespace Buildings.WindwallBuilding
{
    public class VisualizedWindWall : WindWall
    {
        [SerializeField] private Transform wallTransform;
        public float TimeToLive = 10f;
        
        protected override void OnExecuteStart()
        {
            transform.position = windWallPosition;
            wallTransform.localScale = new Vector3(Width, 0.01f, 3f);
            transform.localRotation = Quaternion.Euler(0, 0, (int)Destination * 90f);
            StartCoroutine(Autodestruct());
        }

        protected override void OnExecutePulseUpdate()
        {
            base.OnExecutePulseUpdate();
            transform.position = windWallPosition;
        }

        private void OnValidate()
        {
            transform.localRotation = Quaternion.Euler(0, 0, (int)Destination * 90f);
        }
        
        IEnumerator Autodestruct()
        {
            yield return new WaitForSeconds(TimeToLive);
            Remove();
        }
    }
}
