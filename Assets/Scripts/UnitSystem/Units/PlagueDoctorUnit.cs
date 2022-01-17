using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorUnit : UnitBase
{
    protected override bool Walk()
    {
        if (base.Walk())
        {
            return true;
        }
        else
        {
            var res = ResourcesSystem.singleton.GetResourses();
            res.CountPlagueDoctor++;
            ResourcesSystem.singleton.SetResources(res);
        }

        return false;
    }

    protected override void DoAction()
    {
        target.GetComponent<BuildStates>().States = StateBuildType.Fix;

        base.DoAction();
    }

    public override int GetCountUnitsResources()
    {
        var res = ResourcesSystem.singleton.GetResourses();
        return res.CountKnights;
    }

    public override void SetCountUnitsResources(int count)
    {
        ResourcesSystem.singleton.SetResoursesUnitCount<PlagueDoctorUnit>(count);
    }
}
