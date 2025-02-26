namespace _ImmersiveGames.Scripts.StatesModifiers {
    public class AddOperation : IOperationStrategy {
        private readonly int operationValue;

        public AddOperation(int value) {
            operationValue = value;
        }
        public int Calculate(int value) => operationValue + value;
    }
}