using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextRenPyFinder;

public class Finder
{
    private List<Tuple<string, string>> _stringBuilders = new ();

    private int _fileIndex;
    private int _symbolIndex;
    private bool _finded;
    private string _lastFind;
    private HashSet<string> _extentions = new HashSet<string>();

    public Finder(string file)
    {
        _extentions.Add(".txt");
        _extentions.Add(".cs");
        _extentions.Add(".rpy");
        _extentions.Add(".ini");
        _extentions.Add(".log");
        LoadFile(file);
    }

    public void LoadFile(string path)
    {
        var dir = new DirectoryInfo(path);
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            if (_extentions.Contains(file.Extension))
            {
                _stringBuilders.Add(new Tuple<string, string>(file.FullName, File.ReadAllText(file.FullName).ToLowerInvariant()));
            }
        }

        var dirs = dir.GetDirectories();
        foreach (var directoryInfo in dirs)
        {
            LoadFile(directoryInfo.FullName);
        }
    }

    public FindedData Find(string str)
    {
        _finded = false;
        _fileIndex = 0;
        _symbolIndex = 0;
        return FindNext(str);
    }

    public FindedData FindNext(string str)
    {
        if (FindText(str.ToLowerInvariant(), out var findedData))
        {
            return findedData;
        }

        return null;
    }

    public bool FindText(string str, out FindedData findedData)
    {
        var startIndex = 0;
        for (int i = _fileIndex; i < _stringBuilders.Count; i++)
        {
            var builder = _stringBuilders[i].Item2;
            
            int index = 0;
            for (int j = _symbolIndex; j < builder.Length; j++)
            {
                if (builder[j] == str[index])
                {
                    if (index == 0)
                    {
                        startIndex = j;
                    }
                   
                    index++;
                    if (str.Length == index)
                    {
                        findedData = new FindedData
                        {
                            Index = startIndex, Lenght = str.Length, Path = _stringBuilders[i].Item1,
                            Text = _stringBuilders[i].Item2
                        };
                        _symbolIndex = j;
                        _fileIndex = i;
                        _finded = true;
                        return true;
                    }
                }
                else
                {
                    index = 0;
                }
            }

            _symbolIndex = 0;
        }

        if (_finded)
        {
            _fileIndex = 0;
            _symbolIndex = 0;
            return FindText(str, out findedData);
        }

        findedData = null;
        return false;
    }
}