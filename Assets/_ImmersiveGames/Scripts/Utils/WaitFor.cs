using System.Collections.Generic;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils {
    public static class WaitFor {
        private static readonly WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
        public static WaitForFixedUpdate FixedUpdate => _fixedUpdate;

        private static readonly WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
        public static WaitForEndOfFrame EndOfFrame => _endOfFrame;

        private static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDict = new(100, new FloatComparer());
        public static WaitForSeconds Seconds(float seconds) {
            if (seconds < 1f / Application.targetFrameRate) return null;
            if (WaitForSecondsDict.TryGetValue(seconds, out var forSeconds)) return forSeconds;
            forSeconds = new WaitForSeconds(seconds);
            WaitForSecondsDict[seconds] = forSeconds;
            return forSeconds;
        }

        private class FloatComparer : IEqualityComparer<float> {
            public bool Equals(float x, float y) => Mathf.Abs(x - y) <= Mathf.Epsilon;
            public int GetHashCode(float obj) => obj.GetHashCode();
        }
    }
}