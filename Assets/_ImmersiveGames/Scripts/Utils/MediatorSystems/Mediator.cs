using _ImmersiveGames.Scripts.Utils.Extensions;

namespace _ImmersiveGames.Scripts.Utils.MediatorSystems {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Implementação do padrão Mediator, permitindo comunicação entre múltiplos objetos do tipo T.
    /// T deve ser um Component e implementar IVisitable.
    /// </summary>
    /// <typeparam name="T">Tipo de componente que será mediado.</typeparam>
    public abstract class Mediator<T> : MonoBehaviour where T : Component, IVisitable {
        /// <summary>
        /// Lista de entidades registradas no Mediator.
        /// </summary>
        private readonly List<T> entities = new List<T>();
    
        /// <summary>
        /// Registra uma entidade no Mediator para que possa receber mensagens.
        /// </summary>
        /// <param name="entity">Entidade a ser registrada.</param>
        public void Register(T entity) {
            if (entities.Contains(entity)) return;
            entities.Add(entity);
            OnRegistered(entity);
        }

        /// <summary>
        /// Método chamado quando uma entidade é registrada. Pode ser sobrescrito para lógica adicional.
        /// </summary>
        /// <param name="entity">Entidade que foi registrada.</param>
        protected virtual void OnRegistered(T entity) {
            // noop (Nenhuma operação padrão)
        }

        /// <summary>
        /// Remove uma entidade do Mediator, impedindo-a de receber mensagens.
        /// </summary>
        /// <param name="entity">Entidade a ser removida.</param>
        public void Deregister(T entity) {
            if (!entities.Contains(entity)) return;
            entities.Remove(entity);
            OnDeregistered(entity);
        }
    
        /// <summary>
        /// Método chamado quando uma entidade é removida. Pode ser sobrescrito para lógica adicional.
        /// </summary>
        /// <param name="entity">Entidade que foi removida.</param>
        protected virtual void OnDeregistered(T entity) {
            // noop (Nenhuma operação padrão)
        }

        /// <summary>
        /// Envia uma mensagem de uma entidade para outra específica.
        /// </summary>
        /// <param name="source">Entidade que envia a mensagem.</param>
        /// <param name="target">Entidade que deve receber a mensagem.</param>
        /// <param name="message">Mensagem a ser enviada.</param>
        public void Message(T source, T target, IVisitor message) {
            entities.FirstOrDefault(entity => entity.Equals(target))?.Accept(message);
        }

        /// <summary>
        /// Envia uma mensagem para todas as entidades registradas que atendem a um critério opcional.
        /// </summary>
        /// <param name="source">Entidade que envia a mensagem.</param>
        /// <param name="message">Mensagem a ser enviada.</param>
        /// <param name="predicate">Critério opcional para definir quem recebe a mensagem.</param>
        public void Broadcast(T source, IVisitor message, Func<T, bool> predicate = null) {
            entities.Where(target => source != target && SenderConditionMet(target, predicate) && MediatorConditionMet(target))
                .ForEach(target => target.Accept(message));
        }

        /// <summary>
        /// Verifica se uma entidade atende à condição definida pelo remetente.
        /// </summary>
        /// <param name="target">Entidade alvo.</param>
        /// <param name="predicate">Função que determina se a entidade deve receber a mensagem.</param>
        /// <returns>Retorna true se a condição for atendida ou se não houver um critério definido.</returns>
        private bool SenderConditionMet(T target, Func<T, bool> predicate) => predicate == null || predicate(target);

        /// <summary>
        /// Define se um determinado alvo pode receber mensagens do Mediator.
        /// Deve ser implementado na subclasse.
        /// </summary>
        /// <param name="target">Entidade alvo.</param>
        /// <returns>Retorna true se a entidade puder receber a mensagem.</returns>
        protected abstract bool MediatorConditionMet(T target);
    }

    /// <summary>
    /// Interface para visitantes que podem interagir com objetos do tipo IVisitable.
    /// </summary>
    public interface IVisitor {
        /// <summary>
        /// Método chamado ao visitar um objeto.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto visitável.</typeparam>
        /// <param name="visitable">Objeto a ser visitado.</param>
        void Visit<T>(T visitable) where T : Component, IVisitable;
    }

    /// <summary>
    /// Interface para objetos que podem ser visitados por um IVisitor.
    /// </summary>
    public interface IVisitable {
        /// <summary>
        /// Aceita um visitante e permite interação com ele.
        /// </summary>
        /// <param name="visitor">Visitante a ser aceito.</param>
        void Accept(IVisitor visitor);
    }
}
