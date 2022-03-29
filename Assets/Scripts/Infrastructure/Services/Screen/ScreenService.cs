using UnityEngine;

namespace Infrastructure.Services.Screen
{
    public class ScreenService : MonoBehaviour, IScreenService
    {
        [SerializeField] private GameObject _pressTapScreen;
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _loseScreen;


        public void ShowPressTapScreen()
        {
            DisableEnableScreen(_pressTapScreen, true);
        }

        public void ShowWinScreen()
        {
            DisableEnableScreen(_winScreen, true);
        }

        public void ShowLoseScreen()
        {
            DisableEnableScreen(_loseScreen, true);
        }

        public void HideAllScreens()
        {
            bool isEnable = false;
            
            DisableEnableScreen(_pressTapScreen, isEnable);   
            DisableEnableScreen(_loseScreen, isEnable);   
            DisableEnableScreen(_winScreen, isEnable);   
        }

        private void DisableEnableScreen(GameObject screen, bool isEnable)
        {
            screen.SetActive(isEnable);
        }
    }
}