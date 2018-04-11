using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClick : EventTrigger {
    PrefabPool prefabPool;
    private void Awake()
    {
        prefabPool = GameObject.Find("PrefabPool").GetComponent<PrefabPool>();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        AddTurret(worldPosition);
    }
    protected void AddTurret(Vector3 position)
    {
        Transform turret = prefabPool.Turret;
        turret.position = new Vector3(position.x, position.y, 0);
    }
}
