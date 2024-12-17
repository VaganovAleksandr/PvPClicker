using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeScript : MonoBehaviour, IPointerDownHandler
{
    public ObjectsHolder Holder;
    public GameObject self;
    public void OnPointerDown(PointerEventData eventData)
	{
        if (!Holder.gameCore.state.running_state[Holder.gameCore.PlayerColor]) {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left) {
            Holder.gameCore.LeftClickOnNode(Int32.Parse(self.GetComponent<Text>().text));
        }
        if (eventData.button == PointerEventData.InputButton.Right) {
            Holder.gameCore.RightClickOnNode(Int32.Parse(self.GetComponent<Text>().text));
        }
    }
}
