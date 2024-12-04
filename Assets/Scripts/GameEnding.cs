using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;
    public Text timerText; // Reference to the UI Text displaying the timer
    public float countdownTime = 60f; // Time in seconds for the countdown timer

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;

    void UpdateTimerUI()
    {
        // Update the timer text UI
        timerText.text = "Time Left: " + Mathf.Ceil(countdownTime).ToString() + "s";
    }
    void Start()
    {
        UpdateTimerUI(); // Initialize the timer UI at the start
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
        else
        {
            // Update the countdown timer
            if (countdownTime > 0)
            {
                countdownTime -= Time.deltaTime; // Decrease the timer
                UpdateTimerUI(); // Update the UI with the new time

                if (countdownTime <= 0)
                {
                    TimerExpired(); // Trigger game over if the timer reaches zero
                }
            }
        }
        void TimerExpired() //Recalling the caught effect that was made early on
        {
         m_IsPlayerCaught = true;
        }

        void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
        {
            if (!m_HasAudioPlayed)
            {
                audioSource.Play();
                m_HasAudioPlayed = true;
            }
            m_Timer += Time.deltaTime;
            imageCanvasGroup.alpha = m_Timer / fadeDuration;

            if (m_Timer > fadeDuration + displayImageDuration)
            {
                if (doRestart)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }
}
