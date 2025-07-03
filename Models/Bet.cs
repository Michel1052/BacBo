namespace BacBo.Models
{
    public class Bet
    {
        public string Choice { get; set; } 
        public int PlayerDice1 { get; set; }
        public int PlayerDice2 { get; set; }
        public int BankerDice1 { get; set; }
        public int BankerDice2 { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public List<Bet> Bets { get; set; } = new List<Bet>();

    }

}
