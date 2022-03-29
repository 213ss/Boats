using Actors;
using Logic.Weapon.Weapons;
using UnityEngine;

public class ExcavateService : MonoBehaviour
{
    [SerializeField] private GameObject _imageObject;
    
    private ShovelWeapon _shovelWeapon;
    private Actor _actor;
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _actor = player.GetComponent<Actor>();
        _shovelWeapon = player.GetComponent<ShovelWeapon>();
    }

    public void ClickDig()
    {
        if(_actor.IsTravel) return;
        if(_actor.IsWin) return;
        if(_actor.IsDroppedOutGame) return;
            
        _shovelWeapon.Excavate();
    }

    private void Update()
    {
        if(_actor.IsDroppedOutGame || _actor.IsWin)
            _imageObject.SetActive(false);
    }
}
