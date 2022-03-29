using System.Collections.Generic;
using Logic.Boat;
using Logic.GoldLoot;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory
    {
        List<ISavedProgress> ProgressWriters { get; }
        List<IProgressReader> ProgressReaders { get; }
        void Cleanup();
        GameObject CreateGameObject(string path);
        GameObject CreateGameObject(GameObject prefab);
        GameObject CreateGameObject(string path, Vector3 at);
        GoldLoots CreateGoldLoot(Vector3 position);
        BaseBoat CreateBoat(Vector3 position);
    }
}