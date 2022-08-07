using UnityEngine;

namespace FSM
{
    public abstract class StateBehaviour : MonoBehaviour
    {
        public abstract void OnEnter();
        public abstract void OnExit();

        public abstract void OnUpdate(float deltaTime);
    }
}