using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _ImmersiveGames.Scripts.DebugSystems
{
    public static class DebugManager
    {
        private static bool _globalDebugEnabled = true;
        private static readonly Dictionary<Type, DebugLevels> ScriptDebugLevels = new Dictionary<Type, DebugLevels>();
        private static DebugLevels _defaultDebugLevel = DebugLevels.Logs; // Nível padrão se não configurado

        public enum DebugLevels
        {
            None = 0,       // Sem logs
            Logs = 1,       // Apenas mensagens informativas
            Warning = 2,    // Mensagens informativas + avisos
            Error = 3,      // Mensagens informativas + avisos + erros
            Verbose = 4     // Tudo, incluindo detalhes extras (ex.: estado completo)
        }

        #region Configuração

        /// <summary>
        /// Ativa ou desativa todos os logs globalmente.
        /// </summary>
        public static void SetGlobalDebugState(bool enabled) => _globalDebugEnabled = enabled;

        /// <summary>
        /// Define o nível de debug padrão para roteiros não configurados explicitamente.
        /// </summary>
        public static void SetDefaultDebugLevel(DebugLevels level) => _defaultDebugLevel = level;

        /// <summary>
        /// Define o nível de debug para um tipo específico de script.
        /// </summary>
        public static void SetScriptDebugLevel<T>(DebugLevels debugLevel) => ScriptDebugLevels[typeof(T)] = debugLevel;

        private static DebugLevels GetDebugLevel(Type scriptType)
        {
            return ScriptDebugLevels.GetValueOrDefault(scriptType, _defaultDebugLevel);
        }

        private static bool ShouldLog(DebugLevels localLevel, DebugLevels messageLevel)
        {
            return _globalDebugEnabled && localLevel != DebugLevels.None && (int)localLevel >= (int)messageLevel;
        }

        #endregion

        #region Métodos de Log

        /// <summary>
        /// Registra uma mensagem informativa no console.
        /// </summary>
        public static void Log<T>(string message, Object context = null)
        {
            var scriptType = typeof(T);
            var level = GetDebugLevel(scriptType);
            if (ShouldLog(level, DebugLevels.Logs))
            {
                Debug.Log($"[INFO] [{scriptType.Name}] {message}", context);
            }
        }

        /// <summary>
        /// Registra uma mensagem de aviso no console.
        /// </summary>
        public static void LogWarning<T>(string message, Object context = null)
        {
            var scriptType = typeof(T);
            var level = GetDebugLevel(scriptType);
            if (ShouldLog(level, DebugLevels.Warning))
            {
                Debug.LogWarning($"[WARNING] [{scriptType.Name}] {message}", context);
            }
        }

        /// <summary>
        /// Registra uma mensagem de erro no console.
        /// </summary>
        public static void LogError<T>(string message, Object context = null)
        {
            var scriptType = typeof(T);
            var level = GetDebugLevel(scriptType);
            if (ShouldLog(level, DebugLevels.Error))
            {
                Debug.LogError($"[ERROR] [{scriptType.Name}] {message}", context);
            }
        }

        /// <summary>
        /// Registra uma exceção no console.
        /// </summary>
        public static void LogException<T>(Exception exception, Object context = null)
        {
            var scriptType = typeof(T);
            var level = GetDebugLevel(scriptType);
            if (ShouldLog(level, DebugLevels.Error))
            {
                Debug.LogException(new Exception($"[{scriptType.Name}] {exception.Message}", exception), context);
            }
        }

        /// <summary>
        /// Registra uma mensagem detalhada (verbose) no console, útil para debug avançado.
        /// </summary>
        public static void LogVerbose<T>(string message, Object context = null)
        {
            var scriptType = typeof(T);
            var level = GetDebugLevel(scriptType);
            if (ShouldLog(level, DebugLevels.Verbose))
            {
                Debug.Log($"[VERBOSE] [{Time.frameCount}] [{scriptType.Name}] {message}", context);
            }
        }

        #endregion
    }
}