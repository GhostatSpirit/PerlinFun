using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureGenerator {

    static int k = 30;

    // generate structures in local space, i.e. there will be coordinates with negative numbers
    public static List<Vector2> GenerateStructures(float[,] noiseMap, float r, int seed)
    {
        float w = r / Mathf.Sqrt(2);


        System.Random random = new System.Random(seed);

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        int cols, rows;
        cols = (int) Mathf.Floor(width / w);
        rows = (int)Mathf.Floor(height / w);

        // Step 0
        int[,] grid = new int[rows, cols];
        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < cols; ++j)
            {
                grid[i, j] = -1;
            }
        }

        List<int> actives = new List<int>();
        List<Vector2> emitted = new List<Vector2>();

        // Step 1
        // generated a random start point
        float initX = (float)random.NextDouble() * width;
        float initY = (float)random.NextDouble() * height;
        Vector2 initPoint = new Vector2(initX, initY);
        emitted.Add(initPoint);
        actives.Add(0);

        // Step 2
        while (actives.Count > 0)
        {
            int randIndex = random.Next(0, actives.Count);
            Vector2 pos = emitted[actives[randIndex]];
            var found = false;

            for(int n = 0; n < k; ++n)
            {
                Vector2 offset = GenerateAroundOffset(random, r);
                Vector2 sample = pos + offset;

                int col = (int)Mathf.Floor(sample.x / w);
                int row = (int)Mathf.Floor(sample.y / w);

                if(col >= 0 && col < cols && row >=0 && row < rows && grid[row, col] < 0)
                {
                    bool ok = true;
                    for(int i = Mathf.Max(row - 1, 0); i <= Mathf.Min(row + 1, rows - 1); ++i)
                    {
                        for(int j = Mathf.Max(col - 1, 0); j <= Mathf.Min(col + 1, cols - 1); ++j)
                        {
                            var index = grid[i, j];
                            if (index < 0) continue;
                            else
                            {
                                Vector2 neighbor = emitted[index];
                                float dist = Vector2.Distance(sample, neighbor);

                                if(dist < r)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }
                        if (ok == false) break;
                    }

                    if (ok)
                    {
                        found = true;
                        emitted.Add(sample);

                        int newIndex = emitted.Count - 1;
                        grid[row, col] = newIndex;
                        actives.Add(newIndex);

                    }
                    
                }
            }

            if (!found)
            {
                // delete it from the active list
                actives.RemoveAt(randIndex);
            }
        }


        // grid space -> local space
        Vector2 transformVec = new Vector2(width / 2.0f, height / 2.0f);

        for (int i = 0; i < emitted.Count; ++i)
        {
            emitted[i] -= transformVec;
        }

        return emitted;
        
    }

    static Vector2 GenerateAroundOffset(System.Random random, float r)
    {
        float theta = (float)random.NextDouble() * Mathf.PI * 2.0f;
        float radius = (float)random.NextDouble() * r + r;

        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

        return new Vector2(x, y);
    }
	
}
