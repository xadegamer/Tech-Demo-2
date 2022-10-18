using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HackingMinigameButtonUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    private HackingMinigameUI hackingMinigameUI;
    private int x;
    private int y;

    public void Setup(HackingMinigameUI hackingMinigameUI, int x, int y) {
        this.hackingMinigameUI = hackingMinigameUI;
        this.x = x;
        this.y = y;
    }

    public void OnPointerClick(PointerEventData eventData) {
        hackingMinigameUI.OnClicked(x, y);
    }

    public void OnPointerExit(PointerEventData eventData) {
        hackingMinigameUI.OnGridOut(x, y);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hackingMinigameUI.OnGridOver(x, y);
    }

}
