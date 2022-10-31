using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Hex class defines the grid position, world space position, size,
/// neighbours, etc ... of a Hex Tile. However, it does NOT interact with
/// Unity directly in any way.
/// </summary>
internal class Hex
{
    //Q + R + S = 0
    // S = -(Q+R)

    public readonly int Q; //Column
    public readonly int R; //Row
    public readonly int S; //Size

    public Hex(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q+r);
    }
}
