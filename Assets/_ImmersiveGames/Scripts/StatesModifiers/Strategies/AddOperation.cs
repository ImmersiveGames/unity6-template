using _ImmersiveGames.Scripts.StatesModifiers.Interfaces;

namespace _ImmersiveGames.Scripts.StatesModifiers.Strategies {
    public class AddOperation : IOperationStrategy {
        private readonly int operationValue;

        public AddOperation(int value) {
            operationValue = value;
        }
        public int Calculate(int value) => operationValue + value;
    }
}