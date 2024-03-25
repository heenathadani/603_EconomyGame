using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTime : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerVis;
    [SerializeField]
    private GameStateTracker tracker;

    // Update is called once per frame
    void Update()
    {
        float timeLeft = tracker.GetGameTime();
        timerVis.text = timeLeft.ToString("0.00");
        timerVis.color = Color.Lerp(Color.red, Color.green, timeLeft / 10f);
    }
}
