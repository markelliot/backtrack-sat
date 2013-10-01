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

namespace satgen
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// alpha = n/m
			Gen3 my_gen;
			
			// We want alpha [1,6]
			// Using preliminary estimates, my program can evaluate 10 variables
			// in approximately 0.0001s.  This scales exponentially, so we choose
			// n as something fairly large (let's say 1000).  This means we want
			// to vary 1000/m between [1,6], or rather, m = 1000/[1,6].
			//
			// For each case, we want to have some number of the same file
			// to take out luckiness (cap is 100, for now we'll do 50).
		
			int m,i,j;
			m = 25;
			j = 50;
			
			for(i = 0; i < 12; i++)
			{
				for(j = 0; j < 75; j++)
				{
					// used to be (i+1) + "/"
					my_gen = new Gen3(m * ((i/2)+1), m, j, "Sat3/");
				}
			}
		}
	}
}
