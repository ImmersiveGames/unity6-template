using System.Collections.Generic;
using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;
using Sirenix.OdinInspector;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "ImmersiveGames/Behavior/Tree (Odin)")]
    public class BehaviorTreeSo : ScriptableObject
    {
        [SerializeField, InlineEditor(InlineEditorModes.GUIAndHeader)]
        private BlackboardSo blackboard;

        [SerializeField, FoldoutGroup("Root Nodes"),
         ListDrawerSettings(ShowIndexLabels = true, ShowItemCount = true, NumberOfItemsPerPage = 5)]
        public List<NodeConfig> rootNodes = new List<NodeConfig>();

        [SerializeField, FoldoutGroup("Factory"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        private ScriptableObject nodeFactoryAsset; // Campo configurável no Inspector

        private INodeFactory nodeFactory; // Instância usada em runtime

        private void OnEnable()
        {
            // Define o factory padrão como DefaultNodeFactory se nenhum for configurado
            if (nodeFactoryAsset == null || !(nodeFactoryAsset is INodeFactory))
            {
                nodeFactory = CreateInstance<DefaultNodeFactorySo>();
            }
            else
            {
                nodeFactory = nodeFactoryAsset as INodeFactory;
            }
        }

        public IBehaviorNode Build(BlackboardSo runtimeBlackboard = null)
        {
            var effectiveBlackboard = runtimeBlackboard ?? blackboard;
            var sequence = new SequenceNode();

            if (rootNodes == null || rootNodes.Count == 0)
            {
                DebugManager.LogWarning<BehaviorTreeSo>("Nenhum nó raiz configurado no BehaviorTreeSo");
                return sequence;
            }

            foreach (var nodeConfig in rootNodes)
            {
                if (nodeConfig == null)
                {
                    DebugManager.LogWarning<BehaviorTreeSo>("Configuração de nó raiz nula encontrada no BehaviorTreeSo");
                    continue;
                }

                var builtNode = nodeConfig.Build(effectiveBlackboard, nodeFactory);
                if (builtNode == null)
                {
                    DebugManager.LogWarning<BehaviorTreeSo>($"Falha ao construir nó: {nodeConfig.nodeType}");
                }
                else
                {
                    sequence.AddChild(builtNode);
                }
            }

            return sequence;
        }

        // Método para trocar o factory em runtime, se necessário
        public void SetNodeFactory(INodeFactory factory)
        {
            if (factory == null)
            {
                DebugManager.LogError<BehaviorTreeSo>("Tentativa de definir um NodeFactory nulo no BehaviorTreeSo");
                return;
            }
            nodeFactory = factory;
        }

        public BlackboardSo GetBlackboard()
        {
            return blackboard;
        }

        // Método auxiliar para criar um factory padrão no Inspector (opcional)
        [Button("Reset Factory to Default"), FoldoutGroup("Factory")]
        private void ResetFactoryToDefault()
        {
            nodeFactoryAsset = null; // Reseta pra usar o DefaultNodeFactory automaticamente
            OnEnable(); // Reaplica a lógica de inicialização
        }
    }
}