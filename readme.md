![](../main/doc/SharpTrophy.png) # SharpAchievments

Simple achievment system for gamification of applications or games.

**Currently implemented**

*achievment definition
   *groups
   *score
   *ranks
   
*achievment data
  *events
  

**Definition**

```

public class AchievmentGroups
{
    public const string COMBAT = "Combat";
    public const string STEALTH = "Stealth";
}

public class AchievmentNames
{
    public const string PYROMANIC = "Pyromanic";
    public const string EXECUTIONER = "Executioner";

    public const string SNIPER = "Sniper";
    public const string ASSASSIN = "Assassin";
}

public class RankNames
{
    public const string COMPLETED = "Completed";
    public const string BRONZE = "Bronze";
    public const string SILVER = "Silver";
    public const string GOLD = "Gold";
    public const string PLATIN = "Platin";
}
    
    
var achievmentDefinition = new AchievmentDefinition();
achievmentDefinition.Achievments.Add(new Achievment()
{
    Name = AchievmentNames.PYROMANIC,
    Group = AchievmentGroups.COMBAT,
    Ranks = { new Rank()
                    {
                        Name=RankNames.COMPLETED,
                        Score = 20
                    }
            }
});

achievmentDefinition.Achievments.Add(new Achievment()
{
    Name = AchievmentNames.EXECUTIONER,
    Group = AchievmentGroups.COMBAT,
    Ranks = {       new Rank()
                    {   Order = 1,
                        Name=RankNames.BRONZE,
                        Score = 10
                    },
                    new Rank()
                    {   Order = 2,
                        Name=RankNames.SILVER,
                        Score = 20
                    },
                    new Rank()
                    {   Order = 3,
                        Name=RankNames.GOLD,
                        Score = 30
                    },
                    new Rank()
                    {   Order = 4,
                        Name=RankNames.PLATIN,
                        Score = 50
                    }
            }
});
            
```

**Usage**

```
  var achievmentData = new AchievmentData(achievmentDefinition);
  achievmentData.AchievmentCompleted += this.AchievmentData_AchievmentCompleted;
  achievmentData.RankEarned += this.AchievmentData_RankEarned;


  achievmentData.AddScore(AchievmentNames.PYROMANIC, 15);
  if (!achievmentData.IsCompleted(AchievmentNames.PYROMANIC))
  {
      Debug.WriteLine("Completed: " + achievmentData.GetPercentageCompleted(AchievmentNames.PYROMANIC) + "%");
      achievmentData.SetCompleted(AchievmentNames.PYROMANIC);
      Debug.WriteLine("Completed: " + achievmentData.GetPercentageCompleted(AchievmentNames.PYROMANIC) + "%");
  }


  achievmentData.AddScore(AchievmentNames.EXECUTIONER, 15);
  achievmentData.AddScore(AchievmentNames.EXECUTIONER, 23);
  achievmentData.EarnRank(AchievmentNames.EXECUTIONER, RankNames.GOLD);
 
```     
     
## License

[MIT](License.txt)
