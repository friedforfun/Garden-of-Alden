using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlacerTest : MonoBehaviour
{
    private TreeScanner treeScanner;
    private TreePlacer treePlacer;
    private bool didScan = false;
    private float[,] texture;
    private float scannerReach = 50f;

    [SerializeField]
    private GameObject treePrefab;

    private void Awake()
    {
        treeScanner = new TreeScanner();
        treePlacer = new TreePlacer();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.z);
            texture = treeScanner.ScanForTrees(position, scannerReach, 25);
            didScan = true;

            List<GameObject> trees = FilterTreesOutOfScannerReach(GetAllTrees(), position);

            foreach (GameObject tree in trees)
            {
                Debug.Log("X: " + tree.transform.position.x + " Y: " + tree.transform.position.z);
                Destroy(tree);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (didScan)
            {
                // List<Vector2Int> treePositions = treePlacer.GetTreePositionsInTexture(texture);
                Vector2 scannerPosition = new Vector2(transform.position.x, transform.position.z);
                List<Vector2> treePositions = treePlacer.GetTreePositionsInWorld(texture, scannerReach, scannerPosition);
                Debug.Log("Number of spawned Trees: " + treePositions.Count);

                foreach (Vector2 treePosition in treePositions)
                {
                    Debug.Log("X: " + treePosition.x + " Y: " + treePosition.y);

                    Instantiate(treePrefab, new Vector3(treePosition.x, 0.1f, treePosition.y), Quaternion.identity);
                }
            }
        }
    }


    // Summery: Gets all trees in the world (!) 
    public List<GameObject> GetAllTrees()
    {
        List<GameObject> trees = new List<GameObject>();

        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Trees"))
        {
            trees.Add(tree);
        }

        return trees;
    }


    /* 
    * Summery: Takes a list of trees and returns a new list with only trees within the scanner
    * Takes:   All tree x,y coordinates, and the centerPoint of the scanner
    * Returns: A New list with only tree coordinates within the scanner range
    */
    public List<GameObject> FilterTreesOutOfScannerReach(List<GameObject> trees, Vector2 scanerCenterPoint)
    {
        List<GameObject> treesInScannerReach = new List<GameObject>();

        foreach (GameObject tree in trees)
        {
            Vector3 treePosition = tree.transform.position;

            if (Mathf.Abs(scanerCenterPoint.x - treePosition.x) < 50f &&
                Mathf.Abs(scanerCenterPoint.y - treePosition.z) < 50f)
            {
                treesInScannerReach.Add(tree);
            }
        }

        return treesInScannerReach;
    }
}
