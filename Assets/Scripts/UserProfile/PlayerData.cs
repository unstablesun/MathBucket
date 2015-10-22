[System.Serializable]

public class PlayerData {

	public string userId = "";
	public string userNickname = "";
	public int bestScore = 0;
	public int coins = 0;

	public PlayerData Clone() {
		PlayerData playerData = new PlayerData ();
		playerData.userId = this.userId;
		playerData.userNickname = this.userNickname;
		playerData.bestScore = this.bestScore;
		playerData.coins = this.coins;
		return playerData;
	}
}
