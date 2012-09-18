using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeometricRecognizer 
{
		//--- These are variables because C++ doesn't (easily) allow
		//---  constants to be floating point numbers
	protected	double halfDiagonal;
	
	protected	double angleRange;
	
	protected	double anglePrecision;
	
	protected	double goldenRatio;

		//--- How many points we use to define a shape
	protected	int numPointsInGesture;
		//---- Square we resize the shapes to
	protected	int squareSize;
		
	protected	bool shouldIgnoreRotation;

		//--- What we match the input shape against
	protected	List<GestureTemplate> templates = new List<GestureTemplate>();

	public	GeometricRecognizer()
	{
		//--- How many templates do we have to compare the user's gesture against?
		//--- Can get ~97% accuracy with just one template per symbol to recognize
		//numTemplates = 16;
		//--- How many points do we use to represent a gesture
		//--- Best results between 32-256
		numPointsInGesture = 20;//128;
		//--- Before matching, we stretch the symbol across a square
		//--- That way we don't have to worry about the symbol the user drew
		//---  being smaller or larger than the one in the template
		squareSize = 50;
		//--- 1/2 max distance across a square, which is the maximum distance
		//---  a point can be from the center of the gesture
		halfDiagonal = 0.5f * Mathf.Sqrt((250.0f * 250.0f) + (250.0f * 250.0f));
		//--- Before matching, we rotate the symbol the user drew so that the 
		//---  start point is at degree 0 (right side of symbol). That's how 
		//---  the templates are rotated so it makes matching easier
		//--- Note: this assumes we want symbols to be rotation-invariant, 
		//---  which we might not want. Using this, we can't tell the difference
		//---  between squares and diamonds (which is just a rotated square)
		setRotationInvariance(false);
		anglePrecision = 2.0f;
		//--- A magic number used in pre-processing the symbols
		goldenRatio    = 0.5f * (-1.0f + Mathf.Sqrt(5.0f));
	}

	public	int addTemplate(string name, List<Point2D> points)
	{
		points = normalizePath(points);
		
		GestureTemplate tmp = new GestureTemplate(name, points);
		
		templates.Add(tmp);

		//--- Let them know how many examples of this template we have now
		int numInstancesOfGesture = 0;
		// You know, i don't care so i'm just going to ignore this
		//for (var i = 0; i < templates.size(); i++)
		//{
		//	if (templates[i].Name == name)
		//		numInstancesOfGesture++;
		//}
		return numInstancesOfGesture;
	}
	
	public	Rectangle boundingBox(List<Point2D> points)
	{
		double minX =  double.MaxValue;
		double maxX = -double.MaxValue;
		double minY =  double.MaxValue; 
		double maxY = -double.MaxValue;

		foreach (Point2D point in points)
		{
			if (point.x < minX)
				minX = point.x;
			if (point.x > maxX)
				maxX = point.x;
			if (point.y < minY)
				minY = point.y;
			if (point.y > maxY)
				maxY = point.y;
		}
		Rectangle bounds = new Rectangle(minX, minY, (maxX - minX), (maxY - minY));
		
		return bounds;
	}
	
	public	Point2D centroid(List<Point2D> points)
	{
		double x = 0.0, y = 0.0;
		foreach (Point2D point in points)
		{
			x += point.x;
			y += point.y;
		}
		x = x/points.Count;
		y = y/points.Count;
		Point2D ret = new Point2D(x, y);
		return ret;
	}
	
	public	double getDistance(Point2D p1, Point2D p2)
	{
		double dx = p2.x - p1.x;
		double dy = p2.y - p1.y;
		double distance = Mathf.Sqrt((float)((dx * dx) + (dy * dy)));
		return distance;
	}
	
	public	bool   getRotationInvariance() 
	{
		return shouldIgnoreRotation; 
	}
	
	public	double distanceAtAngle(List<Point2D> points, GestureTemplate aTemplate, double rotation)
	{
		List<Point2D> newPoints = rotateBy(points, rotation);
		return pathDistance(newPoints, aTemplate.points);
	}
	
	public	double distanceAtBestAngle(List<Point2D> points, GestureTemplate aTemplate)
	{
		double startRange = -angleRange;
		double endRange   =  angleRange;
		//Debug.LogWarning("anglerange:  "+angleRange);
		double x1 = goldenRatio * startRange + (1.0f - goldenRatio) * endRange;
		double f1 = distanceAtAngle(points, aTemplate, x1);
		double x2 = (1.0f - goldenRatio) * startRange + goldenRatio * endRange;
		double f2 = distanceAtAngle(points, aTemplate, x2);
		while (Mathf.Abs((float)(endRange - startRange)) > anglePrecision)
		{
			if (f1 < f2)
			{
				endRange = x2;
				x2 = x1;
				f2 = f1;
				x1 = goldenRatio * startRange + (1.0f - goldenRatio) * endRange;
				f1 = distanceAtAngle(points, aTemplate, x1);
			}
			else
			{
				startRange = x1;
				x1 = x2;
				f1 = f2;
				x2 = (1.0f - goldenRatio) * startRange + goldenRatio * endRange;
				f2 = distanceAtAngle(points, aTemplate, x2);
			}
		}
		return Mathf.Min((float)f1, (float)f2);
	}
	
	public	List<Point2D> normalizePath(List<Point2D> points)
	{
		/* Recognition algorithm from 
			http://faculty.washington.edu/wobbrock/pubs/uist-07.1.pdf
			Step 1: Resample the Point Path
			Step 2: Rotate Once Based on the "Indicative Angle"
			Step 3: Scale and Translate
			Step 4: Find the Optimal Angle for the Best Score
		*/
		// TODO: Switch to $N algorithm so can handle 1D shapes

		//--- Make everyone have the same number of points (anchor points)
		points = resample(points);
		//--- Pretend that all gestures began moving from right hand side
		//---  (degree 0). Makes matching two items easier if they're
		//---  rotated the same
		if (getRotationInvariance())
			points = rotateToZero(points);
		//--- Pretend all shapes are the same size. 
		//--- Note that since this is a square, our new shape probably
		//---  won't be the same aspect ratio
		points = scaleToSquare(points);
		//--- Move the shape until its center is at 0,0 so that everyone
		//---  is in the same coordinate system
		points = translateToOrigin(points);

		return points;
	}
	
	public	double pathDistance(List<Point2D> pts1, List<Point2D> pts2)
	{
		double distance = 0.0f;
		
		for (int i = 0; i < (int)pts1.Count; i++) 
			distance += getDistance(pts1[i], pts2[i]);
		return (distance / pts1.Count);
	}
	
	public	double pathLength(List<Point2D> points)
	{
		double distance = 0;
		for (int i = 1; i < (int)points.Count; i++)
			distance += getDistance(points[i - 1], points[i]);
		return distance;
	}
	
	public	RecognitionResult recognize(List<Point2D> points)
	{
		//--- Make sure we have some templates to compare this to
		//---  or else recognition will be impossible
		if (templates== null || templates.Count==0)
		{
			Debug.LogWarning("No templates loaded so no symbols to match.");
			return new RecognitionResult("Unknown", 0);
		}

		points = normalizePath(points);
	
		//--- Initialize best distance to the largest possible number
		//--- That way everything will be better than that
		double bestDistance = double.MaxValue;
		//--- We haven't found a good match yet
		int indexOfBestMatch = -1;

		//--- Check the shape passed in against every shape in our database
		for (int i = 0; i < (int)templates.Count; i++)
		{
			//--- Calculate the total distance of each point in the passed in
			//---  shape against the corresponding point in the template
			//--- We'll rotate the shape a few degrees in each direction to
			//---  see if that produces a better match
			double distance = distanceAtBestAngle(points, templates[i]);
			if (distance < bestDistance)
			{
				bestDistance     = distance;
				indexOfBestMatch = i;
			}
		}

		//--- Turn the distance into a percentage by dividing it by 
		//---  half the maximum possible distance (across the diagonal 
		//---  of the square we scaled everything too)
		//--- Distance = hwo different they are
		//--- Subtract that from 1 (100%) to get the similarity
		double score = 1.0f - (bestDistance / halfDiagonal);

		//--- Make sure we actually found a good match
		//--- Sometimes we don't, like when the user doesn't draw enough points
		if (-1 == indexOfBestMatch)
		{
			Debug.LogWarning("Couldn't find a good match.");
			return new RecognitionResult("Unknown", 1);
		}

		RecognitionResult bestMatch = new RecognitionResult(templates[indexOfBestMatch].name, score);
		return bestMatch;
	}
	
	public	List<Point2D> resample(List<Point2D> points)
	{
		double interval = pathLength(points) / (numPointsInGesture - 1); // interval length
		double D = 0.0f;
		List<Point2D> newPoints = new List<Point2D>();

		//--- Store first point since we'll never resample it out of existence
		newPoints.Add(points[0]);
	    for(int i = 1; i < (int)points.Count; i++)
		{
			Point2D currentPoint  = points[i];
			Point2D previousPoint = points[i-1];
			double d = getDistance(previousPoint, currentPoint);
			if ((D + d) >= interval)
			{
				double qx = previousPoint.x + ((interval - D) / d) * (currentPoint.x - previousPoint.x);
				double qy = previousPoint.y + ((interval - D) / d) * (currentPoint.y - previousPoint.y);
				Point2D point = new Point2D(qx, qy);
				newPoints.Add(point);
				points.Insert(i, point);// Check it points.Insert(points.begin() + i, point);
				D = 0.0f;
			}
			else D += d;
		}

		// somtimes we fall a rounding-error short of adding the last point, so add it if so
		if (newPoints.Count == (numPointsInGesture - 1))
		{
			newPoints.Add(points[points.Count-1]);
		}

		return newPoints;
	}
	
	public	List<Point2D> rotateBy(List<Point2D> points, double rotation)
	{
		Point2D c     = centroid(points);
		//--- can't name cos; creates compiler error since VC++ can't
		//---  tell the difference between the variable and function
		double cosine = Mathf.Cos((float)rotation);	
		double sine   = Mathf.Sin((float)rotation);
		
		List<Point2D> newPoints = new List<Point2D>();
		foreach (Point2D point in points)
		{
			double qx = (point.x - c.x) * cosine - (point.y - c.y) * sine   + c.x;
			double qy = (point.x - c.x) * sine   + (point.y - c.y) * cosine + c.y;
			newPoints.Add(new Point2D(qx, qy));
		}
		return newPoints;
	}
	
	public	List<Point2D> rotateToZero(List<Point2D> points)
	{
		Point2D c = centroid(points);
		double rotation = Mathf.Atan2((float)(c.y - points[0].y), (float)(c.x - points[0].x));
		return rotateBy(points, -rotation);
	}
	
	public	List<Point2D> scaleToSquare(List<Point2D> points)
	{
		//--- Figure out the smallest box that can contain the path
		Rectangle box = boundingBox(points);
		List<Point2D> newPoints = new List<Point2D>();
		foreach (Point2D point in points)
		{
			//--- Scale the points to fit the main box
			//--- So if we wanted everything 100x100 and this was 50x50,
			//---  we'd multiply every point by 2
			double scaledX = point.x * (squareSize / box.width);
			double scaledY = point.y * (squareSize / box.height);
			//--- Why are we adding them to a new list rather than 
			//---  just scaling them in-place?
			// TODO: try scaling in place (once you know this way works)
			newPoints.Add(new Point2D(scaledX, scaledY));
		}
		return newPoints;
	}
	
	public	void   setRotationInvariance(bool ignoreRotation)
	{
		shouldIgnoreRotation = ignoreRotation;

		if (shouldIgnoreRotation)
		{
			angleRange = 15.0f;//45.0f;
		}
		else
		{
			Debug.LogWarning(" setRotationInvariance   angleRange = 15.0f;  ignoreRotation:  "+ignoreRotation);
			angleRange = 0.0f;//15.0f;
		}
	}
	
	public	List<Point2D> translateToOrigin(List<Point2D> points)
	{
		Point2D c = centroid(points);
		List<Point2D> newPoints = new List<Point2D>();
		foreach (Point2D point in points)
		{
			double qx = point.x - c.x;
			double qy = point.y - c.y;
			newPoints.Add(new Point2D(qx, qy));
		}
		return newPoints;
	}

	public	void loadTemplates()
	{
		SampleGestures samples = new SampleGestures();

		//addTemplate("Arrow", samples.getGestureArrow());
		//addTemplate("Caret", samples.getGestureCaret());
		//addTemplate("CheckMark", samples.getGestureCheckMark());
		//addTemplate("Circle", samples.getGestureCircle());
		//addTemplate("Delete", samples.getGestureDelete());
		//addTemplate("Diamond", samples.getGestureDiamond());
		//addTemplate("LeftCurlyBrace", samples.getGestureLeftCurlyBrace());
		//addTemplate("LeftSquareBracket", samples.getGestureLeftSquareBracket());
		//addTemplate("LeftToRightLine", samples.getGestureLeftToRightLine());
		//addTemplate("LineDownDiagonal", samples.getGestureLineDownDiagonal());
		//addTemplate("Pigtail", samples.getGesturePigtail());
		//addTemplate("QuestionMark", samples.getGestureQuestionMark());
		addTemplate("Rectangle", samples.getGestureRectangle());
		addTemplate("Rectangle_1", samples.getGestureRectangle_1());
		addTemplate("Rectangle_2", samples.getGestureRectangle_2());
		addTemplate("Rectangle_3", samples.getGestureRectangle_3());
		addTemplate("Rectangle_4", samples.getGestureRectangle_4());
		addTemplate("Rectangle_5", samples.getGestureRectangle_5());
		addTemplate("Rectangle_6", samples.getGestureRectangle_6());
		addTemplate("Rectangle_7", samples.getGestureRectangle_7());

        addTemplate("UpLeft",   samples.getGestureUpLeft());
        addTemplate("UpRight",  samples.getGestureUpRight());
        addTemplate("DownLeft", samples.getGestureDownLeft());
        addTemplate("DownRight",samples.getGestureDownRight());
        addTemplate("LeftUp",   samples.getGestureLeftUp());
        addTemplate("LeftDown", samples.getGestureLeftDown());
        addTemplate("RightUp",  samples.getGestureRightUp());
        addTemplate("RightDown",samples.getGestureRightDown());
		
		addTemplate("Triangle", samples.getGestureTriangle());
		addTemplate("Triangle_1", samples.getGestureTriangle_1());
		addTemplate("Triangle_2", samples.getGestureTriangle_2());
		addTemplate("Triangle_3", samples.getGestureTriangle_3());
		addTemplate("Triangle_4", samples.getGestureTriangle_4());
		addTemplate("Triangle_5", samples.getGestureTriangle_5());
		

		//addTemplate("RightCurlyBrace", samples.getGestureRightCurlyBrace());
		//addTemplate("RightSquareBracket", samples.getGestureRightSquareBracket());
		//addTemplate("RightToLeftLine", samples.getGestureRightToLeftLine());
		//addTemplate("RightToLeftLine2", samples.getGestureRightToLeftLine2());
		//addTemplate("RightToLeftSlashDown", samples.getGestureRightToLeftSlashDown());
		//addTemplate("Spiral", samples.getGestureSpiral());
		//addTemplate("Star", samples.getGestureStar());
		//addTemplate("Triangle", samples.getGestureTriangle());
		//addTemplate("V", samples.getGestureV());
		//addTemplate("X", samples.getGestureX());
	}
}
