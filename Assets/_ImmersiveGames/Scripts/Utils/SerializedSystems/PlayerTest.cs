using System;
using _ImmersiveGames.Scripts.Utils.Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ImmersiveGames.Scripts.Utils.SerializedSystems {
    public class PlayerTest : MonoBehaviour, IBind<PlayerData> {
        [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        [SerializeField] private PlayerData playerData;
        public void Bind(PlayerData data) {
            playerData = data;
            playerData.Id = Id;
            transform.position = playerData.playerPosition;
            transform.rotation = playerData.playerRotation;
        }

        private void Update() {
            playerData.playerPosition = transform.position;
            playerData.playerRotation = transform.rotation;
        }
    }
}