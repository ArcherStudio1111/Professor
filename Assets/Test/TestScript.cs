using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class TestScript : MonoBehaviour
{
    public List<float> paraList1 = new List<float>();
    public List<float> paraList2 = new List<float>();
    public List<string> outputList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //outputList.Add(String.Join(",", paraList1.Select(x => x.ToString()).ToArray()));
        //outputList.Add(String.Join(",", paraList2.Select(x => x.ToString()).ToArray()));
        File.WriteAllText(@"\result", "aaa");
    }
}
