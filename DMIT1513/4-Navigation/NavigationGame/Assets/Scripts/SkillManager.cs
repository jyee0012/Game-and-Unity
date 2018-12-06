using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {

    bool first = false;
    GameObject currentActive = null;
    [SerializeField]
    GameObject heavyUnit, sniperUnit;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ToggleSkillNameButton(GameObject button)
    {
        // code
        ToggleSkillButton(button);
    }
    public void ToggleSkillButton(GameObject button)
    {
        if (currentActive != null)
        {
            if (button != currentActive)
                first = !first;

            currentActive.GetComponent<Image>().color = Color.white;
            currentActive = null;
        }
        if (FirstSkill())
        {
            button.GetComponent<Image>().color = Color.green;
            currentActive = button;
        }
        else
        {
            button.GetComponent<Image>().color = Color.white;
            currentActive = null;
        }
    }
    public bool FirstSkill()
    {
        first = !first;
        return first;
    }
    public static void UseSkill(Vector3 clickPoint)
    {

    }
}
