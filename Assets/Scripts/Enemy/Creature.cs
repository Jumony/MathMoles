using UnityEngine;

namespace Enemy
{
    public class Creature : MonoBehaviour
    {
        public EquationData equationData;
        public float solution;
        public Hole parentHole;

        private void OnDestroy()
        {
            if (parentHole != null)
            {
                parentHole.OnCreatureDestroyed();
            }
        }
    }
}