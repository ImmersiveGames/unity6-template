namespace _ImmersiveGames.Scripts.BusEventSystems {
    public struct TestEvents : IEvent { }
    public struct PlayerEvents : IEvent {
        public int Health;
        public int Mana;
    }
}