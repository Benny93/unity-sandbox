using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace com.emptystate
{
    public class UIScrollViewButtonWithImage : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Image image;       


        public void SetText(string s)
        {
            text.text = s;

        }

        public void SetImageSprite(Sprite sprite) {
            image.sprite = sprite;
        }
    }
}