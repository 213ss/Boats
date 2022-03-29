using Actors;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " : Death zone");
        other.GetComponent<Actor>().DroppedOutGame();
    }
}
