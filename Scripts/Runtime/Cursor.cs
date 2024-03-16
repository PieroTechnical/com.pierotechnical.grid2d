using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grid;

namespace Grid { 
    public class Cursor : MonoBehaviour
    {
        public static Cursor main;
        public Vector3Int position;

        private void Start()
        {
            main = this;
        }

        private void Update()
        {
            position = Vector3Int.RoundToInt(
                GridMath.ScreenToWorldPos3D(Input.mousePosition, true)
                );

            transform.position = position;
        }

        public static Vector3Int Position => main.position;
    }
}