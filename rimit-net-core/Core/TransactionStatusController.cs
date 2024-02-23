using Microsoft.AspNetCore.Mvc;
using RimitNetCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RimitNetCore.Controllers
{
    [ApiController]
    [Route("transaction/status")]
    public class TransactionStatusController : ControllerBase
    {

        private class StatusData
        {
            public string txn_number { get; set; }
            public string txn_urn { get; set; }
            public string txn_reference { get; set; }
            public string txn_amount { get; set; }
            public string txn_type { get; set; }
            public string txn_nature { get; set; }
        }

        [HttpPost]
        public GeneralResponse Post([FromBody] JsonElement req)
        {
            /*  */
            /* REQUEST PAYLOAD, FOR USING IN POSTMAN */
            /*
             {
                "txn_type": "",
                "txn_nature": "",
                "txn_number": "",
                "txn_urn": "",
                "txn_reference": "",
                "txn_amount": ""
             }
            */
            /*  */

            Debug.WriteLine("------------------");
            Debug.WriteLine("REQUEST : txnStatus");
            Debug.WriteLine("------------------");

            Debug.WriteLine(req.ToString());
            Debug.WriteLine("------------------");

            try
            {
                /*  */
                /* ASSIGN ENCRYPTION_KEY, API_KEY & API_ID OF ENTITY */
                const string ENCRYPTION_KEY = "23XVJ5EITHKI13U9E0MZ9PK2VCDJYQBA";
                const string AUTH_API_ID = "6360b30096066b82e9697996";
                const string AUTH_API_KEY = "b54996b0-59a8-11ed-911b-87fca52fd2dc";
                /*  */

                /*  */
                /* ASSIGNING DATA RECIVED IN THE REQUEST  */
                JsonElement REQUEST_DATA = req;
                /*  */

                /*  */
                /* ASSIGNING DATA RECIVED IN THE REQUEST  */
                string TRANSACTION_TYPE = REQUEST_DATA.GetProperty("txn_type").ToString();
                string TRANSACTION_NATURE = REQUEST_DATA.GetProperty("txn_nature").ToString();
                string TRANSACTION_NUMBER = REQUEST_DATA.GetProperty("txn_number").ToString();
                string TRANSACTION_URN = REQUEST_DATA.GetProperty("txn_urn").ToString();
                string TRANSACTION_AMOUNT = REQUEST_DATA.GetProperty("txn_amount").ToString();
                string TRANSACTION_REF = REQUEST_DATA.GetProperty("txn_reference").ToString();
                /*  */

                // TXN_STATUS REQUEST URL
                string TXN_STATUS_URL = Config.BASE_URL + "/transaction/statusCheck";

                ConfirmHead TXN_STATUS_HEAD = new ConfirmHead()
                {
                    api = "statusCheck",
                    apiVersion = "V1",
                    timeStamp = DateTime.UtcNow.AddMinutes(330).ToString("yyyy-MM-dd hh:mm:ss tt"),
                    auth = new AuthHeader()
                    {
                        API_ID = AUTH_API_ID,
                        API_KEY = AUTH_API_KEY
                    }
                };

                StatusData TXN_STATUS_DATA = new StatusData()
                {
                    txn_number = TRANSACTION_NUMBER,
                    txn_urn = TRANSACTION_URN,
                    txn_reference = TRANSACTION_REF,
                    txn_amount = TRANSACTION_AMOUNT,
                    txn_type = TRANSACTION_TYPE,
                    txn_nature = TRANSACTION_NATURE
                };

                // TXN_STATUS_RESULT MUST BE EMPTY
                Result TXN_STATUS_RESULT = new Result { };

                object TXN_STATUS = Utilities.Request.ConfirmRequest(
                    TXN_STATUS_HEAD,
                    TXN_STATUS_RESULT,
                    TXN_STATUS_DATA,
                    TXN_STATUS_URL,
                    ENCRYPTION_KEY
                );

                Debug.WriteLine("*****************");
                Debug.WriteLine("TXN_STATUS - RESPONSE");
                Debug.WriteLine(TXN_STATUS.ToString());
                Debug.WriteLine("*****************");

                /*  */
                /*  */

                /* MANAGE RECEIVED RESPONSE */
                /*  */

                /*  */
                /*  */
                //return res.status(200).send(TXN_STATUS);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
