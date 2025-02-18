using _ImmersiveGames.Scripts.Utils.BusEventSystems;

namespace _ImmersiveGames.Scripts.GameSystems.BusEvents {
    public struct TestEvents : IEvent {
        
    }
    public struct PlayerEvents : IEvent {
        public int Health;
        public int Mana;
    }
}