namespace _ImmersiveGames.Scripts.StatesModifiers {
    public class MultiplyOperation : IOperationStrategy {
        private readonly int operationValue;

        public MultiplyOperation(int value) {
            operationValue = value;
        }
        public int Calculate(int value) => operationValue * value;
    }
}