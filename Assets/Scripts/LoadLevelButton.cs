using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadLevelButton : MonoBehaviour
{
    [SerializeField] string _sceneName;
    public void Load()
    { 
        SceneManager.LoadScene(_sceneName);
    }
}
