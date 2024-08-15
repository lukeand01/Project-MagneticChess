using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Round : MonoBehaviour
{
    //this serves only to show the progress of the timer. the actual timer is counted in the singleplayerhandler
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Transform roundHolder;
    [SerializeField] Transform presentationPosition;

    public void StartRound()
    {
        //it shouild come from a region and then come down.
        roundHolder.DOMove(presentationPosition.position, 0.5f).SetEase(Ease.Linear);

    }

    public void UpdateTimerText(TimeClass currentTimer)
    {
        string minute = "0" + currentTimer.minute.ToString();
        string second = "";
        

        if(currentTimer.second > 9)
        {
            second = currentTimer.second.ToString();
        }
        else
        {
            second = "0" + currentTimer.second.ToString();
        }


        timerText.text = minute + " : " + second;
    }

}

public class TimeClass
{
    public int minute {  get; private set; }
    public int second { get; private set; }

    public TimeClass(int minute, int second   )
    {
        this.minute = minute;
        this.second = second;
    }

    public void CountTimerDown()
    {
        second -= 1;
        if(second <= 0 && minute > 0)
        {
            minute -= 1;
            second = 60;
        }

    }
    public bool IsTimerZero() => minute == 0 && second == 0;

}