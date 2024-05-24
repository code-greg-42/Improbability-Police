using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField] private Button backstoryButton;

    void Start()
    {
        backstoryButton.onClick.AddListener(LoadBackstory);
    }

    private void LoadBackstory()
    {
        SceneManager.LoadScene(1);
    }
}
