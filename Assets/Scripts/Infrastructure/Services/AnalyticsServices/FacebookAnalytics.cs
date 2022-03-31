using Facebook.Unity;
using UnityEngine;

namespace Infrastructure.Services.AnalyticsServices
{
    public class FacebookAnalytics : MonoBehaviour
    {
        public static FacebookAnalytics Instance;
        
        private void Awake ()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            if (!FB.IsInitialized) 
            {
                FB.Init(InitCallback, OnHideUnity);
            } else 
            {
                FB.ActivateApp();
            }
            
            DontDestroyOnLoad(this);
        }

        private void InitCallback ()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            } else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity (bool isGameShown)
        {
            if (!isGameShown) 
            {
                Time.timeScale = 0;
            } else 
            {

                Time.timeScale = 1;
            }
        }
    }

}
