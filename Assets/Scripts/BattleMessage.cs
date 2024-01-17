using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMessage : MonoBehaviour
{
    private static BattleMessage _instance;
    private bool displayingMessage = false; // 메시지가 현재 표시 중인지 여부를 나타내는 플래그
    private Queue<string> messageQueue = new Queue<string>(); // 메시지 큐
    public GameObject _dialog;
    public Text _dialogMsg;

    public static BattleMessage Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _dialog.SetActive(false);
        _instance = this;
    }

    public void Open(string text)
    {
        messageQueue.Enqueue(text); // 메시지 큐에 메시지 추가

        if (!displayingMessage)
        {
            StartCoroutine(DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        displayingMessage = true;

        while (messageQueue.Count > 0)
        {
            string currentMessage = messageQueue.Dequeue(); // 큐에서 메시지 가져오기
            _dialog.SetActive(true);
            _dialogMsg.text = "";

            foreach (char letter in currentMessage)
            {
                _dialogMsg.text += letter;
                // dialog 메시지의 출력 속도
                yield return new WaitForSeconds(.02f);
            }
        }

        displayingMessage = false;
    }

    public bool IsDisplayingMessage()
    {
        return displayingMessage;
    }
}
