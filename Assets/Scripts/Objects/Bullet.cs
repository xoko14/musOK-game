using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Bullet
{
    public Vector3 Position { get; set; }
    public BulletPattern Pattern { get; set; }

    public Bullet(Vector3 pos, BulletPattern bp)
    {
        Position = pos;
        Pattern = bp;
    }
}

