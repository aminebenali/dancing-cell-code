using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestureTemplate
{
	public string name;
	
	public List<Point2D> points;
	
	public GestureTemplate(string _name, List<Point2D> _points)
	{
		name = _name;
		
		points = _points;
	}
}


