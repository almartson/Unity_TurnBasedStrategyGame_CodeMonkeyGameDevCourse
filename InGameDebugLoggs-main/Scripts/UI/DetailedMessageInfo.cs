using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetailedMessageInfo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text conditionText;
    [SerializeField] TMP_Text stackTrace;

    public void AssignMessage(LogMessage message) {
        typeText.SetText(message.typeName);
        timeText.SetText(message.timeRecived);
        conditionText.SetText(message.condition);
        stackTrace.SetText(message.stackTrace);
    }
    public void OnPointerClick(PointerEventData eventData) {
        gameObject.SetActive(false);
    }

    private void Awake() {
        gameObject.SetActive(false);
    }
}
