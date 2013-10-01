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

namespace BackTrackSat
{
	public class DPSearch
	{
		int m, // vars
		    n; // clauses
		Clause[] c;
		bool[] final;
		
		public bool[] EmptyXArray()
		{
			int i;
			bool[] x = new bool[m];
			for(i = 0; i < m; i++)
			{
				x[i] = false;
			}
			return x;
		}
		
		public bool Run()
		{
			bool[] x;
			bool[] a;
			x = this.EmptyXArray();
			a = this.EmptyXArray();
			return this.Run(x, a, 0);
		}
		
		/**
		 * bool[] x  variable assignment
		 * bool[] a  assigned variable?
		 * int    i  number of variables assigned
		 */
		public bool Run(bool[] x, bool[] a, int i)
		{
			int res, unit, difi;
			res = Truth(x, a);
			if(res < 0){     // early termination
				return false;
			}
			if(res > 0){
				this.final = x;
				return true; // if this is a satisfying assignment, stop
			}
			if(i < m){
				// does a unit clause exist?
				if((unit = FindUnitClause(x, a)) != 0){
					// found a unit clause, set var
					a[Math.Abs(unit)-1] = true;
					x[Math.Abs(unit)-1] = (unit > 0);
					if( Run(x, a, i+1) )
					{
						return true;
					}else{
						a[Math.Abs(unit)-1] = false;
						return false;
					}
				}
				//Console.WriteLine(i + " " + PrintX(x));
				difi = GetFirstUnassigned(a); // gets an unset var.
				//if(difi == -1){ return false; }
				difi = i;
				a[difi] = true;
				x[difi] = false;
				if( Run(x, a, i+1) ){ return true; }
				// if we get here, we need to flip the var.
				x[difi] = true;
				if( Run(x, a, i+1) ){ return true; }else{ a[difi] = false; return false; }
			}
			return false;
		}
		
		public bool[] Final()
		{
			return this.final;
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
		 * returns alpha
		 * 
		 * Post:  Populates m, n, c and inits x
		 */
		public float LoadFile(string filepath)
		{
			int i;
			String my_line; // storage for reading a line.
			String[] my_expl; // explosion storage
			
			int vars;
			bool[] var_used;
			
			try{
				System.IO.StreamReader sr = System.IO.File.OpenText(filepath);
				
				my_line = sr.ReadLine();
				// first line contains some very necessary vars (namely #clauses, #vars)
				my_expl = my_line.Split(' ');
				m = int.Parse(my_expl[2]);
				n = int.Parse(my_expl[3]);
	
				c = new Clause[n]; // n clauses
				var_used = new bool[m];
				
				vars = 0;
				for(i = 0; i < m; i++){ var_used[i] = false; }
				
				i = 0;
				
				my_line = sr.ReadLine();
				while( my_line != null )
				{
					my_expl = my_line.Split(' ');
					c[i] = new Clause(int.Parse(my_expl[0]), 
					                  int.Parse(my_expl[1]), 
					                  int.Parse(my_expl[2]));
					
					if(!var_used[c[i].c1-1]){
						var_used[c[i].c1-1] = true;
						vars++;
					}
					if(!var_used[c[i].c2-1]){
						var_used[c[i].c2-1] = true;
						vars++;
					}
					if(!var_used[c[i].c3-1]){
						var_used[c[i].c3-1] = true;
						vars++;
					}
					
					my_line = sr.ReadLine();
					i++;
				}
				sr.Close();
				return (float)n / (float)vars;
			}catch(Exception e){
				Console.WriteLine("" + e.Message);
			}
			return 0;
		}
		
		int Truth(bool[] x, bool[] a)
		{
			int i, r;
			for(i = 0; i < n; i++)
			{
				r = c[i].Eval(x, a);
				if(r == 0){ return 0; }
				if(r == -1){ return -1; }
			}
			return 1;
		}
		
		int FindUnitClause(bool[] x, bool[] a)
		{
			int i, r;
			for(i = 0; i < n; i++)
			{
				r = c[i].UnitClause(x, a);
				if(r != 0){ return r; }
			}
			return 0;
		}
		
		static int GetFirstUnassigned(bool[] a)
		{
			int i;
			for(i = 0; i < a.Length; i++)
			{
				if(!a[i]) return i;
			}
			return -1;
		}
	}
}
