using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterGUI : MonoBehaviour
{
    [SerializeField] GameObject leaderBoardPanel;
    [SerializeField] GameObject interactText;
    [SerializeField] CharacterInput input;
    [SerializeField] CharacterInteractions interactions;

    private void Update()
    {
        leaderBoardPanel.SetActive(input.ShowLeaderboard);
        interactText.SetActive(interactions.HasInteractions);
    }
}
