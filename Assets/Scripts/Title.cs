using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private AudioClip gameStartSound;
    [SerializeField] private AudioClip selectSound;

    private static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        /*if(Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(gameStartSound, 0.8f);
            SceneManager.LoadScene("Scenes/Game");
        }*/
    }

    public void OnGameStartButtonPressed()
    {
        audioSource.PlayOneShot(gameStartSound, 0.8f);
        Invoke(nameof(GameStart), 0.2f);
    }

    public void OnHowToButtonPressed()
    {
        audioSource.PlayOneShot(selectSound, 0.4f);
        Invoke(nameof(ViewHowTo), 0.2f);
    }

    public void OnCreditsButtonPressed()
    {
        audioSource.PlayOneShot(selectSound, 0.4f);
        Invoke(nameof(ViewCredits), 0.2f);
    }

    private void GameStart()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    private void ViewHowTo()
    {
        SceneManager.LoadScene("Scenes/HowTo");
    }

    private void ViewCredits()
    {
        SceneManager.LoadScene("Scenes/Credits");
    }
}
