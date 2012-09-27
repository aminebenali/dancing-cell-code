using UnityEngine;
using System.Collections;

public class Point2D 
{
	//--- Wobbrock used doubles for these, not ints
	//int x, y;
	public double x;
	
	public double y;
	
	public	Point2D() 
	{
		x = 0; 
		y = 0;
	}
	public Point2D(double _x, double _y)
	{
		x = _x;
		y = _y;
	}
	
	public string GetString()
	{
		return "("+x+","+y+")";
	}

    public Vector3 ToVector3()
    {
        return new Vector3((float)x,(float)y,0);
    }
}
