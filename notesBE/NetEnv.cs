namespace notesBE;

public static class NetEnv
{
    public static void LoadEnvironmentVariables(string path)
    {
        if (!Path.Exists(path)) 
            throw new Exception("Path does not exist");

        foreach (var line in File.ReadLines(path))
        {
            var parts = line.Split('=', 2);

            if (parts.Length != 2) 
                continue;
            
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}