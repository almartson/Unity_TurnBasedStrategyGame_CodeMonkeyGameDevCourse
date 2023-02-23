using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ToggleAbleCanvasGroup : MonoBehaviour {
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private bool startActive;

    public Action<bool> OnActivationChanged;

    private bool isActive;

    public bool IsActive => isActive;

    private void Awake() {
        ToggleCanvas();
        if(isActive != startActive) {
            ToggleCanvas();
        }
    }
    private void OnValidate() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ToggleCanvas() {
        if(canvasGroup != null) {
            isActive = !isActive;
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.blocksRaycasts = isActive;
            canvasGroup.interactable = isActive;
            OnActivationChanged?.Invoke(isActive);
        }
    }
}
