using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnlLost : MonoBehaviour
{
    [SerializeField] GameObject boardCountDown;
    private void OnEnable()
    {
        int temp = Random.Range(0,2);
        Debug.Log(temp);

        if (temp % 2 == 0)
        {
            boardCountDown.SetActive(true);
        }
        
    }
}
