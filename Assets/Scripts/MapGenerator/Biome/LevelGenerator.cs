using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _levelSize;
    [SerializeField] private int _seed;

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        int order = Mathf.CeilToInt(Mathf.Log(_levelSize, 2));
        Vector2Int[] hilbertCurve = HilbertCurve.GenerateHilbertCurve(order);

        // ��������� ��������� ��������
        List<Vector2Int> mainPath = new List<Vector2Int>(hilbertCurve);

        // ��������� �������� �����
        List<Vector2Int> shortcuts = GenerateShortcuts(mainPath);

        // ��������� �������
        List<Vector2Int> deadEnds = GenerateDeadEnds(mainPath);

        // ����������� ���� �����
        List<Vector2Int> allPaths = new List<Vector2Int>(mainPath);
        allPaths.AddRange(shortcuts);
        allPaths.AddRange(deadEnds);

        // �������������� ����� � ��������� ������
        List<GameObject> levelFragments = ConvertPathsToFragments(allPaths);

        // �������� � ���������� ���������� ������
        LoadAndSaveLevelFragments(levelFragments);
    }

    private List<Vector2Int> GenerateShortcuts(List<Vector2Int> mainPath)
    {
        List<Vector2Int> shortcuts = new List<Vector2Int>();
        System.Random random = new System.Random(_seed);

        for (int i = 0; i < mainPath.Count - 1; i++)
        {
            if (random.NextDouble() < 0.1) // 10% ���� �������� �������� ����
            {
                Vector2Int start = mainPath[i];
                Vector2Int end = mainPath[i + 1];
                Vector2Int midPoint = new Vector2Int((start.x + end.x) / 2, (start.y + end.y) / 2);
                shortcuts.Add(midPoint);
            }
        }

        return shortcuts;
    }

    private List<Vector2Int> GenerateDeadEnds(List<Vector2Int> mainPath)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        System.Random random = new System.Random(_seed);

        for (int i = 0; i < mainPath.Count; i++)
        {
            if (random.NextDouble() < 0.05) // 5% ���� �������� �����
            {
                Vector2Int point = mainPath[i];
                Vector2Int deadEnd = point + new Vector2Int(random.Next(-1, 2), random.Next(-1, 2));
                deadEnds.Add(deadEnd);
            }
        }

        return deadEnds;
    }

    private List<GameObject> ConvertPathsToFragments(List<Vector2Int> allPaths)
    {
        List<GameObject> levelFragments = new List<GameObject>();

        foreach (var path in allPaths)
        {
            GameObject fragment = new GameObject("LevelFragment");
            fragment.transform.position = new Vector3(path.x, 0, path.y);
            levelFragments.Add(fragment);
        }

        return levelFragments;
    }

    private void LoadAndSaveLevelFragments(List<GameObject> levelFragments)
    {
        // ������ �������� � ���������� ���������� ������
        // � ������ ������� �� ������ ������� ���������� ����������
        Debug.Log("Generated " + levelFragments.Count + " level fragments.");
    }
}
