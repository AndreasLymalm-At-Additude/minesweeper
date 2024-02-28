namespace Minesweeper
{
    internal class MenuOption
    {
        public string Name { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public int MinefieldWidth { get; set; }
        public int MinefieldHeight { get; set; }
        public int MineCount { get; set; }
    }
}
