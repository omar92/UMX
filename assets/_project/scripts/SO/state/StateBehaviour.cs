using UnityEngine.Events;
namespace SO
{
    [System.Serializable]
    public struct StateBehaviour
    {
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
    }
}