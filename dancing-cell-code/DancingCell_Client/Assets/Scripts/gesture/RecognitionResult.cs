using UnityEngine;
using System.Collections;

public class RecognitionResult 
{
	public string name;
	
	public double score;
	
	public RecognitionResult(string _name, double _score)
	{
		name = _name;
		
		score = _score;
	}
	
	public void Print()
	{
		Debug.LogWarning("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$==============>  Drop mouse: restult:  name: "+name+"  score:  "+score);
	}
}
