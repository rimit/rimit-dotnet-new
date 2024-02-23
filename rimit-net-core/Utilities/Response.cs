using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RimitNetCore.Utilities
{

    public class Head
    {
        public string api { get; set; }
        public string apiVersion { get; set; }
        public string timeStamp { get; set; }
        public int HTTP_CODE { get; set; }

        public Head(string api)
        {
            this.api = api;
            this.apiVersion = "V1";
            // Asia/Calcutta timezone = GMT+5:30 (+330 minutes)
            this.timeStamp = DateTime.UtcNow.AddMinutes(330).ToString("yyyy-MM-dd hh:mm:ss tt");
        }
    }

   

    public class GeneralResponse
    {
        public Head head { get; set; }


        public GeneralResponse(string api)
        {
            head = new Head(api);
        }
    }

    /*public class Data
    {

    }*/

    public class Result
    {
        public int code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }

    public class Content
    {
        public Result result { get; set; }

        public object data { get; set; }

        public Content()
        {
            result = new Result();
            data = new object();
        }
    }

    public class ContentHolder
    {
        public Content content { get; set; }

        public ContentHolder()
        {
            content = new Content();
        }
    }

    public class ErrorResponse : GeneralResponse
    {
        public Content content { get; set; }
        public ErrorResponse(string api, int code, string status, string message) : base(api)
        {
            content = new Content();
            content.result.code = code;
            content.result.status = status;
            content.result.message = message;
        }
    }

    

    public class SuccessResponse : GeneralResponse
    {
        public EncryptResult encrypted_data { get; set; }
        public SuccessResponse(string api, EncryptResult encrypted_data) : base(api)
        {
            this.encrypted_data = encrypted_data;
        }
    }

    public class Response
    {
        public static SuccessResponse Success(string api, int code, string status, string message, int httpCode, object data, string key)
        {
            ContentHolder encryptData = new ContentHolder();
            encryptData.content.result.code = code;
            encryptData.content.result.status = status;
            encryptData.content.result.message = message;
            encryptData.content.data = data;
            string datas = JsonSerializer.Serialize(encryptData);

            SuccessResponse res =  new SuccessResponse(api, Crypto.EncryptRimitData(datas, key));
            res.head.HTTP_CODE = httpCode;
            return res;
        }

        public static ErrorResponse Error(string api, int code, string status, string message, int httpCode)
        {
            ErrorResponse res = new ErrorResponse(api, code, status, message);
            res.head.HTTP_CODE = httpCode;
            return res;
        }
    }
}
