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
	/**
	 * Holds a "symbolic" 3-SAT cluase in memory
	 */
	public class Clause
	{
		public int  c1, c2, c3;  // store variable #
		public bool f1, f2, f3;  // store inverted
		
		/**
		 * int c1p First variable of clause [1,m]
		 *     c2p Second                   [1,m]
		 *     c3p Third                    [1,m]
		 */
		public Clause(int c1p, int c2p, int c3p)
		{
			c1 = Math.Abs(c1p);
			c2 = Math.Abs(c2p);
			c3 = Math.Abs(c3p);
			f1 = (c1p < 0);
			f2 = (c2p < 0);
			f3 = (c3p < 0);			
		}
		
		public bool Eval(bool[] x)
		{
			return ((!f1 == x[c1-1]) || (!f2 == x[c2-1]) || (!f3 == x[c3-1]));
		}
		
		/**
		 * bool[] x   Vector of assignments [1,m] indexed as [0,m-1]
		 * bool[] a   Vector of what variables are assigned indexed as above
		 * 
		 * returns 1  True
		 *        -1  Never true
		 *         0  False
		 */
		public int Eval(bool[] x, bool[] a)
		{
			bool resp = false;
			resp = (!f1 == x[c1-1]) || (!f2 == x[c2-1]) || (!f3 == x[c3-1]);
			if(!resp && a[c1-1] && a[c2-1] && a[c3-1]){
				//Console.WriteLine(this.ToString());
				return (resp) ? 1 : -1;
			}
			return (resp) ? 1 : 0;
		}
		
		override public string ToString()
		{
			return string.Format("{0}{1} {2}{3} {4}{5}", (f1) ? "-" : "", c1,
			                     (f2) ? "-" : "", c2,
			                     (f3) ? "-" : "", c3);
		}
		
		/**
		 * A unit clause is one that already has two variables
		 * assigned falsely, and a third value that does not have
		 * an assignment.
		 * 
		 * bool[] a - variables are assigned
		 * 
		 * return 0    Not a unitclause - vars not fully assigned
		 *                                or expression is true
		 *        int  Signed variable representation 
		 */
		public int UnitClause(bool[] x, bool[] a)
		{
			// not a unit clause if it is already true.
			if(a[c1-1] && a[c2-1] && !a[c3-1]){
				if((f1 == x[c1-1]) && (f2 == x[c2-1])){
					return (f3) ? -c3 : c3;
				}
			}else if(a[c1-1] && a[c3-1] && !a[c2-1]){
				if((f1 == x[c1-1]) && (f3 == x[c3-1])){
					return (f2) ? -c2 : c2;
				}
			}else if(a[c2-1] && a[c3-1] && !a[c1-1]){
				if((f2 == x[c2-1]) && (f3 == x[c3-1])){
					return (f1) ? -c1 : c1;
				}
			}
			return 0;
		}
	}
}
