using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace RimitNetCore.Utilities
{
    public class AuthHeader
    {
        public string API_ID { get; set; }
        public string API_KEY { get; set; }
    }

    public class ConfirmHead
    {
        public string api { get; set; }
        public string apiVersion { get; set; }
        public string timeStamp { get; set; }

        public AuthHeader auth { get; set; }

        public ConfirmHead()
        {
            auth = new AuthHeader();
        }

    }

    class RequestDetails
    {
        public ConfirmHead head { get; set; }
        public EncryptResult encrypted_data { get; set; }
    }

    public class Request
    {

        public static object ConfirmRequest(ConfirmHead head, Result result, object data, string url, string key)
        {
            ContentHolder encryptData = new ContentHolder();
            encryptData.content.result = result;
            encryptData.content.data = data;

            Debug.WriteLine("---------------------");
            Debug.WriteLine("DATA TO BE ENCRYPTED");
            Debug.WriteLine(JsonSerializer.Serialize(encryptData));
            Debug.WriteLine("---------------------");

            string stringData = JsonSerializer.Serialize(encryptData);
            EncryptResult encrypted = Crypto.EncryptRimitData(stringData, key);

            RequestDetails details = new RequestDetails()
            {
                head = head,
                encrypted_data = encrypted
            };

            string responseString = "";
            HttpStatusCode statusCode = HttpStatusCode.OK;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                try
                {
                    responseString = client.UploadString(url, "POST", JsonSerializer.Serialize(details));
                }
                catch (WebException webex)
                {
                    statusCode = ((HttpWebResponse)webex.Response).StatusCode;
                    using (Stream stream = webex.Response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseString= reader.ReadToEnd();
                        }
                    }
                }
            }

            if (statusCode==HttpStatusCode.BadRequest || statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.ServiceUnavailable)
            {
                Debug.WriteLine("---------------------");
                Debug.WriteLine("DECRYPTED FAILED");
                Debug.WriteLine(responseString);
                Debug.WriteLine("---------------------");
                return responseString;
            }

            Debug.WriteLine("---------------------");
            Debug.WriteLine("DATA TO BE DECRYPTED");
            Debug.WriteLine(responseString);
            Debug.WriteLine("---------------------");

            JsonElement encryptedElement = JsonDocument.Parse(responseString).RootElement.GetProperty("encrypted_data");

            JsonElement? decrypted = Crypto.DecryptRimitData(encryptedElement, key);
            
            return new JsonElement[] { JsonDocument.Parse(responseString).RootElement.GetProperty("head"), decrypted.Value.GetProperty("content") };
        }
    }
}
