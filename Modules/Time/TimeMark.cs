namespace CliTool.Modules.Time
{
    public  class TimeMark
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Time { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
