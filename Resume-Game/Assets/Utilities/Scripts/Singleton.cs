namespace Utilities
{
    using UnityEngine;

    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //If the reference isn't filled, this method will look for the corresponding GameObject in the scene. 
                    instance = (T)FindObjectOfType(typeof(T));

                    //A new Gameobject will be instantiated if none could be found.
                    if (instance == null)
                    {
                        GameObject singletonGO = new GameObject();
                        instance = singletonGO.AddComponent<T>();
                        singletonGO.name = typeof(T).ToString() + " (Singleton)";
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = (T)this;
            }
        }
    }
}
