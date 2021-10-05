namespace Milkify.Core.Models
{
    public class LayoutTabModel
    {
        public string TabName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int Order { get; set; }
        public string NavIcon { get; set; }
    }
}