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

namespace satgen
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class Gen3
	{
		// alpha = n/m;
		public Gen3(int clauses, int vars, int inst, string pref)
		{
			int n;
			int a,b,c;
			
			System.IO.Directory.CreateDirectory(pref);
			
			System.IO.TextWriter tw = new System.IO.StreamWriter(pref + "n"+clauses+"m"+vars+"i"+inst+".txt", false);
			Random r1 = new Random();
			Random r2 = new Random(inst * clauses);
			
			// init file
			tw.WriteLine(string.Format("p cnf {0} {1}", vars, clauses));
			
			// for every clause
			for(n = 0; n < clauses; n++)
			{
				/* We need to choose 3 numbers, these three statements utilize a function
				 * to ensure that hte numbers are distinct.  Initially, Next() was returning
				 * (every execution) clauses containing two of the same number.  While we
				 * don't pick evenly every time, we do not lose generality in that the problem
				 * does not say "Pick 3 numbers with equal probability" it is, "For every
				 * clause with equal probability pick 3 numbers".
				 */
				a = NextNumber(vars, r2, 0, 0);
				b = NextNumber(vars, r2, a, 0);
				c = NextNumber(vars, r2, a, b);
				tw.WriteLine(string.Format("{0}{1} {2}{3} {4}{5} 0", 
				                           MinusSign(r1.NextDouble()), a,
				                           MinusSign(r1.NextDouble()), b,
				                           MinusSign(r1.NextDouble()), c
				                          ));
			}
			tw.Close();
		}
		
		private string MinusSign(double prob)
		{
			return (prob > 0.5) ? "-" : "";
		}
		
		private int NextNumber(int vars, Random r, int a, int b){
			int next;
			do{
				next = r.Next(1, vars+1);
			}while(next == a || next == b);
			return next;
		}
	}
}
