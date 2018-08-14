using System;
using System.Collections.Generic;
using System.Linq;

namespace Finexio
{
	public class Program
	{
		//Assumption: No duplicate points in input array; n is always greater than 1 (n is bonus work as instructions just wanted 2 points... FYI)
		public static void Main(string[] args)
		{
			Console.Write("Enter points [ex. (0, 0), (1,1), (3,5), (2,2)]:");
			string arr = Console.ReadLine();
			Console.Write("Enter minumum number of points for a line to intercept [ex. 2]:");
			int n = 0;
			try
			{
				n = int.Parse(Console.ReadLine());
			}
			catch (Exception ex)
			{
				Console.WriteLine("Number of points needs to be a real number.  Better luck next time!");
				Console.ReadLine();
				Environment.Exit(0);
			}

			arr = arr.Trim();
			arr = arr.Replace(" ", "");
			arr = arr.Replace("(", "");
			arr = arr.Replace(")", "");
			string[] words = arr.Split(',');
			int strCounter = 0;
			int[, ] coords2 = new int[words.Length / 2, 2];
			try
			{
				for (int i = 0; i < coords2.GetLength(0); i++)
				{
					coords2[i, 0] = int.Parse(words[strCounter].ToString());
					strCounter++;
					coords2[i, 1] = int.Parse(words[strCounter].ToString());
					strCounter++;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Coordinates entered did not match [ex. (0, 0), (1,1), (3,5), (2,2)].  Better luck next time!");
				Console.ReadLine();
				Environment.Exit(0);
			}

			//Get lines for each set of coordinates
			List<string> resultList = GetLines(coords2, n);
			//Return result
			if (resultList != null && resultList.Count > 0)
			{
				foreach (string result in resultList)
				{
					Console.WriteLine(result);
				}

				Console.ReadLine();
			}
			else
			{
				Console.WriteLine("Empty results found.");
				Console.ReadLine();
			}
		}

		public static List<string> GetLines(int[, ] vectors, int n)
		{
			//If number of points < hit requirment (n) or null then return empty results
			if (vectors == null || vectors.GetLength(0) < n || n < 2)
				return null;
			//stores GetSlope results
			string tempLine = string.Empty;
			//results for each iteration
			List<string> tempresult = new List<string>();
			//final result of unique lines with more than n points intersecting
			List<string> result = new List<string>();
			//store (x,y) points for lines that meet n requirement
			List<string> hitPoints = new List<string>(); //every line (m,b) set needs to holds these values separate
			//Loop through all points
			for (int i = 0; i < vectors.GetLength(0); i++)
			{
				//Loops through each subsequent point to get all potential combinations
				for (int j = i + 1; j < vectors.GetLength(0); j++)
				{
					//Console.WriteLine("(" + vectors[i, 0] + ", " + vectors[i, 1] + "), (" + vectors[j, 0] + ", " + vectors[j, 1] + ")");
					tempLine = GetSlope(vectors[i, 0], vectors[i, 1], vectors[j, 0], vectors[j, 1]);
					//Skip if already ran these values
					if (hitPoints.Contains(tempLine + "(" + vectors[i, 0] + ", " + vectors[i, 1] + ")") && hitPoints.Contains(tempLine + "(" + vectors[j, 0] + ", " + vectors[j, 1] + ")"))
					{
						continue;
					}
					else
					{
						//add to list
						tempresult.Add(tempLine);
						//Stores to not use again as part of count
						hitPoints.Add(tempLine + "(" + vectors[i, 0] + ", " + vectors[i, 1] + ")");
						//Stores to not use again as part of count
						hitPoints.Add(tempLine + "(" + vectors[j, 0] + ", " + vectors[j, 1] + ")");
					}
				}
			}

			//Finds duplicate points greater than n hit requirement
			var duplicatePoints = tempresult.GroupBy(s => s).Where(group => (group.Count() > n - 1)).Select(group => group.Key);
			//Adds distinct list of (m,b) matching requirement
			foreach (string str in duplicatePoints)
			{
				result.Add(str);
			}

			return result;
		}

		public static string GetSlope(int x1, int y1, int x2, int y2)
		{
			float m = (((float)(y2 - y1) / (x2 - x1))); //slope
			float b = ((float)(y1 - (m * x1))); //y-intercept
			return "(m=" + Math.Round(m * 100f) / 100f + ", b=" + Math.Round(b * 100f) / 100f + ")";
		}
	}
}
