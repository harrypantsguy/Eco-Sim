using UnityEngine;

namespace _Project.Codebase
{
    public class Manager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Managers.AddManager(this);
        }
    }
}