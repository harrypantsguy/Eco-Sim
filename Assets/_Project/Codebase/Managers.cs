using System;
using System.Collections.Generic;

namespace _Project.Codebase
{
    public static class Managers
    {
        private static Dictionary<Type, Manager> _managerDict = new Dictionary<Type, Manager>();
        
        public static bool TryGetManager<T>(out T foundManager) where T : Manager
        {
            bool found = _managerDict.TryGetValue(typeof(T), out Manager manager);

            if (manager is T castedManager)
                foundManager = castedManager;
            else
                foundManager = null;
            
            return found;
        }
        public static void AddManager(Manager manager)
        {
           _managerDict.Add(manager.GetType(), manager);
        }
    }
}