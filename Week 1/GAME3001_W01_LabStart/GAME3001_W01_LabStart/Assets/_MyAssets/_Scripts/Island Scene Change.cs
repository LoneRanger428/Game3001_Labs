using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandSceneChange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AgentMovement>())
        {
            SceneManager.LoadScene(2);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
