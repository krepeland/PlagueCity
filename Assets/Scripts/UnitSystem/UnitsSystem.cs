using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Classes.Generators.RoadsGenerator;

public class UnitsSystem : MonoBehaviour
{
    public static UnitsSystem singleton;

    [SerializeField] private UnitSystemSettings settings;

    [SerializeField] private GameObject prefabKnight;
    [SerializeField] private GameObject prefabPlagueDoctor;

    private List<UnitBase> knights = new List<UnitBase>();

    public void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        ResourcesSystem.singleton.GetResourses();
    }

    public void CreateAndNavigateUnit<T>(GameObject build) where T : UnitBase
    {
        //var res = ResourcesSystem.singleton.GetResoursesUnitCount<T>();

        //if (res > 0)
        //{
           // res--;

          //  ResourcesSystem.singleton.SetResoursesUnitCount<T>(res);

            var unit = SpawnUnit<T>(new List<Vector3>()
        { CityGenerator.singleton.GetStartPoint().Position }, build);


            NavigateUnit<T>(unit.GetComponent<T>(), build);
       // }
    }

    public void NavigateUnit<T>(T unit, GameObject build) where T : UnitBase
    {


        var way = RoadFind.FindRoadTo(unit.transform.position, build.transform.position);

        //for (var i = 0; i < 5; i++)
        //    way.Add(build.transform.position);

        //var half = new List<Vector3>();
        //for (var i = way.Count - 1; i >= 0; i--)
        //    half.Add(way[i]);


        // way.AddRange(half);

        //var unitBase = unit.GetComponent<UnitBase>();

        if (unit != null)
            unit.SetWalkWay(way);
        else
            throw new System.Exception($"'{unit}' this GameObject haven't {typeof(UnitBase).Name} component");


    }

    private GameObject SpawnUnit<T>(List<Vector3> road, GameObject target) where T : UnitBase
    {
        if (typeof(T).Name.Equals(typeof(KnightUnit).Name))
        {
            var go = Instantiate(prefabKnight, new Vector3(0, 0, 0.1f), Quaternion.identity);//new GameObject();
            go.transform.position = road[0];
            var kn = go.GetComponent<UnitBase>();
            kn.Initialize(road, 0.1f, 3, target);

            return go;
        }
        if (typeof(T).Name.Equals(typeof(PlagueDoctorUnit).Name))
        {
            var go = Instantiate(prefabPlagueDoctor, new Vector3(0, 0, 0.1f), Quaternion.identity);//new GameObject();
            go.transform.position = road[0];
            var kn = go.GetComponent<UnitBase>();
            kn.Initialize(road, 0.1f, 3, target);

            return go;
        }

        throw new System.Exception("False Unit Type");
    }
}
