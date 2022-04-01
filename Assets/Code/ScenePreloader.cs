using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class ScenePreloader : MonoBehaviour
    {
        [SerializeField] private string fstnaeme;
        [SerializeField] private string secnaeme;

        private void Awake()
        {
            StartCoroutine(AdditiveLoad());
        }

        private void Start()
        {
           
        }

        private IEnumerator AdditiveLoad()
        {
            var asyncOp = SceneManager.LoadSceneAsync (fstnaeme, LoadSceneMode.Additive);
            asyncOp.allowSceneActivation = true;
 
            Debug.Log("Start Load");
            
            while (!asyncOp.isDone)
            {
                yield return null;
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    Debug.Log($"Load end in {i}...");

            //    yield return new WaitForSeconds(1f);
            //}
            
            SceneManager.LoadScene(PlayerPrefs.GetInt("lvl",2));
        }
    }
}