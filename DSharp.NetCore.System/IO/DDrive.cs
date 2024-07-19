namespace DSharp.NetCore.IO;

/// <summary>
/// 
/// </summary>
public static class DDrive
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static List<string> GetLocalDrives()
    {
        var list = new List<string>();

        foreach (var drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
                list.Add(drive.Name);
        }

        return list;
    }
}