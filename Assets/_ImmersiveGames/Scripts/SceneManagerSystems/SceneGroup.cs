namespace _ImmersiveGames.Scripts.SceneManagerSystems {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eflatun.SceneReference;

    namespace Systems.SceneManagement {
        [Serializable]
        public class SceneGroup {
            public string GroupName = "New Scene Group";
            public List<SceneData> Scenes;
        
            public string FindSceneNameByType(SceneType sceneType) {
                return Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Reference.Name;
            }
        }
    
        [Serializable]
        public class SceneData {
            public SceneReference Reference;
            public string Name => Reference.Name;
            public SceneType SceneType;
        }
    
        //Adicionar as cenas usadas para referencia
        public enum SceneType { ActiveScene, MainMenu, UserInterface, HUD, Cinematic, Environment, Tooling, TESTES }
    }
}