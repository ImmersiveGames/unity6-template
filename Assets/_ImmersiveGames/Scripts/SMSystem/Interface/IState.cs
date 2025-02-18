﻿namespace _ImmersiveGames.Scripts.SMSystem.Interface {
    public interface IState {
        void Update();
        void FixedUpdate();
        void OnEnter();
        void OnExit();
    }
}