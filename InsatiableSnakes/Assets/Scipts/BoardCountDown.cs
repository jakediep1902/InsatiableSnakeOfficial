using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class BoardCountDown : MonoBehaviour
{
    public static UnityEvent eventCountDone = new UnityEvent();
    public TextMeshProUGUI txtCountDown;
    public int duration = 2;
    IEnumerator countDown(int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            txtCountDown.text = (duration-i).ToString();
            yield return new WaitForSeconds(1);
            if(i==duration-1)
            {
                eventCountDone?.Invoke();
                this.gameObject.SetActive(false);
            }
        }     
    }
    private void OnEnable()
    {
        StartCoroutine(countDown(duration));
    }
}
