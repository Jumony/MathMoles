using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class Hole : MonoBehaviour
    {
        public Image healthBarImage;
        public Vector3 spawnOffset;

        [Tooltip("Possible sprites the creature can have")]
        public List<GameObject> possibleCreatures;

        public bool IsOccupied => _currentCreature != null;

        private GameObject _currentCreature;
        private float _currentLifetime;
        private float _timeRemaining;
        private bool _isCountingDown;
        private Coroutine _currentSpawnCoroutine; // ADD THIS

        private void Update()
        {
            if (!_isCountingDown || healthBarImage is null) return;
            _timeRemaining -= Time.deltaTime;
            healthBarImage.fillAmount = Mathf.Clamp01(_timeRemaining / _currentLifetime);

            healthBarImage.color = healthBarImage.fillAmount switch
            {
                < 0.3f => Color.red,
                < 0.6f => Color.yellow,
                _ => Color.green
            };
        }

        public void SpawnCreature(EquationData equationData, float creatureLifetime)
        {
            // Stop any existing coroutine for this hole
            if (_currentSpawnCoroutine != null)
            {
                StopCoroutine(_currentSpawnCoroutine);
            }

            _currentSpawnCoroutine = StartCoroutine(SpawnAndHandleLifetime(equationData, creatureLifetime));
        }

        private IEnumerator SpawnAndHandleLifetime(EquationData equationData, float creatureLifetime)
        {
            if (possibleCreatures.Count == 0)
            {
                Debug.LogError("possibleCreatures list is empty. Returning");
                yield break;
            }
            if (_currentCreature != null)
                yield break;

            _currentLifetime = creatureLifetime;
            _timeRemaining = creatureLifetime;
            _isCountingDown = true;

            var randomCreature = possibleCreatures[Random.Range(0, possibleCreatures.Count)];
            _currentCreature = Instantiate(randomCreature, transform.position + spawnOffset, Quaternion.identity, transform);

            var creatureScript = _currentCreature.GetComponent<Creature>();
            creatureScript.equationData = equationData;
            creatureScript.solution = equationData.solution;
            creatureScript.parentHole = this;

            var gameController = FindAnyObjectByType<GameController>();
            if (gameController != null)
                gameController.activeCreatures.Add(creatureScript);

            AttachEquationText(_currentCreature, equationData.question);

            if (healthBarImage != null)
            {
                healthBarImage.gameObject.SetActive(true);
                healthBarImage.fillAmount = 1f;
                healthBarImage.color = Color.green;
            }

            // Wait for lifetime to expire
            while (_timeRemaining > 0 && _currentCreature != null) // ADD check for currentCreature
            {
                yield return null;
            }

            // Clean up - only if creature still exists
            _isCountingDown = false;
            if (_currentCreature != null)
            {
                Destroy(_currentCreature);
                _currentCreature = null;
            }

            if (healthBarImage != null)
                healthBarImage.gameObject.SetActive(false);

            _currentSpawnCoroutine = null; // ADD THIS
        }

        private static void AttachEquationText(GameObject creature, string text)
        {
            var textObj = new GameObject("EquationText");
            textObj.transform.SetParent(creature.transform);
            textObj.transform.localPosition = new Vector3(0, 1.50f, 0);

            TMP_Text tmp = textObj.AddComponent<TextMeshPro>();
            tmp.text = text;
            tmp.fontSize = 3;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;

            var meshRenderer = tmp.GetComponent<MeshRenderer>();
            meshRenderer.sortingOrder = 1;
        }

        public void OnCreatureDestroyed()
        {
            _isCountingDown = false;
            _currentCreature = null;

            if (healthBarImage != null)
                healthBarImage.gameObject.SetActive(false);

            // Stop the coroutine when creature is destroyed early
            if (_currentSpawnCoroutine == null) return;
            StopCoroutine(_currentSpawnCoroutine);
            _currentSpawnCoroutine = null;
        }
    }
}