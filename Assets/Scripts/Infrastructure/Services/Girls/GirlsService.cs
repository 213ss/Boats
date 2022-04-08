using UnityEngine;

namespace Infrastructure.Services.Girls
{
    public class GirlsService : MonoBehaviour, IGirlsService
    {
        [SerializeField] private GameObject[] _girls;


        private Animator[] _girlsAnimator;
        
        private void Start()
        {
            _girlsAnimator = new Animator[_girls.Length];
            for (int i = 0; i < _girls.Length; i++)
            {
                _girlsAnimator[i] = _girls[i].GetComponent<Animator>();
            }
        }

        public void StartGirlDancing(int animIndex)
        {
            for (int i = 0; i < _girls.Length; ++i)
            {
                _girls[i].SetActive(true);
                _girlsAnimator[i].SetInteger("finalAnim", animIndex);
                _girlsAnimator[i].SetTrigger("GoDance");
            }
        }
    }
}