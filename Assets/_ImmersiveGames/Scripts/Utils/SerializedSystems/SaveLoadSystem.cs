using System;
using System.Collections.Generic;
using System.Linq;
using _ImmersiveGames.Scripts.Utils.Helpers;
using _ImmersiveGames.Scripts.Utils.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _ImmersiveGames.Scripts.Utils.SerializedSystems {
    
    /// <summary>
    /// Representa os dados salvos do jogo, incluindo nome e progresso do jogador.
    /// </summary>
    [Serializable]
    public class GameData { 
        /// <summary>
        /// Nome do jogo salvo.
        /// </summary>
        public string name;

        /// <summary>
        /// Nome da fase atual salva.
        /// </summary>
        public string currentLevelName;

        /// <summary>
        /// Dados do jogador.
        /// </summary>
        public PlayerData playerData;

        //public InventoryData inventoryData;
    }

    #region For test only

    /// <summary>
    /// Representa os dados do jogador que podem ser salvos.
    /// </summary>
    [Serializable]
    public class PlayerData : ISaveable {
        /// <summary>
        /// Identificador único do jogador.
        /// </summary>
        [field: SerializeField] public SerializableGuid Id { get; set; }

        /// <summary>
        /// Posição do jogador na cena.
        /// </summary>
        public Vector3 playerPosition;

        /// <summary>
        /// Rotação do jogador na cena.
        /// </summary>
        public Quaternion playerRotation;
    }

    #endregion
        
    /// <summary>
    /// Interface para objetos que podem ser salvos.
    /// </summary>
    public interface ISaveable  {
        /// <summary>
        /// Identificador único do objeto salvo.
        /// </summary>
        SerializableGuid Id { get; set; }
    }
   
    /// <summary>
    /// Interface para vincular dados salvos a objetos na cena.
    /// </summary>
    /// <typeparam name="TData">Tipo dos dados salvos.</typeparam>
    public interface IBind<in TData> where TData : ISaveable {
        /// <summary>
        /// Identificador único do objeto.
        /// </summary>
        SerializableGuid Id { get; set; }

        /// <summary>
        /// Associa os dados salvos a um objeto na cena.
        /// </summary>
        /// <param name="data">Dados a serem vinculados.</param>
        void Bind(TData data);
    }
    
    /// <summary>
    /// Sistema de salvamento e carregamento do jogo.
    /// </summary>
    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem> {
        /// <summary>
        /// Dados do jogo atual.
        /// </summary>
        [SerializeField] public GameData gameData;

        /// <summary>
        /// Serviço de armazenamento de dados (exemplo: JSON em arquivos).
        /// </summary>
        IDataService dataService;

        /// <summary>
        /// Inicializa o sistema de salvamento e define o serviço de dados.
        /// </summary>
        protected override void Awake() {
            base.Awake();
            dataService = new FileDataService(new JsonSerializer());
        }
        
        /// <summary>
        /// Registra eventos de carregamento de cena.
        /// </summary>
        void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

        /// <summary>
        /// Remove eventos de carregamento de cena ao desativar.
        /// </summary>
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
        
        /// <summary>
        /// Executado quando uma nova cena é carregada, vinculando dados aos objetos da cena.
        /// </summary>
        /// <param name="scene">Cena carregada.</param>
        /// <param name="mode">Modo de carregamento da cena.</param>
        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name == "Menu") return;
            
            Bind<PlayerTest, PlayerData>(gameData.playerData);
            //Bind<Inventory.Inventory, InventoryData>(gameData.inventoryData);
        }
        
        /// <summary>
        /// Vincula um objeto salvo a uma entidade correspondente na cena.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto na cena.</typeparam>
        /// <typeparam name="TData">Tipo dos dados salvos.</typeparam>
        /// <param name="data">Dados a serem vinculados.</param>
        private void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new() {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null) {
                if (data == null) {
                    data = new TData { Id = entity.Id };
                }
                entity.Bind(data);
            }
        }
        
        /// <summary>
        /// Vincula uma lista de objetos salvos às entidades correspondentes na cena.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto na cena.</typeparam>
        /// <typeparam name="TData">Tipo dos dados salvos.</typeparam>
        /// <param name="datas">Lista de dados a serem vinculados.</param>
        private void Bind<T, TData>(List<TData> datas) where T: MonoBehaviour, IBind<TData> where TData : ISaveable, new() {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach(var entity in entities) {
                var data = datas.FirstOrDefault(d=> d.Id == entity.Id);
                if (data == null) {
                    data = new TData { Id = entity.Id };
                    datas.Add(data); 
                }
                entity.Bind(data);
            }
        }

        /// <summary>
        /// Cria um novo jogo e carrega a cena inicial.
        /// </summary>
        public void NewGame() {
            gameData = new GameData {
                name = "Game",
                currentLevelName = "TESTES"
            };
            SceneManager.LoadScene(gameData.currentLevelName);
        }
        
        /// <summary>
        /// Salva o estado atual do jogo.
        /// </summary>
        public void SaveGame() => dataService.Save(gameData);

        /// <summary>
        /// Carrega um jogo salvo e muda para a cena correspondente.
        /// </summary>
        /// <param name="gameName">Nome do jogo salvo a ser carregado.</param>
        public void LoadGame(string gameName) {
            gameData = dataService.Load(gameName);

            if (string.IsNullOrWhiteSpace(gameData.currentLevelName)) {
                gameData.currentLevelName = "Demo";
            }

            SceneManager.LoadScene(gameData.currentLevelName);
        }
        
        /// <summary>
        /// Recarrega o jogo atual a partir dos dados salvos.
        /// </summary>
        public void ReloadGame() => LoadGame(gameData.name);

        /// <summary>
        /// Exclui um jogo salvo.
        /// </summary>
        /// <param name="gameName">Nome do jogo salvo a ser excluído.</param>
        public void DeleteGame(string gameName) => dataService.Delete(gameName);
    }
}
