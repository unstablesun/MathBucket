using UnityEngine;
using System.Collections;
using FUEL.Utils;
using FUEL.UserProfile;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FUEL.UserProfile
{
	public class UserProfileManager : Singleton<UserProfileManager> {

		public User currentUser;
		public bool startMultiplayer = false;

		void Awake(){
		}

		void Start() {
		}


		public IEnumerator InitCoroutine() {
			User saveUser = this.LoadUserData();
			if (saveUser == null) {
				this.currentUser = new User();
			} 
			else {
				this.currentUser = saveUser.Clone();
			}

			yield return 0;
		}

		private void SaveUserData(){
			BinaryFormatter bf = new BinaryFormatter();
			
			FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
			bf.Serialize(file,this.currentUser);
			file.Close();
			
		}
		
		private User LoadUserData(){

			if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
				BinaryFormatter bf = new BinaryFormatter();
				
				FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
				User data = (User)bf.Deserialize(file);
				file.Close();

				return data;
			}
			return null;
		}

		public bool SetBestScore( int score ) {
			if ( score > this.currentUser.player.bestScore) {
				this.currentUser.player.bestScore = score;
				return true;
			}
			return false;
		}

		public void DebugResetData(){
			if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
				File.Delete(Application.persistentDataPath + "/playerInfo.dat");
			}
			//this.currentUser.player.bestScore = 0;
			//this.currentUser.player.coins = 0;
		}

		void OnApplicationQuit() {
			this.SaveUserData();
		}

		void OnApplicationPause() {
			this.SaveUserData();
		}
	}
}
