using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class Title : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button rankingButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            startButton.onClick.AddListener(()=>SceneManager.LoadScene(1));
            rankingButton.onClick.AddListener(()=>SceneManager.LoadScene(4));
            exitButton.onClick.AddListener(()=>Application.Quit());
        }
    }
}
