﻿using _ImmersiveGames.Scripts.SMSystem.Interface;

namespace _ImmersiveGames.Scripts.SMSystem {
    public class Transition : ITransition {
        public IState To { get; }
        public IPredicate Condition { get; }

        public Transition(IState to, IPredicate condition) {
            To = to;
            Condition = condition;
        }
    }
}