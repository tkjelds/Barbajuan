internal partial class Program
{
    // Helper class for timeline experiment.
    public class Timeline
    {

        public int turn { get; set; }
        public string cardColor { get; set; }
        public string cardType { get; set; }
        public int quartile { get; set; }

        public Timeline(int turn, Card card)
        {
            this.turn = turn;
            this.cardColor = card.cardColor.ToString();
            this.cardType = card.cardType.ToString();
        }

        public Timeline(int turn, string cardColor, string cardType, int quartile)
        {
            this.turn = turn;
            this.cardColor = cardColor;
            this.cardType = cardType;
            this.quartile = quartile;
        }


    }
}
