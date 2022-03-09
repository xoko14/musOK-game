using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class BulletPattern
{
    public string Name { get; set; }
    public BulletType Type { get; set; }
    public float PosX { get; set; }
    public float Velocity { get; set; }

    // override object.Equals
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return this.Name == (obj as BulletPattern).Name;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
