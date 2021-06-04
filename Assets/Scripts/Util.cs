using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    public static void RotateBoolMatrixClockwise(bool[,] arr)
    {
        transpose(arr);
        reverseRows(arr);
    }

    public static void RotateBoolMatrixCunterClockwise(bool[,] arr)
    {
        transpose(arr);
        reverseColumns(arr);
    }

    static void transpose(bool[,] arr)
    {
        int size = (int)Mathf.Sqrt(arr.Length);


        for (int i = 0; i < size; i++)
            for (int j = i; j < size; j++)
                swap(ref arr[i,j], ref arr[j,i]);
    }

    static void swap(ref bool a, ref bool b)
    {
        bool c;
        c = a;
        a = b;
        b = c;
    }

    static void reverseRows(bool[,] arr)
    {
        int size = (int)Mathf.Sqrt(arr.Length);
        int temp;
        for (int i = 0; i < size; i++)
        {
            temp = size - 1;
            for (int j = 0; j < temp; j++)
            {
                swap(ref arr[i,j], ref arr[i,temp]);
                temp--;
            }
        }
    }

    static void reverseColumns(bool[,] arr)
    {
        int size = (int)Mathf.Sqrt(arr.Length);
        int temp;
        for (int i = 0; i < size; i++)
        {
            temp = size - 1;
            for (int j = 0; j < temp; j++)
            {
                swap(ref arr[j,i], ref arr[temp, i]);
                temp--;
            }
        }
    }

#if DEBUG
    public static void printMatrix(bool[,] arr)
    {
        string print = "\n";
        int size = (int)Mathf.Sqrt(arr.Length);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
                print += arr[i,j] ? "1," : "0,";

            print += "\n";
        }
        Debug.Log(print);
    }
#endif
}
