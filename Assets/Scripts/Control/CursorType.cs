using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "CursorType", menuName = "ScriptableObjects/Create Cursor", order = 2)]
    public class CursorType : ScriptableObject
    {
        [SerializeField] Texture2D texture = null;
        [SerializeField] Vector2 hotspot = Vector2.zero;
        [SerializeField] CursorMode cursorMode = CursorMode.Auto;

        public void SetCursor()
        {
            if (texture == null)
            {
                Debug.Log("Cursor texture is not set.");
                return;
            }
            Cursor.SetCursor(texture, hotspot, cursorMode);
        }
    }
}