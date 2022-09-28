using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI tmp = null;
        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float value)
        {
            tmp.SetText(value.ToString());
        }
    }
}