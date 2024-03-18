using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridPosition: IEquatable<GridPosition>
{
    public int x;
    public int z;
    public GridPosition(int x, int z)
    {
        this.x = x; this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }
    public bool Equals(GridPosition other)
    {
        return this==other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return "X: "+x+"; Z: "+z;
    }
    public static bool operator == (GridPosition a, GridPosition b) 
    {
        return a.x==b.x && a.z==b.z;
    }
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a==b);
    }
    public static GridPosition operator + (GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    public static GridPosition operator - (GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x-b.x,a.z-b.z);
    }
    public GridPosition Add(GridPosition other) 
    {
        return Add(other);
    }
    public GridPosition Add(object obj)
    {
        if(obj is GridPosition gridPos)
            return new GridPosition(this.x+gridPos.x,this.z+gridPos.z);
        else
        {
            throw new ArgumentException("Can't add this object to GridPosition");
        }
    }
    public GridPosition Subtract(GridPosition other)
    {
        return Subtract(other);
    }
    public GridPosition Subtract(object obj)
    {
        if (obj is GridPosition gridPos)
            return new GridPosition(this.x - gridPos.x, this.z - gridPos.z);
        else
        {
            throw new ArgumentException("Can't subtract this object from GridPosition");
        }
    }

    
}
