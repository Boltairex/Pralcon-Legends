using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    public void NextScene() => SceneManager.LoadScene(1);
}
