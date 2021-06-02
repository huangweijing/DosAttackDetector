/*
 * Created by SharpDevelop.
 * User: HuangWeijing
 * Date: 2021/6/1
 * Time: 17:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;

namespace DOSAttackChecker
{
	class Program
	{
		public static void Main(string[] args)
		{
			try{
				
				Dictionary<String, Int32> prevConMap = generateTcpConnectionMap();
				
				while(true) {
					Thread.Sleep(10000);
					Dictionary<String, Int32> currConMap = generateTcpConnectionMap();
					Dictionary<String, Int32> result = compareTcpConnectionMap(prevConMap, currConMap);
					
					Boolean dosAttackDetected = false;
					foreach(String key in result.Keys) {
						Console.WriteLine(key + "     " + result[key]);
						if(result[key] > 10) {
							dosAttackDetected = true;
						}
					}
					
					if(dosAttackDetected) {
						Console.WriteLine("DOS ATTACKを検知しました！");
					}
					
					Console.WriteLine("-------------------------------");
					
					prevConMap = currConMap;
				}
				
			} catch(Exception e) {
				Console.WriteLine(e);
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		/// <summary>
		/// 現在接続中のＴＣＰリモートポート一覧を取得し、リストで返す
		/// </summary>
		/// <returns></returns>
		public static Dictionary<String, Int32> generateTcpConnectionMap() {
			Dictionary<String, Int32> connectionMap = new Dictionary<String, Int32>();
			//現在接続中の接続先とポート一覧を取得する
			TcpConnectionInformation[] connectionArr = 
				IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
			foreach(TcpConnectionInformation connection in connectionArr) {
				String key = connection.RemoteEndPoint.Address.ToString() + ":" + connection.RemoteEndPoint.Port.ToString();
				if(!connectionMap.ContainsKey(key)) {
					connectionMap[key] = 1;
				} else {
					connectionMap[key] = (Int32)connectionMap[key] + 1;
				}
			}
			return connectionMap;
		}
		
		public static Dictionary<String, Int32> compareTcpConnectionMap(
			Dictionary<String, Int32> map1, Dictionary<String, Int32> map2) {
			Dictionary<String, Int32> resultMap = new Dictionary<String, Int32>();
			foreach(String key in map1.Keys) {
				resultMap[key] = 0;
			}
			foreach(String key in map2.Keys) {
				resultMap[key] = 0;
			}
			for(int i=0; i<resultMap.Keys.Count; i++) {
				
			}
			foreach(String key in new List<String>(resultMap.Keys)) {
				Int32 map1ConCount = 0;;
				Int32 map2ConCount = 0;
				if(map1.ContainsKey(key)) {
					map1ConCount = map1[key];
				}
				if(map2.ContainsKey(key)) {
					map2ConCount = map2[key];
				}
				Int32 diff = map2ConCount - map1ConCount;
				resultMap[key] = diff;
			}
			
			foreach(String key in new List<String>(resultMap.Keys)) {
				if((Int32)resultMap[key] == 0) {
					resultMap.Remove(key);
				}
			}
			return resultMap;
		}
		
		
	}
}