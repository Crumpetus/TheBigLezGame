using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    [SerializeField]
    Text[] m_timerText;
    [SerializeField]
    Image m_evacArrow;
    [SerializeField]
    Animator m_evacAnim;
    [SerializeField]
    AudioClip m_evacSound;
    [SerializeField]
    GameObject m_helingchopter;
    bool m_paused;
    public float m_maxTime;
    float m_currentTime;
	
    // Update is called once per frame
    void Update()
    {
        if (!m_paused)
        {
            m_currentTime += Time.deltaTime;

            foreach (Text t in m_timerText)
            {
                float timeRemaining = (m_maxTime - m_currentTime);
                t.text = "Time remaining: " + (int)(timeRemaining / 60) + ":" + (timeRemaining % 60).ToString("00.00").Replace('.', ':');
            }
            if (m_currentTime > m_maxTime)
            {
                TimeUp();
                m_currentTime = m_maxTime;
                m_paused = true;
            }
        }
        else if (helicopter.instance)
        {
            m_evacArrow.gameObject.SetActive(Mathf.Abs(Vector3.Distance(helicopter.instance.transform.position, Player.instance.transform.position)) > 10);
            m_evacArrow.transform.LookAt(helicopter.instance.transform.position);
        }
    }

    void TimeUp()
    {
        m_evacArrow.gameObject.SetActive(true);
        SoundManager.instance.playSound(m_evacSound, 1, 1);
        m_evacAnim.Play("evac_text");

        GameObject heli = Instantiate(m_helingchopter,
                              Player.instance.transform.position + (Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.left * 20),
                              Quaternion.Euler(0, 0, 0));
        foreach (Text t in m_timerText)
        {
            t.text = "";
        }
    }
}
