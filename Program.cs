using System;
using System.Collections.Generic;
using System.IO;

namespace GTA5OnlineLua;

internal class Program
{
    static void Main(string[] args)
    {
        var kiddionLua = new List<LuaInfo>();
        var yimMenuLua = new List<LuaInfo>();

        Console.WriteLine("操作开始!");
        Console.WriteLine();

        GetLuaInfo(kiddionLua, "Kiddion");
        Console.WriteLine();
        GetLuaInfo(yimMenuLua, "YimMenu");

        JsonHelper.WriteFile("./Kiddion.json", kiddionLua);
        JsonHelper.WriteFile("./YimMenu.json", yimMenuLua);

        Console.WriteLine();
        Console.WriteLine("操作结束!");
        Console.ReadLine();
    }

    private static void GetLuaInfo(List<LuaInfo> onlineLua, string dirName)
    {
        foreach (var file in Directory.GetFiles($"./{dirName}"))
        {
            var fileInfo = new FileInfo(file);

            if (fileInfo.Extension != ".ini")
                continue;

            var luaFile = fileInfo.FullName.Replace(".ini", "") + ".zip";
            var luaFileInfo = new FileInfo(luaFile);

            var name = IniHelper.ReadValue("LuaInfo", "Name", file);
            var author = IniHelper.ReadValue("LuaInfo", "Author", file);
            var description = IniHelper.ReadValue("LuaInfo", "Description", file);
            var version = IniHelper.ReadValue("LuaInfo", "Version", file);
            var update = IniHelper.ReadValue("LuaInfo", "Update", file);

            var size = GetFileFormatSize(luaFileInfo.Length);
            var download = $"{dirName}/{luaFileInfo.Name}";

            onlineLua.Add(new()
            {
                Name = name,
                Author = author,
                Description = description,
                Version = version,
                Update = update,
                Size = size,
                Download = GetDownloadAddress(download)
            });

            Console.WriteLine($"{dirName} 脚本: {luaFileInfo.Name} 获取信息成功");
        }
    }

    private static List<string> GetDownloadAddress(string link)
    {
        var downloads = new List<string>
        {
            $"https://blog.cc2077.site/https://raw.githubusercontent.com/CrazyZhang666/GTA5OnlineLua/main/{link}",
            $"https://ghproxy.net/https://raw.githubusercontent.com/CrazyZhang666/GTA5OnlineLua/main/{link}",
            $"https://gcore.jsdelivr.net/gh/CrazyZhang666/GTA5OnlineLua@main/{link}",
            $"https://jsdelivr.b-cdn.net/gh/CrazyZhang666/GTA5OnlineLua@main/{link}",
            $"https://github.moeyy.xyz/https://raw.githubusercontent.com/CrazyZhang666/GTA5OnlineLua/main/{link}",
            $"https://raw.githubusercontent.com/CrazyZhang666/GTA5OnlineLua/main/{link}"
        };

        return downloads;
    }

    private static string GetFileFormatSize(long size)
    {
        var kb = size / 1024.0f;

        if (kb > 1024)
            return $"{kb / 1024.0f:0.00} MB";
        else
            return $"{kb:0.00} KB";
    }
}
