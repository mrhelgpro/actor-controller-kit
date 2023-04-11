using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    private string _sceneName;
    private Button _button;

    private void Start()
    {
        _sceneName = gameObject.name;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadSceneByGameObjectName);
    }

    public void LoadSceneByGameObjectName() => SceneManager.LoadScene(_sceneName);
}
