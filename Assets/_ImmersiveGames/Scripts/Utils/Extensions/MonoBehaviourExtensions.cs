using System;
using System.Collections;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.Extensions
{
    /// <summary>
    /// Classe de extensões para MonoBehaviour, fornecendo métodos utilitários para execução retardada e manipulação de componentes.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        #region DelayedExecution
        /// <summary>
        /// Executa uma ação após um determinado atraso em segundos.
        /// </summary>
        public static MonoBehaviour DelayedExecution(
            this MonoBehaviour monoBehaviour,
            float delay,
            Action callback
        )
        {
            monoBehaviour.StartCoroutine(Execute(delay, callback));
            return monoBehaviour;
        }

        private static IEnumerator Execute(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
        #endregion

        #region Delayed Execution Frame
        /// <summary>
        /// Executa uma ação no próximo frame.
        /// </summary>
        public static MonoBehaviour DelayedExecutionUntilNextFrame(
            this MonoBehaviour monoBehaviour,
            Action callback
        )
        {
            monoBehaviour.StartCoroutine(ExecuteAfterFrame(callback));
            return monoBehaviour;
        }

        private static IEnumerator ExecuteAfterFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
        #endregion

        #region Delayed Execution Until Condition True
        /// <summary>
        /// Executa uma ação quando uma condição específica for atendida.
        /// </summary>
        public static MonoBehaviour DelayedExecutionUntil(
            this MonoBehaviour monoBehaviour,
            Func<bool> condition,
            Action callback,
            bool expectedResult = true
        )
        {
            if (condition != null)
                monoBehaviour.StartCoroutine(WaitForCondition(condition, callback, expectedResult));
            return monoBehaviour;
        }

        private static IEnumerator WaitForCondition(
            Func<bool> condition,
            Action callback,
            bool expectedResult
        )
        {
            yield return new WaitUntil(() => condition() == expectedResult);
            callback?.Invoke();
        }
        #endregion

        #region Repeated Execution
        /// <summary>
        /// Executa uma ação repetidamente enquanto uma condição for atendida, com um intervalo entre as execuções.
        /// </summary>
        public static MonoBehaviour RepeatExecutionWhile(
            this MonoBehaviour monoBehaviour,
            Func<bool> condition,
            float interval,
            Action callback,
            bool expectedResult = true
        )
        {
            if (condition != null)
                monoBehaviour.StartCoroutine(
                    RepeatWhileCoroutine(condition, interval, callback, expectedResult)
                );
            return monoBehaviour;
        }

        private static IEnumerator RepeatWhileCoroutine(
            Func<bool> condition,
            float interval,
            Action callback,
            bool expectedResult
        )
        {
            while (condition() == expectedResult)
            {
                yield return new WaitForSeconds(interval);
                callback?.Invoke();
            }
        }
        #endregion

        /// <summary>
        /// Obtém um componente do GameObject ou adiciona um novo caso ele não exista.
        /// </summary>
        public static T GetOrAddComponent<T>(this MonoBehaviour behaviour)
            where T : Component
        {
            T component = behaviour.GetComponent<T>();
            return component != null ? component : behaviour.gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Adiciona um componente ao GameObject caso ele ainda não esteja presente.
        /// </summary>
        public static void AddComponentIfMissing<T>(this MonoBehaviour behaviour)
            where T : Component
        {
            if (behaviour.GetComponent<T>() == null)
                behaviour.gameObject.AddComponent<T>();
        }
    }
}
