using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickIntercept : EventTrigger
{
    protected PrefabPool prefabPool;
    private void Awake()
    {
        prefabPool = GameObject.Find("PrefabPool").GetComponent<PrefabPool>();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        string message = "" + worldPosition;
        Debug.Log(message);

        AddTurret(worldPosition);
    }

    protected void AddTurret(Vector3 worldPosition)
    {
        Transform turret = prefabPool.Turret;
        //the "worldPosition" is on the same plane as the camera (z-wise), move
        //it to the z-plane where the action is happening
        turret.position = new Vector3(worldPosition.x, worldPosition.y, 0);
    }

}
