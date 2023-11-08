using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_TImer : MonoBehaviour
{

    public float timeLeft = 14f;
    public TextMeshProUGUI timerText;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "Time Left: " + Mathf.Round(timeLeft).ToString();

        if (timeLeft <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
