using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    static Block s_block;
    public Block block;

    public static Block Block { get => s_block; }

    // Start is called before the first frame update
    void Awake()
    {
        s_block = block;
    }
}
