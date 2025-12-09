using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class CreatureSpawnBehavior : MonoBehaviour
    {
        public List<Hole> holes;
        public float spawnIntervalMin;
        public float spawnIntervalMax;
        public float creatureLifetime = 2.0f;

        [FormerlySerializedAs("useCSVEquations")]
        [Header("Equation Source")]
        [Tooltip("If true, uses equations from CSV. If false, generates random equations.")]
        public bool useCsvEquations = true;

        private float _spawnInterval = 2.0f;

        private void Start()
        {
            StartCoroutine(SpawnCreatures());
        }

        private IEnumerator SpawnCreatures()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                var emptyHoles = holes.FindAll(h => !h.IsOccupied);
                if (emptyHoles.Count == 0)
                {
                    continue;
                }

                var randomHole = emptyHoles[Random.Range(0, emptyHoles.Count)];

                // Get equation from CSV or generate random 
                // Random is still here mostly for debugging and testing purposes
                if (useCsvEquations && EquationLoader.Instance != null)
                {
                    var equationData = EquationLoader.Instance.GetRandomEquation();
                    randomHole.SpawnCreature(equationData, creatureLifetime);
                }
                else
                {
                    // Fallback to random generation if csv not available
                    var equation = GenerateRandomEquation();

                    // Converts old Equation to EquationData format
                    var questionText = $"{equation.Operand1} {equation.Operation} {equation.Operand2}";
                    var equationData = new EquationData(questionText, equation.Evaluate());
                    randomHole.SpawnCreature(equationData, creatureLifetime);
                }

                _spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            }
        }

        private static Equation GenerateRandomEquation()
        {
            char[] operations = { '+', '-', '*', '/' };
            var operation = operations[Random.Range(0, operations.Length)];
            int operand1 = Random.Range(0, 10), operand2;

            if (operation == '/')
            {
                operand2 = Random.Range(1, 10);
                if (operand1 % operand2 != 0)
                {
                    operand1 += operand2 - operand1 % operand2;
                }
            }
            else
            {
                operand2 = Random.Range(0, 10);
            }

            return new Equation(operand1, operand2, operation);
        }
    }
}