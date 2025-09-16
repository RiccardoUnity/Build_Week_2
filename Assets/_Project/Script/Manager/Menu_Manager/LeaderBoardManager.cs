using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGM;
using TMPro;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _leaderboardTexts;

    private void Start()
    {
        RefreshLeaderboardUI();
    }

    public void RefreshLeaderboardUI()
    {
        Leaderboard leaderboard = S_SaveManager.GetLeaderboard();

        for (int i = 0; i < _leaderboardTexts.Length; i++)
        {
            int score = leaderboard.GetPosition(i);
            if (score > 0)
            {
                _leaderboardTexts[i].text = (i + 1).ToString() + ". " + score.ToString() + " punti";
            }
            else
            {
                _leaderboardTexts[i].text = (i + 1).ToString() + ". ---";
            }
        }
    }
    public void ResetLeaderboard()
    {
        S_SaveManager.ResetLeaderboard();
        RefreshLeaderboardUI();
    }
}
