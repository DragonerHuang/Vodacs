using Vodace.Core.Enums;
using Vodace.Core.Extensions;

namespace Vodace.Core.Utilities
{
    public class WebResponseContent
    {
        public WebResponseContent()
        {
        }
        public WebResponseContent(bool status)
        {
            this.status = status;
        }
        public bool status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        //public string message { get; set; }
        public object data { get; set; }

        public WebResponseContent OK()
        {
            this.status = true;
            return this;
        }

        public static WebResponseContent Instance
        {
            get { return new WebResponseContent(); }
        }


        public WebResponseContent OK(string message = null,object data=null)
        {
            this.status = true;
            this.message = message;
            this.data = data;
            this.code = "200";
            return this;
        }
        public WebResponseContent OK(ResponseType responseType)
        {
            return Set(responseType, true);
        }
        public WebResponseContent Error(string message = null)
        {
            this.status = false;
            this.message = message;
            this.code = "400";
            return this;
        }
        public WebResponseContent Error(ResponseType responseType)
        {
            return Set(responseType, false);
        }
        public WebResponseContent Set(ResponseType responseType)
        {
            bool? b = null;
            return this.Set(responseType, b);
        }
        public WebResponseContent Set(ResponseType responseType, bool? status)
        {
            return this.Set(responseType, null, status);
        }
        public WebResponseContent Set(ResponseType responseType, string msg)
        {
            bool? b = null;
            return this.Set(responseType, msg, b);
        }
        public WebResponseContent Set(ResponseType responseType, string msg, bool? status)
        {
            if (status != null)
            {
                this.status = (bool)status;
            }
            this.code = ((int)responseType).ToString();
            if (!string.IsNullOrEmpty(msg))
            {
                message = msg;
                return this;
            }
            message = responseType.GetMsg();
            return this;
        }

    }
}
