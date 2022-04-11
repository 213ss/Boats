using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.LevelService
{
    public class LoadLevel : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}