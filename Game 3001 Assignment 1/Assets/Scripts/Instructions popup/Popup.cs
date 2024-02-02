using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Popup : MonoBehaviour
{
    public TMP_Text popupText;

    private GameObject window;
    private Animator popupAnimator;

    private Queue<string> popupQueue;
    private bool isActive;
    private Coroutine queueChecker;

    private void Start()
    {
        window = transform.GetChild(0).gameObject;
        popupAnimator = GetComponent<Animator>();
        window.SetActive(false);
        popupQueue = new Queue<string>();   
    }

    public void AddToQueue(string text)
    {
        popupQueue.Enqueue(text);
        if(queueChecker == null)
        {
            queueChecker = StartCoroutine(CheckQueue());
        }
    }
    private void showPopup(string text)
    {
        isActive = true;
        window.SetActive(true);
        popupText.text = text;
        popupAnimator.Play("Popup");
    }

    private IEnumerator CheckQueue()
    {
        do
        {
            showPopup(popupQueue.Dequeue());
            do
            {
                yield return null;
            } while (!popupAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        } while (popupQueue.Count > 0);
        isActive = false;
    }
}
