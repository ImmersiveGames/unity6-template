using System.Runtime.CompilerServices;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.Extensions
{
    /// <summary>
    /// Fornece métodos de extensão para conversão entre os tipos de vetor do 'System.Numerics' e UnityEngine.
    /// </summary>
    public static class VectorConversionExtensions
    {
        /// <summary>
        /// Converte um <see cref="System.Numerics.Vector2"/> para um <see cref="UnityEngine.Vector2"/>.
        /// </summary>
        /// <param name="vector">O vetor do System.Numerics a ser convertido.</param>
        /// <returns>Um <see cref="UnityEngine.Vector2"/> equivalente.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToUnityVector(this System.Numerics.Vector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        /// Converte um <see cref="UnityEngine.Vector2"/> para um <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        /// <param name="vector">O vetor do UnityEngine a ser convertido.</param>
        /// <returns>Um <see cref="System.Numerics.Vector2"/> equivalente.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Numerics.Vector2 ToSystemVector(this Vector2 vector)
        {
            return new System.Numerics.Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Converte um <see cref="System.Numerics.Vector3"/> para um <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        /// <param name="vector">O vetor do System.Numerics a ser convertido.</param>
        /// <returns>Um <see cref="UnityEngine.Vector3"/> equivalente.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ToUnityVector(this System.Numerics.Vector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Converte um <see cref="UnityEngine.Vector3"/> para um <see cref="System.Numerics.Vector3"/>.
        /// </summary>
        /// <param name="vector">O vetor do UnityEngine a ser convertido.</param>
        /// <returns>Um <see cref="System.Numerics.Vector3"/> equivalente.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Numerics.Vector3 ToSystemVector(this Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
        }
    }
}
