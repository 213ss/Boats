using Actors;
using Scripts.Infrastructure.Services.Gold;
using UnityEngine;

namespace Infrastructure.Services.GoldLoot
{
    public interface IGoldLootService
    {
        void OnDropGold(IGoldChanger goldChanger, Vector3 spawnPosition, Actor owner);
    }
}