using UnityEngine;

namespace Enemy
{
    public class Equation
    {
        public int Operand1 { get; }
        public int Operand2 { get; }
        public char Operation { get; }

        public Equation(int operand1, int operand2, char operation)
        {
            Operand1 = operand1;
            Operand2 = operand2;
            Operation = operation;
        }

        public int Evaluate()
        {
            switch (Operation)
            {
                case '+':
                    return Operand1 + Operand2;
                case '-':
                    return Operand1 - Operand2;
                case '*':
                    return Operand1 * Operand2;
                case '/':
                    return Operand1 / Operand2;
                default:
                    Debug.LogError("Invalid Operation");
                    return 0; // this is my comment
            }
        }
    }
}
