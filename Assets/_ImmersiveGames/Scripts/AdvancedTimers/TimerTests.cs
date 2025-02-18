using _ImmersiveGames.Scripts.AdvancedTimers.Timers;
using UnityEngine;

namespace _ImmersiveGames.Scripts.AdvancedTimers
{
    public class TimerTests : MonoBehaviour
    {
        private CountdownTimer _timer, _timer2;
        [SerializeField] private float timeDuration = 10f;
        [SerializeField] private float timeDuration2 = 8f;

        private void Start()
        {
            _timer = new CountdownTimer(timeDuration);
            _timer.OnTimerStart += () => { Debug.Log("Timer Start"); };
            _timer.OnTimerStop += () => { Debug.Log("Timer Stopped"); };
            _timer.Start();
            _timer2 = new CountdownTimer(timeDuration2);
            _timer2.OnTimerStart += () => { Debug.Log("Timer2 Start"); };
            _timer2.OnTimerStop += () => { Debug.Log("Timer2 Stopped"); };
            _timer2.Start();
        }

        private void OnDestroy()
        {
            _timer.Dispose();
            _timer2.Dispose();
        }
    }
}