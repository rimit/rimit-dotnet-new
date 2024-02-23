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
    [Route("transaction/depositSuccess")]
    public class DepositSuccessController : ControllerBase
    {
        private class CreditConfirmData
        {
            public string txn_urn { get; set; }
            public string txn_number { get; set; }
            public string txn_reference { get; set; }
            public string txn_amount { get; set; }
            public string txn_type { get; set; }
            public string txn_nature { get; set; }
            public string account_balance { get; set; }
        }

        [HttpPost]
        public GeneralResponse Post([FromBody] JsonElement req)
        {
            Debug.WriteLine("------------------");
            Debug.WriteLine("REQUEST : depositSuccess");
            Debug.WriteLine("------------------");

            Debug.WriteLine(req.ToString());
            Debug.WriteLine("------------------");

            try
            {
                // ASSIGNING DATA RECIVED IN THE REQUEST
                JsonElement TRANSACTION = req;

                string TRANSACTION_NO = TRANSACTION.GetProperty("txn_number").ToString();
                string TRANSACTION_URN = TRANSACTION.GetProperty("txn_urn").ToString();
                string TRANSACTION_TYPE = TRANSACTION.GetProperty("txn_type").ToString();
                string TRANSACTION_NATURE = TRANSACTION.GetProperty("txn_nature").ToString();
                string TRANSACTION_AMOUNT = TRANSACTION.GetProperty("txn_amount").ToString();

                /*  */
                /* ASSIGN ENCRYPTION_KEY, API_KEY & API_ID OF ENTITY */
                const string ENCRYPTION_KEY = "23XVJ5EITHKI13U9E0MZ9PK2VCDJYQBA";
                const string AUTH_API_ID = "6360b30096066b82e9697996";
                const string AUTH_API_KEY = "b54996b0-59a8-11ed-911b-87fca52fd2dc";
                /*  */

                // CREDIT_CONFIRM REQUEST URL
                string CREDIT_CONFIRM_URL = Config.BASE_URL + "/transaction/confirmCredit";

                ConfirmHead CREDIT_CONFIRM_HEAD = new ConfirmHead()
                {
                    api = "confirmCredit",
                    apiVersion = "V1",
                    timeStamp = DateTime.UtcNow.AddMinutes(330).ToString("yyyy-MM-dd hh:mm:ss tt"),
                    auth = new AuthHeader()
                    {
                        API_ID = AUTH_API_ID,
                        API_KEY = AUTH_API_KEY
                    }
                };

                /*  */
                /* GENERATE A UNIQUE TRANSACTION_REF */
                string TRANSACTION_REF = "";
                /*  */

                /*  */
                /* ASSIGN LATEST ACCOUNT_BALANCE AFTER CREDITING THE TRANSACTION_AMOUNT */
                string ACCOUNT_BALANCE = "";
                /*  */

                CreditConfirmData CREDIT_CONFIRM_DATA = new CreditConfirmData(){
                    txn_urn = TRANSACTION_URN,
                    txn_number = TRANSACTION_NO,
                    txn_reference = TRANSACTION_REF,
                    txn_amount = TRANSACTION_AMOUNT,
                    txn_type = TRANSACTION_TYPE,
                    txn_nature = TRANSACTION_NATURE,
                    account_balance = ACCOUNT_BALANCE
                };

                // IF THE CREDIT AMOUNT IS SUCCESSFUL
                Result CREDIT_CONFIRM_RESULT = new Result
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,

                };
                

                object CREDIT_CONFIRM = Utilities.Request.ConfirmRequest(
                    CREDIT_CONFIRM_HEAD,
                    CREDIT_CONFIRM_RESULT,
                    CREDIT_CONFIRM_DATA,
                    CREDIT_CONFIRM_URL,
                    ENCRYPTION_KEY
                );

                Debug.Write("*****************");
                Debug.Write("CREDIT_CONFIRM - RESPONSE");
                Debug.Write(CREDIT_CONFIRM.ToString());
                Debug.Write("*****************");

                // return res.status(200).send(CREDIT_CONFIRM);
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
