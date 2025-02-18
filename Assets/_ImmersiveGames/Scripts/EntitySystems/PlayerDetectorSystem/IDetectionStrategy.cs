using _ImmersiveGames.Scripts.AdvancedTimers.Timers;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerDetectorSystem {
    public interface IDetectionStrategy {
        bool Execute(Transform player, Transform detector, CountdownTimer timer);
    }
}