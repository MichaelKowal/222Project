using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Completed
{
    public class LevelManager : Singleton<LevelManager>
    {
        public int levelToLoad = 3;
        public void LoadLevel1(string name)
        {
            levelToLoad = 1;

            Debug.Log("New Level load: " + name);

            Application.LoadLevel(name);

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }
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
