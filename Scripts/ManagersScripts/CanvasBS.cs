using TMPro;
using UnityEngine;

public class CanvasBS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] scores;

    private void Start()
    {
        scores[0].color = NetworkStartupBS.Instance.playerColors[0];
        scores[1].color = NetworkStartupBS.Instance.playerColors[1];
        scores[2].color = NetworkStartupBS.Instance.playerColors[2];
        scores[3].color = NetworkStartupBS.Instance.playerColors[3];
    }

    void Update()
    {
        scores[0].text = GameManagerBS.Instance.transform.Find("Player1Score").GetComponent<UIBS>().GetText();
        scores[1].text = GameManagerBS.Instance.transform.Find("Player2Score").GetComponent<UIBS>().GetText();
        scores[2].text = GameManagerBS.Instance.transform.Find("Player3Score").GetComponent<UIBS>().GetText();
        scores[3].text = GameManagerBS.Instance.transform.Find("Player4Score").GetComponent<UIBS>().GetText();
        scores[4].text = GameManagerBS.Instance.transform.Find("Timer").GetComponent<UIBS>().GetText();
    }
}
