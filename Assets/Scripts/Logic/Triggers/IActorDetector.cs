using System;
using Actors;

namespace Logic.Triggers
{
    public interface IActorDetector
    {
        event Action<Actor> OnDetectActor;
        event Action OnDetect;
        void EnableDetect();
        void DisableDetect();
    }
}