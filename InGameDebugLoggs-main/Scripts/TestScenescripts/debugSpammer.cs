    using UnityEngine;

public class debugSpammer : MonoBehaviour {
    int counter;
    private void Awake() {
        InvokeRepeating("ShowDebugMessage", 1, 1);
    }

    void ShowDebugMessage() {
        if(counter % 3 == 0) {
            Debug.Log("Hej Hej, Vanlig log" + counter);
        }
        else if((counter + 1) % 3 == 0) {
            Debug.LogWarning("Hej Hej, En varning!" + counter);
        }
        else if((counter + 2) % 3 == 0) {
            Debug.LogError("HEJ HEJ!!! FEEEEEEL!" + counter);
        }

        counter++;
    }
}
