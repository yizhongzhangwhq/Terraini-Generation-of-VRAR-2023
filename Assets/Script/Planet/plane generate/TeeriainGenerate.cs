using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerate 
{

    
    // Set parameters including normal vector, and other vector in X and Y.
    // Inflate_folds is the traniangle numbers.
    // PS: this script don't need to update and start becasue it hasn't Play function.
    // this scirpt is control the cube to inflate with more trangles.
    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int Inflate_folds;
    Vector3 NormalUp;
    Vector3 _axisX;
    Vector3 _axisY;
     
    // Constructor, initialization.
    public TerrainGenerate(ShapeGenerator shapeGenerator, Mesh mesh, int Inflate_folds, Vector3 NormalUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.Inflate_folds = Inflate_folds;
        this.NormalUp = NormalUp;
    
    //Caculate Vector X Y by Cross, 
        _axisX = new Vector3(NormalUp.y, NormalUp.z, NormalUp.x);
        _axisY = Vector3.Cross(NormalUp, _axisX);
    }

    
    // ConstructMesh
    public void ConstructMesh()
    {
        // set array group data to storage triangles.
        // this scirpt is control the cube to inflate with more trangles.
        // vertices number equal the inflate_folds.
        // one fold unchange cube.
        // Mesh generate parameters in unity, index, triangle numbers, normals, vertices.
        Vector3[] vertices = new Vector3[Inflate_folds * Inflate_folds];
        int[] triangles = new int[(Inflate_folds - 1) * (Inflate_folds - 1) * 6];
        int triIndex = 0;
        
        // 
        for (int y = 0; y < Inflate_folds; y++)
        {
            for (int x = 0; x < Inflate_folds; x++)
            {
                int i = x + y * Inflate_folds;
                Vector2 percent = new Vector2(x, y) / (Inflate_folds - 1);
                Vector3 pointOnUnitCube = NormalUp + (percent.x - .5f) * 2 * _axisX + (percent.y - .5f) * 2 * _axisY;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != Inflate_folds - 1 && y != Inflate_folds - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + Inflate_folds + 1;
                    triangles[triIndex + 2] = i + Inflate_folds;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + Inflate_folds + 1;
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}