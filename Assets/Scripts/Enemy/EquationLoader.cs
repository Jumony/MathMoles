using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [System.Serializable]
    public class EquationData
    {
        public string question;
        public int solution;

        public EquationData(string question, int solution)
        {
            this.question = question;
            this.solution = solution;
        }
    }

    public class EquationLoader : MonoBehaviour
    {
        public static EquationLoader Instance { get; private set; }

        [FormerlySerializedAs("equationCSV")] public TextAsset equationCsv; // Assign your CSV file here in Inspector
        private readonly List<EquationData> _allEquations = new();
        private List<EquationData> _unusedEquations = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadEquations();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadEquations()
        {
            if (equationCsv == null)
            {
                Debug.LogError("No CSV file assigned to EquationLoader!");
                return;
            }

            var lines = equationCsv.text.Split('\n');

            // Skip header row (starts at index 1)
            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (string.IsNullOrEmpty(line))
                    continue;

                var data = line.Split(',');

                if (data.Length < 2) continue;
                var question = data[0].Trim();

                if (int.TryParse(data[1].Trim(), out var solution))
                {
                    _allEquations.Add(new EquationData(question, solution));
                }
                else
                {
                    Debug.LogWarning($"Could not parse solution for: {question}");
                }
            }

            // Initialize unused equations list
            _unusedEquations = new List<EquationData>(_allEquations);

            Debug.Log($"Loaded {_allEquations.Count} equations from CSV");
        }

        public EquationData GetRandomEquation()
        {
            // If we've used all equations, refill the pool
            if (_unusedEquations.Count == 0)
            {
                _unusedEquations = new List<EquationData>(_allEquations);
                Debug.Log("Refilled equation pool - all equations used once");
            }

            // Get random equation from unused pool
            var randomIndex = Random.Range(0, _unusedEquations.Count);
            var selectedEquation = _unusedEquations[randomIndex];

            // Remove from unused pool so it won't repeat until all are used
            _unusedEquations.RemoveAt(randomIndex);

            return selectedEquation;
        }
    }
}