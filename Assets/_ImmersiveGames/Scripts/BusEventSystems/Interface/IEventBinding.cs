using System;
namespace _ImmersiveGames.Scripts.BusEventSystems {
    internal interface IEventBinding<T> {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
}