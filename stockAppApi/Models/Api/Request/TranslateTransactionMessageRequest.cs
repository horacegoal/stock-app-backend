using System.ComponentModel;

namespace stockAppApi.Models.Api.Request
{
    public class TranslateTransactionMessageRequest
    {
        // [DefaultValue("Tue, 14 Feb at 11:41 PM  滙豐: 買入 20 股 GOOGL， 成交價 USD92.96")]
        public string Message { get; set; }
    }
}