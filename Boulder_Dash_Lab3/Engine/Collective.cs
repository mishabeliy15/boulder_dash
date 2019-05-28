namespace Boulder_Dash_Lab3
{
    public abstract class Collective:Cell
    {
        public int Bonus { get; protected set; }
        public Collective(){}
        public Collective(int bonus) => Bonus = bonus;
    }
}