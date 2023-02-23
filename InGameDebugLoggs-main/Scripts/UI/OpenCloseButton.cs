using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenCloseButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;
    [SerializeField] private ToggleAbleCanvasGroup toggleAbleCanvasGroup;
    [SerializeField] private bool useHotkey;
    [SerializeField] private KeyCode hotKey = KeyCode.L;

    public void UpdateSpriteBasedOnCanvasActive() {
        buttonImage.sprite = toggleAbleCanvasGroup.IsActive ? closeSprite : openSprite;
    }
        
    private void Start() {
        button = GetComponent<Button>();
        if(toggleAbleCanvasGroup!= null && button != null) {
            button.onClick.AddListener(toggleAbleCanvasGroup.ToggleCanvas);
            button.onClick.AddListener(UpdateSpriteBasedOnCanvasActive);
            UpdateSpriteBasedOnCanvasActive();
        }
    }

    private void Update() {
        if(Input.GetKeyDown(hotKey) && useHotkey) {
            button.onClick.Invoke();
        }
    }
}
