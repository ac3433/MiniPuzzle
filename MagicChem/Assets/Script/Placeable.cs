using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Placeable : MonoBehaviour, IDropHandler
{

    private GameObject Item
    {
        //if there is a tile exist on the grid, get it
        get
        {

            if (transform.childCount > 0)
            {
                foreach(Transform child in transform)
                {
                    if (string.Equals(child.tag, "CircleNode"))
                        return child.gameObject;
                }
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        TileData td = GetComponent<TileData>();
        MouseInteraction mi = eventData.pointerDrag.transform.GetComponent<MouseInteraction>();
        if (Item != null)
        {
            //mi.ResetPosition(); 
        }
        else
        {
            mi.SetPoint(td.GetPoint());
            MouseInteraction.DraggedObject.transform.SetParent(transform);
        }
    }
}
