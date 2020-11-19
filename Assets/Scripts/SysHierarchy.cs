using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.


public class SysHierarchy
{
    public static List<Vector2Int> CreateSysTree(int stars)
    {
        int nodes = stars - 1;

        /*
        tree = [[-1, -1]]
        free_edges = [(0, 0), (0, 1)]
        */
        List<Vector2Int> tree = new List<Vector2Int>();
        Vector2Int new_node = new Vector2Int(-1, -1);
        tree.Add(new_node);

        List<Vector2Int> free_edges = new List<Vector2Int>();
        Vector2Int v00 = new Vector2Int(0, 0);
        free_edges.Add(v00);
        Vector2Int v01 = new Vector2Int(0, 1);
        free_edges.Add(v01);


        /*
        while len(tree) < n:
        edge = random.choice(free_edges)  # select a free edge
        node, child = edge
        assert tree[node][child] == -1  # make sure we made no mistake
        */
        while (tree.Count < nodes)
        {
            //Debug.Log("Tree count: " + tree.Count);
            int index = Random.Range(0, free_edges.Count);
            //Debug.Log("index: " + index);
            Vector2Int edge = free_edges[index];
            //Debug.Log("edge: " + edge);
            int node = edge.x;
            //Debug.Log("node: " + node);
            int child = edge.y;
            //Debug.Log("child: " + child);
            Debug.Assert(tree[node][child] == -1);

            /*
            k = len(tree)  # index of new node
            tree.append([-1, -1])  # add new node
            */
            int k = tree.Count;                 // index of new node
            tree.Add(new_node);                 // add new node


            /*
            tree[node][child] = k  # set new node as child of an old node
            */
            Vector2Int oldNode = tree[node];
            //Debug.Log("Old node: " + oldNode);
            if (child == 0)
            {
                tree[node] = new Vector2Int(k, oldNode[1]);
                //Debug.Log("New node: " + tree[node]);
            }
            else if (child == 1)
            {
                tree[node] = new Vector2Int(oldNode[0], k);
                //Debug.Log("New node: " + tree[node]);
            }
            else { Debug.Log("bad"); }


            /*
            free_edges.extend([(k, 0), (k, 1)])  # new node has two free edges
            */
            free_edges.Add(new Vector2Int(k, 0));
            free_edges.Add(new Vector2Int(k, 1));
            /*
            for (int ed = 0; ed < free_edges.Count; ed++)
            {
                Debug.Log("free edges:" + free_edges[ed]);
            }
            */


            /*
            free_edges.remove(edge)  # edge is no longer free
            */
            int feIndex;
            feIndex = free_edges.IndexOf(edge);
            Debug.Assert(feIndex >= 0);
            //Debug.Log("free edge index: " + feIndex);
            if (index > -1)
                //Debug.Log("Removing edge: " + free_edges[index]);
                free_edges.RemoveAt(index);
        }
        for (int i = 0; i < tree.Count; i++)
        {
            //sort the tree
            if (tree[i][1] < tree[i][0])
            {
                tree[i] = new Vector2Int(tree[i][1], tree[i][0]);
            }
        }
        return tree;
    }

    public static int GetParentIndex(List<Vector2Int> tree, int node)
    {
        int parentNode = -1;
        for (int i = 0; i < node; i++)
        {
            if (tree[i][0] == node || tree[i][1] == node)
            {
                parentNode = i;
            }
        }
        Debug.Assert(parentNode != -1);
        return parentNode;
    }

