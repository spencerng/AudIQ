using System.Collections.Generic;

public class Algorithms
{
    //Algorithm: Fisher-Yates Shuffle, taken from https://stackoverflow.com/questions/273313/randomize-a-listt
    public static List<int> ShuffleList(List<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }

        return list;
    }
}