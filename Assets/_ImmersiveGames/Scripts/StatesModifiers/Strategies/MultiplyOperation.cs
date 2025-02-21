using _ImmersiveGames.Scripts.StatesModifiers.Interfaces;

namespace _ImmersiveGames.Scripts.StatesModifiers.Strategies {
    public class MultiplyOperation : IOperationStrategy {
        private readonly int operationValue;

        public MultiplyOperation(int value) {
            operationValue = value;
        }
        public int Calculate(int value) => operationValue * value;
    }
}