    public static List<int>[,] GetChildren(List<Vector2Int> tree)
    {
        int nodes = tree.Count;
        List<int>[,] all_children = new List<int>[nodes, 2];
        for (int i = 0; i < nodes; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                all_children[i, j] = new List<int>();
            }
        }
        int parent_node = -1;
        for (int i = 0; i < nodes; i++)
        {
            if (tree[i][0] == -1)
            {
                Vector2Int tempV = tree[i];
                tree[i] = new Vector2Int(parent_node, tempV[1]);
                parent_node--;
            }
            if (tree[i][1] == -1)
            {
                Vector2Int tempV = tree[i];
                tree[i] = new Vector2Int(tempV[0], parent_node);
                parent_node--;
            }
        }
        for (int i = nodes - 1; i > -1; i--)
        {
            if (tree[i][0] < 0)
            {
                all_children[i, 0].Add(tree[i][0]);
            }
            if (tree[i][1] < 0)
            {
                all_children[i, 1].Add(tree[i][1]);
            }
        }
        for (int i = nodes - 1; i > -1; i--)
        {
            if (tree[i][0] > 0)
            {
                for (int x = 0; x < all_children[tree[i][0], 0].Count; x++)
                {
                    all_children[i, 0].Add(all_children[tree[i][0], 0][x]);
                }
                for (int y = 0; y < all_children[tree[i][0], 1].Count; y++)
                {
                    all_children[i, 0].Add(all_children[tree[i][0], 1][y]);
                }
            }
            if (tree[i][1] > 0)
            {
                for (int x = 0; x < all_children[tree[i][1], 0].Count; x++)
                {
                    all_children[i, 1].Add(all_children[tree[i][1], 0][x]);
                }
                for (int y = 0; y < all_children[tree[i][1], 1].Count; y++)
                {
                    all_children[i, 1].Add(all_children[tree[i][1], 1][y]);
                }
            }
        }
        for (int i = 0; i < nodes; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int v = 0; v < all_children[i, j].Count; v++)
                {
                    //abs(value) - 1
                    all_children[i, j][v] = Mathf.Abs(all_children[i, j][v]) - 1;
                }
            }
        }
        return all_children;
    }

    public static int[][] GetHierarchy(List<Vector2Int> tree)
    {
        int nodes = tree.Count;

        // assert(nnodes >= 0)
        Debug.Assert(nodes >= 0);

        //system_hierarchy = 0
        int system_hierarchy = 0;

        //node_hierarchy = [0] * nnodes
        int[] node_hierarchy = new int[nodes];

        /*
        for idx in range(1, nnodes) :
            index = idx
            possible_hierarchy = 0
        */
        for (int idx = 1; idx < nodes; idx++)
        {
            int index = idx;
            int possible_hierarchy = 0;

            /*
            while index > 0:
                index = find_parent_index(tr, index)
                possible_hierarchy = possible_hierarchy + 1
                node_hierarchy[idx] = possible_hierarchy
            */
            while (index > 0)
            {
                index = GetParentIndex(tree, index);
                possible_hierarchy = possible_hierarchy + 1;
                node_hierarchy[idx] = possible_hierarchy;
            }
            //node_hierarchy[0] = 0
            node_hierarchy[0] = 0;
            /*
            if system_hierarchy < possible_hierarchy:
                system_hierarchy = possible_hierarchy
            */
            if (system_hierarchy < possible_hierarchy)
            {
                system_hierarchy = possible_hierarchy;
            }
        }

        system_hierarchy += 1;

        var h = new[]
        {
            new[] { system_hierarchy },
            new[] { 0 }
        };

        h[1] = node_hierarchy;

        //return system_hierarchy + 1, node_hierarchy
        return h;
    }

    public static void PrintTree(List<Vector2Int> tree)
    {
        Debug.Log("TREE ################");
        int nodes = tree.Count;
        for (int i = 0; i < nodes; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Debug.Log("Node " + i + " [" + tree[i][0] + "," + tree[i][1] + "]");
            }
        }
    }

    public static void PrintChildren(List<int>[,] all_children)
    {
        int nodes = all_children.Length / 2; // children are node pairs so length of children is 2xnodes
        Debug.Log("CHILDREN ################");
        for (int i = 0; i < nodes; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int v = 0; v < all_children[i, j].Count; v++)
                {
                    Debug.Log("all_children[i,j] " + i + "," + j + "        " + all_children[i, j][v]);
                }
            }
        }
    }

    public static void PrintHierarchy(int[][] h)
    {
        Debug.Log("HIERARCHY ###############");
        Debug.Log("System Hierarchy: " + h[0][0]);
        for (int i = 0; i < h[1].Length; i++)
        {
            Debug.Log("Node Hierarchy[" + i + "] " + h[1][i]);
        }
    }
}