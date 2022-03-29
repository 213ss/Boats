using System.Collections.Generic;
using Infrastructure.Data.ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Services.SkinChanger
{
    public class SkinChanger : MonoBehaviour, ISkinChanger
    {
        [Header("Defaults skin")]
        [SerializeField] private ActorSkinData _defaultMainActorSkin;
        [SerializeField] private ActorSkinData _defaultEnemyActorSkin;

        [Header("All skins")] 
        [SerializeField] private List<ActorSkinData> _allActorSkins = new List<ActorSkinData>();

        private Dictionary<string, ActorSkinData> _skins = new Dictionary<string, ActorSkinData>();

        public void InitSkinContainer()
        {
            foreach (var skin in _allActorSkins)
            {
                _skins.Add(skin.SkinName, skin);
            }
        }

        public bool TrySetSkin(string skinName, ISkin skin)
        {
            var successful = _skins.TryGetValue(skinName, out var skinData);
            
            if (successful)
            {
                skin.ChangeSkin(skinData);
            }

            return successful;
        }

        public void SetSkin(int hash, ISkin skin)
        {
            if (hash < _skins.Count)
            {
                skin.ChangeSkin(_allActorSkins[hash]);
            }
        }

        public void SetDefaultMainActorSkin(ISkin skin)
        {
            skin.ChangeSkin(_defaultMainActorSkin);
        }

        public void SetDefaultEnemyActorSkin(ISkin skin)
        {
            skin.ChangeSkin(_defaultEnemyActorSkin);
        }


    }
}