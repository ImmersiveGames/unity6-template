using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.MediatorSystems.Tests {
    public abstract class PayLoad<TData> : IVisitor {
        public abstract TData Content { get; set; }
        public abstract void Visit<T>(T visitable) where T : Component, IVisitable;
    }

    public class MessagePayload : PayLoad<string> {
        public Agent Source { get; set; }
        public override string Content { get; set; }
        
        private MessagePayload() {}
        public override void Visit<T>(T visitable) {
            Debug.Log($"{visitable.name} received message from {Source.name}: {Content}");
        }
        
        public class Builder {
            private readonly MessagePayload _payload = new MessagePayload();

            public Builder(Agent source)=> _payload.Source = source;
            
            public Builder WithContent(string content) { 
                _payload.Content = content;
                return this;
            }
            public MessagePayload Build() => _payload;
        }
    }
}