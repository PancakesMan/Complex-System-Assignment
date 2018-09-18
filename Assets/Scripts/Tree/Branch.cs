using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour {
    public List<GameObject> branchEnds;
    public List<GameObject> spawnableBranches;
    public GameObject bush;

    public int MinBranches = 5;
    public int MaxBranches = 15;

    public int branchAngle = 40;

	// Use this for initialization
	void Start () {
		foreach (GameObject end in branchEnds)
        {
            int depth = 0;
            Transform t = transform;
            while (t != null)
            {
                depth++;
                t = t.parent;
            }

            if (depth < Random.Range(MinBranches, MaxBranches))
            {
                GameObject obj = Instantiate(spawnableBranches[Random.Range(0, spawnableBranches.Count)], end.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.parent = end.transform;
                obj.transform.Rotate(new Vector3(Random.Range(-branchAngle, branchAngle), 0, Random.Range(-branchAngle, branchAngle)));
            }
            else
            {
                GameObject obj = Instantiate(bush, end.transform);
                //obj.transform.localScale = Vector3.one;
                obj.transform.parent = end.transform;
            }

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
