using System;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader
    {
        public int CurrentSceneIndex
        {
            get { return SceneManager.GetActiveScene().buildIndex; }
        }
        
        public void ReloadCurrentScene()
        {
            var index = CurrentSceneIndex;
            
            UnloadScene(index, () =>
            {
                LoadSceneAdditiveByIndex(index);
            });
        }

        public void UnloadScene(int index, Action callback = null)
        {
            var operation = SceneManager.UnloadSceneAsync(index);
            operation.completed += asyncOperation => { callback?.Invoke(); };
        }

        public void LoadSceneAdditiveByIndex(int index, Action callback = null)
        {
            var operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            operation.completed += asyncOperation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
                callback?.Invoke();
            };
        }
    }
}