namespace Store.Web.Models
{
    public class ConfirmationModel
    {
        public string CellPhone { get; set; }

        public int OrderId { get; set; }

        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
