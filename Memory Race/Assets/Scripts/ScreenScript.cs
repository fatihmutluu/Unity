using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenScripts
{
    public class ScreenScript : MonoBehaviour
    {
        private static GameObject instance;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (instance == null)
                instance = gameObject;
            else
                Destroy(gameObject);
        }
    }
}
