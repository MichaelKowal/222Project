using UnityEngine;
using System.Collections;

namespace Completed
{
    public class LevelManager : Singleton<LevelManager>
    {
        public int levelToLoad = 1;
        public void LoadLevel1(string name)
        {
            levelToLoad = 1;

            Debug.Log("New Level load: " + name);

            Application.LoadLevel(name);
        }

        public void LoadLevel2(string name)
        {
            levelToLoad = 2;

            Debug.Log("New Level load: " + name);

            Application.LoadLevel(name);
        }

        public void LoadLevel3(string name)
        {
            levelToLoad = 3;

            Debug.Log("New Level load: " + name);

            Application.LoadLevel(name);
        }
        public void QuitRequest()
        {
            Debug.Log("Quit requested");
            Application.Quit();
        }

    }
}
