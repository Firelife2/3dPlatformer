using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardLine : MonoBehaviour
{
    [SerializeField] TMP_Text nameText, scoreText, deathsText;

    public void Init(string name, int score, int deaths)
    {
        nameText.text = name;
        scoreText.text = score.ToString();
        deathsText.text = deaths.ToString();
    }

    public void UpdateData(PlayerNetworked.E_Stats key, int value)
    {
        switch (key)
        {
            case PlayerNetworked.E_Stats.Score:
                scoreText.text = value.ToString();
                break;
            case PlayerNetworked.E_Stats.Death:
                deathsText.text = value.ToString();
                break;
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    
}
