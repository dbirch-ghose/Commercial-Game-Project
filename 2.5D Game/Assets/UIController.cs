using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static bool hosting = true;
    public static string roomCode;
    public GameObject playButton;
    public GameObject RoomCodeTitle;
    public GameObject RoomCodeInputBox;
    public GameObject HostButton;
    public GameObject JoinButton;
    public GameObject test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void playClick()
    {
        playButton.SetActive(false);
        RoomCodeTitle.SetActive(true);
        RoomCodeInputBox.SetActive(true);
        HostButton.SetActive(true);
        JoinButton.SetActive(true);
    }

    public void hostClick()
    {
        hosting = true;
        roomCode = RoomCodeInputBox.GetComponent<TMP_InputField>().text;
        SceneManager.LoadScene("MainLevelDraft2");
    }
    public void joinClick()
    {
        hosting=false;
        roomCode = RoomCodeInputBox.GetComponent<TMP_InputField>().text;
        SceneManager.LoadScene("MainLevelDraft2");
    }

}
