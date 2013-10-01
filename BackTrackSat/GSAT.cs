/* Copyright 2006 Mark Elliot
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;

namespace BackTrackSat
{
	public class GSAT
	{
		int m, // vars
		    n; // clauses
		Clause[] c;
		List<Clause>[] cv;
		int[] gains;
		
		public bool[] EmptyXArray()
		{
			int i;
			Random rand = new Random();
			bool[] x = new bool[m];
			for(i = 0; i < m; i++)
			{
				x[i] = (rand.NextDouble() <= 0.5) ? false : true;
			}
			return x;
		}
		
		public bool Run(int maxflips, int retries)
		{
			int i, j, k, g, sat, flip, max;
			int[] temp_gain = new int[4];
			List<int> vars = new List<int>();
			Random rand = new Random();
			bool[] x;
			for(j = 0; j < retries; j++)
			{
				x = this.EmptyXArray();
				for(g = 0; g < m; g++)
				{
					gains[g] = 0;
				}
				for(i = 0; i < maxflips; i++)
				{
					// are we satisfied?
					sat = Truth(x);
					if(sat == n){ return true; }
					vars.Clear();
					max = 0;
					for(k = 0; k < m; k++){
						if(cv[k] != null){
							gains[k] = 0;
							//Console.WriteLine("vars for " + k);
							foreach(Clause myc in cv[k]){
								gains[k] += CalcGain(myc, x, k);
							}
							//Console.WriteLine(gains[k] + "/" + max);
							if(gains[k] > max){
								max = gains[k];
								vars.Clear();
								vars.Add(k);
							}
							if(gains[k] == max){
								vars.Add(k);
							}
						}
					}
					flip = rand.Next(0,vars.Count-1);
					x[flip] = !x[flip]; // flip!
				}
			}
			return false;
		}
		
		int CalcGain(Clause myc, bool[] x, int k)
		{
			int gain = 0;
			if(!myc.Eval(x)){
				x[k] = !x[k];
				if(myc.Eval(x)){ gain = 1; }
			}else{
				x[k] = !x[k];
				if(!myc.Eval(x)){ gain = -1; }
			}
			x[k] = !x[k];
			return gain;
		}
		
		// this is the "naive" GSAT
		/*
		public bool Run(int maxflips, int retries)
		{
			int i, j, k, max, sat, flip;
			List<int> vars = new List<int>();
			Random rand = new Random();
			
			bool[] x = this.EmptyXArray();
			for(j = 0; j < retries; j++)
			{
				for(i = 0; i < maxflips; i++)
				{
					// are we satisfied?
					sat = Truth(x);
					if(sat == n){ return true; }

					// find flip that will change the most
					max = 0;
					vars.Clear();
					for(k = 0; k < m; k++){
						x[k] = !x[k];   // flip
						sat = Truth(x); // test
						if(sat > max){
							max = sat;
							vars.Clear();
							vars.Add(k);
						}
						if(sat == max){
							vars.Add(k);
						}
						x[k] = !x[k];   // flip back
					}
					flip = rand.Next(0,vars.Count-1);
					x[flip] = !x[flip]; // flip!
				}
			}
			return false;
		}*/
		
		int Truth(bool[] x)
		{
			int i, t;
			t = 0;
			for(i = 0; i < n; i++)
			{
				if(c[i] != null && c[i].Eval(x)){ t++; }
			}
			return t;
		}
		
		public string PrintX(bool[] x)
		{
			int i;
			string output = "";
			for(i = 0; i < x.Length; i++)
			{
				output += (x[i]) ? "1" : "0";
			}
			return output;
		}
		
		/**
		 * string filepath Path to definition file
		 * 
		 * Post:  Populates m, n, c and inits x
		 */
		public void LoadFile(string filepath)
		{
			int i;
			String my_line; // storage for reading a line.
			String[] my_expl; // explosion storage
			
			try{
				System.IO.StreamReader sr = System.IO.File.OpenText(filepath);
				
				my_line = sr.ReadLine();
				// first line contains some very necessary vars (namely #clauses, #vars)
				my_expl = my_line.Split(' ');
				m = int.Parse(my_expl[2]);
				n = int.Parse(my_expl[3]);
	
				c = new Clause[n]; // n clauses
				cv = new List<Clause>[m]; // need m of these bins for clauses
				gains = new int[m]; // need m gains.
				
				for(i = 0; i < m; i++)
				{
					cv[i] = new List<Clause>();
				}
				
				i = 0;
						
				my_line = sr.ReadLine();
				while( my_line != null )
				{
					my_expl = my_line.Split(' ');
					c[i] = new Clause(int.Parse(my_expl[0]), 
					                  int.Parse(my_expl[1]), 
					                  int.Parse(my_expl[2]));
					
					cv[c[i].c1-1].Add(c[i]);
					cv[c[i].c2-1].Add(c[i]);
					cv[c[i].c3-1].Add(c[i]);
					
					//Console.WriteLine(cv[c[i].c1 - 1].Count);
										
					my_line = sr.ReadLine();
					i++;
				}
				
				sr.Close();
			}catch(Exception e){
				Console.WriteLine(e.Message);
			}
		}
	}
}
