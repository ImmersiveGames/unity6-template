using _ImmersiveGames.Scripts.AdvancedTimers;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerDetectorSystem {
    public interface IDetectionStrategy {
        bool Execute(Transform player, Transform detector, CountdownTimer timer);
    }
}