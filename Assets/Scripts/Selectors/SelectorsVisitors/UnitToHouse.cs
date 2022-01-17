using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitToHouse : VisitorSelector
{
    sealed public override void Initialize(GameObject target)
    {
        base.Initialize(target);
    }

    public void Fire()
    {
        if (target == null)
            return;

        //UnitsSystem.singleton.FireBuild(target);
    }

    public void Fix()
    {
        if (target == null)
            return;

        //UnitsSystem.singleton.FixBuild(target);
    }
}
