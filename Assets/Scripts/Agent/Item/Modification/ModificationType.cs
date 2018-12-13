using System.Collections.Generic;
using UnityEngine;

public class ModificationType
{
    public static readonly ModificationType ICE = new ModificationType("Ice");
    public static readonly ModificationType SANDPAPER = new ModificationType("Sandpaper");
    public static readonly ModificationType RUBBER = new ModificationType("Rubber");
    public static readonly ModificationType SPIKES = new ModificationType("Spikes");
    public static readonly ModificationType POSIONUS = new ModificationType("Posionus");
    public static readonly ModificationType MASS = new ModificationType("Mass");

    public static IEnumerable<ModificationType> Values
    {
        get
        {
            yield return ICE;
            yield return SANDPAPER;
            yield return RUBBER;
            yield return SPIKES;
            yield return POSIONUS;
            yield return MASS;
        }
    }

    public string Name { get; private set; }

    public ModificationType(string name)
    {
        Name = name;
    }

    public static ModificationType GetModificationByName(string name)
    {
        foreach (ModificationType modification in Values)
        {
            if (modification.Name.Equals(name))
            {
                return modification;
            }
        }

        return null;
    }

    public override string ToString()
    {
        return Name;
    }
}