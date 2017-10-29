using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helicopter : MonoBehaviour
{
    public static helicopter instance;
    [SerializeField]
    Animator m_animator;
    Vector3 camStart;
    float lerpy = 0;

    void Start()
    {
        instance = this;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.EndGame);
            Player.instance.gameObject.SetActive(false);
            camStart = Camera.main.transform.position;
        }
    }

    void Update()
    {
        if (GameStateManager.instance.currentState == GameStateManager.GameState.EndGame && lerpy < 1)
        {
            lerpy += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(camStart, transform.position + (Vector3.back * 10), lerpy);
        }
        else if (lerpy > 1)
        {
            lerpy += Time.deltaTime * 10;
            transform.position += Vector3.up * Time.deltaTime * lerpy * 3;
            //end game screen
        }
    }
}
