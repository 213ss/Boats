using System;
using GameAnalyticsSDK;
using UnityEngine;

namespace Infrastructure.Services.AnalyticsServices
{
    public enum TakeGoldType
    {
        Excavating,
        PickedUpCoins,
        CurrentGold
    }

    public enum DropGoldType
    {
        TakeDamage,
        EnterHoleTrigger
    }
    
    public enum LevelProgressingStatus
    {
        Start,
        Complete,
        Fail
    }
    
    public class GameAnalyticsService : MonoBehaviour
    {
        public static GameAnalyticsService Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                GameAnalytics.Initialize();
                GameAnalytics.StartSession();
                
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayerProgress(LevelProgressingStatus status, float goldCount)
        {
            switch (status)
            {
                case LevelProgressingStatus.Start:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_1");
                    break;
                case LevelProgressingStatus.Complete:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, 
                        "Level_1", goldCount.ToString());
                    break;
                case LevelProgressingStatus.Fail:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, 
                        "Level_1");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
            
            //GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus,
                //string progression01, string progression02);
        }

        public void EventTakeResource(float count, TakeGoldType takeType)
        {
            //GameAnalytics.NewResourceEvent (GA_Resource.GAResourceFlowType flowType,
            //string resourceType, float amount, string itemType, string itemID);
            
            switch (takeType)
            {
                case TakeGoldType.Excavating:
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, 
                        "Gold", count, "Excavating_gold", "Shovel");
                    break;
                case TakeGoldType.PickedUpCoins:
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, 
                        "Gold", count, "PickedUp_Coins", "Coin");
                    break;
                case TakeGoldType.CurrentGold:
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, 
                        "Gold", count, "CurrentAmounts_Gold", "Player_gold");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(takeType), takeType, null);
            }
        }

        public void EventDropResource(float amount, DropGoldType dropGoldType)
        {
            switch (dropGoldType)
            {
                case DropGoldType.TakeDamage:
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, 
                        "Gold", amount, "Take damage", "Shovel strike");
                    break;
                case DropGoldType.EnterHoleTrigger:
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, 
                        "Gold", amount, "Enter to hole", "Throw away coins");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dropGoldType), dropGoldType, null);
            }
        }
    }
}
