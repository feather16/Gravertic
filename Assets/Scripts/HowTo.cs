using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hypertext;

public class HowTo : MonoBehaviour
{
    private static readonly string REGEX_URL = @"https?://(?:[!-~]+\.)+[!-~]+";
    [SerializeField] RegexHypertext howtoText = default;

    [SerializeField] private AudioClip backSound;

    private static AudioSource audioSource;

    private static void OpenURL(string url)
    {
#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
        Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
        Application.OpenURL(url);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        TextAsset creditsFile = Resources.Load("Data/howto") as TextAsset;
        string creditsTextStr = creditsFile.text;
        howtoText.text = creditsTextStr;
        var lines = creditsTextStr.Split('\n').Length;
        howtoText.fontSize = 180 / (lines + 1);
        howtoText.OnClick(
            REGEX_URL, 
            new Color32(156, 194, 203, 255), 
            url =>
        {
            OpenURL(url);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OnBackButtonPressed();
        }
    }

    public void OnBackButtonPressed()
    {
        audioSource.PlayOneShot(backSound, 0.6f);
        Invoke(nameof(BackToTitle), 0.2f);
    }

    private void BackToTitle()
    {
        SceneManager.LoadScene("Scenes/Title");
    }
}
