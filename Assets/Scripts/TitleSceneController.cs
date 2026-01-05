using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private GameObject ErrorMessageObject;
    [SerializeField] private TextMeshProUGUI bestscoremessage;
    private void Start()
    {
        ErrorMessageObject.SetActive(false);

        var username = MainManager.instance.BestplayerName;
        var score = MainManager.instance.BestScore;

        if (string.IsNullOrEmpty(username) && score == 0)
        {
            bestscoremessage.text = "^^ hello";
        }
        else
        {
            bestscoremessage.text = $"{username} | {score}";
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.K)) { MainManager.instance.ResetData(); Debug.Log("ResetData!"); }
    }
    public void StartButtonClicked()
    {
        if (IsValidUserName(usernameInput.text))
        {
            MainManager.instance.playerName = usernameInput.text;
            SceneManager.LoadScene("main");
            return;
        }
        ErrorMessageObject.SetActive(true);
        StopAllCoroutines(); //˜A‘Å‘Îˆ
        StartCoroutine(MessageCount());
    }
    public void QuitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public bool IsValidUserName(string name) => !string.IsNullOrWhiteSpace(name);
    IEnumerator MessageCount()
    {
        yield return new WaitForSeconds(3);
        ErrorMessageObject.SetActive(false);
    }
}
