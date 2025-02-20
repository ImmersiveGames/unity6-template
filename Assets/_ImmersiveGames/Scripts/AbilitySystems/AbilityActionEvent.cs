using _ImmersiveGames.Scripts.Utils.BusEventSystems;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public struct AbilityActionEvent : IEvent {
        public AbilityData AbilityData { get; set; }
        public int AbilityIndex { get; set; }
        public object Source { get; set; }
    }

    public struct PlayerAnimationEvent : IEvent {
        public int AnimationHash;
    }
}