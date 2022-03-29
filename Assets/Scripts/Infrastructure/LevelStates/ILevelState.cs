﻿namespace Infrastructure.LevelStates
{
    public interface ILevelState
    {
        void Enter();
        void Exit();
    }
}