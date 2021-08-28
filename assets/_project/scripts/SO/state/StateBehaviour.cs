using UnityEngine.Events;
namespace so
{
    [System.Serializable]
    public struct StateBehaviour
    {
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
    }
}