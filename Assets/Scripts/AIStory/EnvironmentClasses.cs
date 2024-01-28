using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public Position position;
    public Environment environment;
}

[System.Serializable]
public class Position
{
    public int x, y, z;
}

[System.Serializable]
public class Environment
{
    public string vegetation;
    public string wildlife;
    public string terrain;
}
