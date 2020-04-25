using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AndreTheBoss.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void NewGame()
        {
            Debug.Log("NewGame was called");
        }

        public void LoadGame()
        {
            Debug.Log("LoadGame was called");
        }

        public void Settings()
        {
            Debug.Log("Settings was called");
        }

        public void Credits()
        {
            Debug.Log("Credits was called");
        }

        public void Quit()
        {
            Debug.Log("Quitting game");
            Application.Quit();
        }
    }

}
