using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.Utils.Extensions
{
    /// <summary>
    /// Classe de extensões para manipulação de dicionários.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adiciona um novo par chave-valor ao dicionário ou atualiza o valor caso a chave já exista.
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value
        )
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);

            return dictionary;
        }

        /// <summary>
        /// Obtém a chave correspondente a um determinado valor no dicionário.
        /// </summary>
        public static TKey GetKeyByValue<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TValue value
        )
        {
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (pair.Value.Equals(value))
                {
                    return pair.Key;
                }
            }
            return default;
        }

        /// <summary>
        /// Verifica se o dicionário contém uma chave e se o valor associado não é nulo.
        /// </summary>
        public static bool ContainsAndNotNull<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key
        )
            where TValue : UnityEngine.Object =>
            dictionary.ContainsKey(key) && dictionary[key] != null;
    }
}