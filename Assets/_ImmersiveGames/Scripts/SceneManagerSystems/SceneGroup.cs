using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;

namespace _ImmersiveGames.Scripts.SceneManagerSystems {
    namespace Systems.SceneManagement {
        [Serializable]
        public class SceneGroup {
            public string groupName = "New Scene Group";
            public List<SceneData> scenes;
        
            public string FindSceneNameByType(SceneType sceneType) {
                return scenes.FirstOrDefault(scene => scene.sceneType == sceneType)?.reference.Name;
            }
        }
    
        [Serializable]
        public class SceneData {
            public SceneReference reference;
            public string Name => reference.Name;
            public SceneType sceneType;
        }
    
        //Adicionar as cenas usadas para referencia
        public enum SceneType { ActiveScene, MainMenu, UserInterface, HUD, Cinematic, Environment, Tooling, Testes }
    }
}