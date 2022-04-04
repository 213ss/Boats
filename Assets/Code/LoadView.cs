using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code
{
    public class LoadView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Awake()
        {
            StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            for (int i = 0; i < 100; i++)
            {


                if (text.text.Length >= 3)
                {
                    text.text = ".";
                }
                else
                {
                    text.text += ".";
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}