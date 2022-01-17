using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightUnit : UnitBase
{
    protected override void TryAttack(UnitBase unit)
    {
        if (unit is AngryMenUnit)
        {
            var output = unit.SetDamage(Healh);

            SetDamage(output);
        }
    }

    protected override bool Walk()
    {
        if (base.Walk())
        {
            return true;
        }
        //else
        //{
        //    var res = ResourcesSystem.singleton.GetResourses();
        //    res.CountKnights++;
        //    ResourcesSystem.singleton.SetResources(res);
        //}

        return false;
    }

    protected override void DoAction()
    {
        target.GetComponent<BuildStates>().States = StateBuildType.Fire;

        base.DoAction();
    }

    public override int GetCountUnitsResources()
    {
        var res = ResourcesSystem.singleton.GetResourses();
        return res.CountKnights;
    }

    public override void SetCountUnitsResources(int count)
    {
        ResourcesSystem.singleton.SetResoursesUnitCount<KnightUnit>(count);
    }
}
