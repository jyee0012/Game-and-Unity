using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    public enum Skill
    {
        None = 0,
        SpawnUnit = 1,
        Possess = 2
    }
    public Skill currentSkill = Skill.None;
    bool bActiveSkill = false;
    GameObject currentActive = null;
    [SerializeField]
    GameObject heavyUnit, sniperUnit;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ActivateSkillButton(GameObject button, int skillIndex)
    {
        // code
        ChangeSkill(skillIndex);
        ToggleSkillButton(button);
    }
    public void ChangeSkill(int skillIndex)
    {
        if (bActiveSkill)
            currentSkill = (Skill)skillIndex;
        else
            currentSkill = 0;
    }
    public void ToggleSkillButton(GameObject button)
    {
        if (currentActive != null)
        {
            if (button != currentActive)
                bActiveSkill = !bActiveSkill;

            currentActive.GetComponent<Image>().color = Color.white;
            currentActive = null;
        }
        if (ToggleActiveSkill())
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
    public bool ToggleActiveSkill()
    {
        bActiveSkill = !bActiveSkill;
        return bActiveSkill;
    }
    public void UseSkill(Vector3 clickPoint)
    {
        if (currentSkill == Skill.None) return;

        switch (currentSkill)
        {
            case Skill.SpawnUnit:
                SpawnHeavyUnits(clickPoint);
                break;
            case Skill.Possess:
                break;
            default:
                break;
        }
    }
    void SkillSpawnUnits(Vector3 clickPoint, GameObject unit)
    {
        UnitSpawner.StaticSpawnUnits(unit, clickPoint, 5, 0.5f, 0.5f);
    }
    void SpawnSniperUnits(Vector3 clickPoint)
    {
        SkillSpawnUnits(clickPoint, sniperUnit);
    }
    void SpawnHeavyUnits(Vector3 clickPoint)
    {
        SkillSpawnUnits(clickPoint, heavyUnit);
    }
}
