using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreventingDeselection : MonoBehaviour
{
    [SerializeField]
    private GameObject arrowUp;
    [SerializeField]
    private GameObject arrowDown;

    private EventSystem evt;
    private GameObject sel;
    private GameObject sel2;


    private void Start()
    {
        evt = EventSystem.current;
    }

    private void Update()
    {
        if (evt.currentSelectedGameObject != null && (!evt.currentSelectedGameObject.Equals(arrowUp)) && evt.currentSelectedGameObject != sel)
            sel = evt.currentSelectedGameObject;
        else if (evt.currentSelectedGameObject != null &&  sel != null && evt.currentSelectedGameObject.Equals(arrowUp))
            evt.SetSelectedGameObject(sel);

        if (evt.currentSelectedGameObject != null && (!evt.currentSelectedGameObject.Equals(arrowDown)) && evt.currentSelectedGameObject != sel2)
            sel2 = evt.currentSelectedGameObject;
        else if (evt.currentSelectedGameObject != null && sel2 != null && evt.currentSelectedGameObject.Equals(arrowDown))
            evt.SetSelectedGameObject(sel2);
    }
}
