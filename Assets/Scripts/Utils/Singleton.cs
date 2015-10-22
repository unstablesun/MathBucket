using System;
using UnityEngine;
using Trace = System.Diagnostics.Trace;

namespace FUEL.Utils
{
    /// <summary>
    /// A thread-safe singleton implementation for MonoBehaviors.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : class
    {
        private static volatile object lockSync = new object();
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockSync)
                    {
                        if (instance == null)
                        {
                            Trace.TraceInformation(string.Format("Creating Singleton {0}", typeof(T).Name));
                            instance = SingletonManager.GetSingletonContainer(typeof(T).Namespace).AddComponent(typeof(T)) as T;
                        }
                    }
                }

                return instance;
            }
        }

        public static void CreateInstance()
        {
            if (instance == null)
            {
                instance = Instance;
            }
        }
    }

    /// <summary>
    /// Maintains a copy of the game object holding singletons across scenes.
    /// </summary>
    internal class SingletonManager
    {
        private static GameObject gameObjectInstance = null;

        public static GameObject GetSingletonContainer(string gameObjectName)
        {
            if (gameObjectInstance == null)
            {
                if (GameObject.Find(gameObjectName) != null)
                {
                    throw new InvalidOperationException("There's an old SingletonManager in the scene. Delete and save scene!");
                }

                gameObjectInstance = new GameObject(gameObjectName);
                gameObjectInstance.name = "SingletonContainer";
                UnityEngine.Object.DontDestroyOnLoad(gameObjectInstance);
            }

            return gameObjectInstance;
        }
    }
}