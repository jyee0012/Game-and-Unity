[System.Serializable]
public class Profile
{
    public string profileName;
    public int highScore;

    public void SetName(string name)
    {
        profileName = name;
    }
    public void SetScore(int score)
    {
        highScore = score;
    }
    public string GetName()
    {
        return profileName;
    }
    public int GetScore()
    {
        return highScore;
    }
}
