using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestExpressionEvaluator : MonoBehaviour {

    public bool output = false;
    public string expression;

	// Use this for initialization
	void Start () {
	}

    void Update()
    {
        if (output)
        {
            output = !output;
            Debug.Log(Calculate()); //Testing Something
            // oof
        }
    }

    public int Calculate()
    {
        //It's a kind o' magic
        return ExpressionEvaluator.Evaluate<int>(expression);
    }
}
