[System.Serializable]

public class User {

	public PlayerData player;

	//settings
	public bool soundEnabled = true;
	public bool soundFXEnabled = true;
	public bool notificationsEnabled = true;

	//multiplayer vars
	public string lastTournamentID = "";
	public string lastMatchID = "";

	public User() {
		this.player = new PlayerData ();
	}

	public User Clone() {
		User user = new User ();
		user.player = this.player.Clone ();
		user.soundEnabled = this.soundEnabled;
		user.soundFXEnabled = this.soundFXEnabled;
		user.notificationsEnabled = this.notificationsEnabled;
		user.lastTournamentID = this.lastTournamentID;
		user.lastMatchID = this.lastMatchID;
		return user;
	}
}