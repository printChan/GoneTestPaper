using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string seneName;

    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(seneName);
    }
}
