using System;
using System.Collections.Generic;

[Serializable]
public class JsonList<T>
{
    public List<T> list;

    public JsonList()
    {
    }

    public JsonList(List<T> list)
    {
        this.list = list;
    }
}