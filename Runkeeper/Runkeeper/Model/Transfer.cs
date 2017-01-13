namespace Runkeeper.Model
{
    public class Transfer
    {
        public Transfer(DataHandler datahandler)
        {
            data = datahandler;
        }

        public DataHandler data { get; set; }
    }
}
