using System;
using Actors;
using UnityEngine;

namespace Logic.Boat
{
    public interface IBoat
    {
        event Action<Actor> OnStartDelivery;
        event Action<Actor> OnDropOff;
        
        bool IsEmpty { get; }
        void SetCostDelivery(float cost);
        void SetDestinationPoint(Transform destination);
        bool TryStartDelivery(Actor passenger);
        void ShowIndicator();
        void HideIndicator();
    }
}