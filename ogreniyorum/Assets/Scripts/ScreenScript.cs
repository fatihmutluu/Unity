using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenScripts
{
    public class ScreenScript : MonoBehaviour
    {
        public void MainScreenClicked()
        {
            SceneManager.LoadScene("MainScreen");
        }

        public void NextLevelClicked()
        {
            Debug.Log("Next Level Clicked");
        }

        public void RestartClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void PlayClicked()
        {
            SceneManager.LoadScene("Level1");
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }
    }
}
