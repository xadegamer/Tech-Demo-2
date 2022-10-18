using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaycastUI : MonoBehaviour {

    private GraphicRaycaster raycaster;
    private IPointerWorldUI lastSelectedButtonWorldUI;

    private void Awake() {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Update() {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        pointerData.position = Input.mousePosition;

        raycaster.Raycast(pointerData, results);

        if (results.Count > 0) {
            if (results[0].gameObject.TryGetComponent<IPointerWorldUI>(out IPointerWorldUI buttonWorldUI)) {
                // Is it a different button from the selected one?
                if (buttonWorldUI != lastSelectedButtonWorldUI) {
                    // Deselect previous one
                    lastSelectedButtonWorldUI?.PointerExit();
                    // Select current one
                    lastSelectedButtonWorldUI = buttonWorldUI;
                    lastSelectedButtonWorldUI.PointerEnter();
                }
            }
        } else {
            // Nothing selected
            lastSelectedButtonWorldUI?.PointerExit();
            lastSelectedButtonWorldUI = null;
        }
        foreach (RaycastResult result in results) {
            if (result.gameObject.TryGetComponent<IPointerEnterHandler>(out IPointerEnterHandler pointerEnterHandler)) {
                pointerEnterHandler.OnPointerEnter(pointerData);
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            // Activate!
            lastSelectedButtonWorldUI?.PointerDown();
        }
    }

}
