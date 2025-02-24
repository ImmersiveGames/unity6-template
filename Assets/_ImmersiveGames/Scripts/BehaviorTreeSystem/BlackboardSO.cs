using System.Collections.Generic;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    [CreateAssetMenu(fileName = "Blackboard", menuName = "ImmersiveGames/Behavior/Blackboard")]
    public class BlackboardSo : ScriptableObject
    {
        // Campos serializados para configuração estática no Inspector
        [SerializeField] private Vector3 position;
        [SerializeField] private Transform target;
        [SerializeField] private GameObject owner;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform spawnPoint;


        // Propriedades públicas para acesso dinâmico
        public Vector3 Position { get => position; set => position = value; }
        public Transform Target { get => target; set => target = value; }
        public GameObject Owner { get => owner; set => owner = value; }
        public Animator Animator { get => animator; set => animator = value; }
        public Transform SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        // Dados dinâmicos (não serializados)
        private readonly Dictionary<string, object> runtimeMemory = new Dictionary<string, object>();

        // Método para resetar o estado dinâmico
        public void Reset()
        {
            runtimeMemory.Clear();
            // Não resetamos campos serializados (Alvo, SpawnPoint, etc.) aqui para manter configurações do Inspector
            position = Owner != null ? Owner.transform.position : Vector3.zero;
        }

        // Método genérico para inicializar referências dinâmicas do Owner
        public void Initialize(GameObject ownerInstance)
        {
            Owner = ownerInstance;
            if (ownerInstance == null) return;
            Animator = ownerInstance.GetComponent<Animator>();
            position = ownerInstance.transform.position;

            // Se SpawnPoint não estiver configurado no Inspector, tenta encontrar dinamicamente
            if (SpawnPoint != null) return;
            var spawn = ownerInstance.transform.Find("SpawnPoint");
            SpawnPoint = spawn != null ? spawn : ownerInstance.transform;
        }

        // Métodos genéricos para valores dinâmicos
        public T GetValue<T>(string key, T defaultValue = default)
        {
            return runtimeMemory.TryGetValue(key, out var value) ? (T)value : defaultValue;
        }

        public void SetValue<T>(string key, T value)
        {
            runtimeMemory[key] = value;
        }
    }
}