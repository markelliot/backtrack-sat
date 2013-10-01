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
	class MainClass
	{
		public static void Main(string[] args)
		{
			bool x;
			DateTime start;
			TimeSpan btdur, dpdur, gsdur, ttime;
			int m = 25; // all our Sat3s are 25 vars.
			int i,j, btcnt, dpcnt, gscnt;
			float alpha;
			
			Search bts;
			DPSearch dps;
			GSAT gst;
			
			bool btsrun = false;
			bool dpsrun = true;
			bool gstrun = true;
			
			int f = 0;
			int r = 0;
			f = int.Parse(args[0]);
			r = int.Parse(args[1]);
			
			
			System.IO.TextWriter sw = new System.IO.StreamWriter("myfile.txt", false);
			
			/*
			bts = new Search();
			bts.LoadFile("./Sat3/n25m25i71.txt");
			Console.WriteLine(bts.Run());
			*/
			
			sw.WriteLine("file, alpha, satisfiable, dp time, bts time");
			for(i = 0; i < 12; i++){
				btcnt = 0;
				dpcnt = 0;
				gscnt = 0;
				btdur = DateTime.Now - DateTime.Now; // 0 time
				dpdur = DateTime.Now - DateTime.Now; // 0 time
				gsdur = DateTime.Now - DateTime.Now; // 0 time
				for(j = 0; j < 75; j++){
					alpha = 0;
					if(dpsrun){
						dps = new DPSearch();
						alpha = dps.LoadFile("./Sat3/n" + ( m*(i/2+1) ) + "m" + m + "i" + j + ".txt");
						
						start = DateTime.Now;
						x = dps.Run();
						ttime = DateTime.Now - start;						
						dpdur += ttime; // add to total time
						if(x){ dpcnt++; }       // increment number of satisfiable
						
						sw.Write(string.Format("{0}, {1}, {2}, {3}",
						                       "n" + ( m*(i/2+1) ) + "m" + m + "i" + j + ".txt",
						                       alpha,
						                       x,
						                       ttime));
					}
					
					if(btsrun){
						bts = new Search();
						bts.LoadFile("./Sat3/n" + ( m*(i/2+1) ) + "m" + m + "i" + j + ".txt");
						
						start = DateTime.Now;
						x = bts.Run();
						ttime = DateTime.Now - start;
						btdur += ttime; // add to total time
						if(x){ btcnt++; }       // increment number of satisfiable
						sw.Write(string.Format(", {0}\n", ttime));
					}
					
					if(gstrun){
						gst = new GSAT();
						gst.LoadFile("./Sat3/n" + ( m*(i/2+1) ) + "m" + m + "i" + j + ".txt");
						
						start = DateTime.Now;
						x = gst.Run(f, r);
						ttime = DateTime.Now - start;
						gsdur += ttime; // add to total time
						if(x){ gscnt++; }       // increment number of satisfiable
						sw.Write(string.Format(", {1}, {0}\n", ttime, x));
					}
					
					bts = null;
					dps = null;
					gst = null;
				}
				// for every alpha:
				Console.WriteLine("alpha = " + ((float)i/2+1));
				Console.WriteLine("satis (BT) = " + btcnt + "/75");
				Console.WriteLine("satis (DP) = " + dpcnt + "/75"); // these BETTER be equal
				Console.WriteLine("satis (GS) = " + gscnt + "/75");
				Console.WriteLine("BT time " + btdur);
				Console.WriteLine("DP time " + dpdur);
				Console.WriteLine("GS time " + gsdur);
			} // */
			sw.Close();
		}
	}
}
