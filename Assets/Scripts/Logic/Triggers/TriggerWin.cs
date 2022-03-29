using Actors;
using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    private readonly int _finalCamera = Animator.StringToHash("FinalCamera");

    [SerializeField] private Animator _cinemachineAnimator;
    

    private void SwitchCameraToWinCamera()
    {
        _cinemachineAnimator.Play(_finalCamera);
    }

    private void OnTriggerEnter(Collider other)
    {
        var actor = other.GetComponent<Actor>();
        actor.YouWin();
        actor.ActorTransform.rotation = transform.rotation;

        if (other.CompareTag("Player") == false)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().DroppedOutGame();
        }
        
        SwitchCameraToWinCamera();
    }
    
#if UNITY_EDITOR

    [Header("Only editor mode")]
    public Color ColorBox;
    public Vector3 BoxSize;


    private void OnDrawGizmos()
    {
        Gizmos.color = ColorBox;
        Gizmos.DrawWireCube(transform.position, BoxSize);
    }

#endif
}
