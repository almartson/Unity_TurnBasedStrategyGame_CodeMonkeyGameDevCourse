using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugMessage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text messageTMP_text;
    [SerializeField] private Color messageColor = Color.white;
    [SerializeField] private Color warningAndAssertColor = Color.yellow;
    [SerializeField] private Color errorAndExceptionColor = Color.red;

    private LogMessage message;
    private DetailedMessageInfo detailedMessageInfo;

    public LogType MessageType => message.type;

    public void OnPointerClick(PointerEventData eventData) {
        detailedMessageInfo.gameObject.SetActive(true);
        detailedMessageInfo.AssignMessage(message);
    }

    public void SetDebugMessageInfo(LogMessage message) {

        this.message = message;
        SetTextColor();
        messageTMP_text.SetText(message.condition);
    }

    private void SetTextColor() {
        Color color = messageColor;
        if(MessageType == LogType.Error || MessageType == LogType.Exception) {
            color = errorAndExceptionColor;
        }
        else if(MessageType == LogType.Warning || MessageType == LogType.Assert) {
            color = warningAndAssertColor;
        }
        messageTMP_text.color = color;
    }

    public void SetDetailedMessageInfoPanel(DetailedMessageInfo detailedMessageInfo) {
        this.detailedMessageInfo = detailedMessageInfo;
    }
}
