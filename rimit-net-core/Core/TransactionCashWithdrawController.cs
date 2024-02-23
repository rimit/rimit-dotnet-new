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
    [Route("transaction/withdraw-amount")]
    public class TransactionCashWithdrawController : ControllerBase
    {
        private class DebitConfirmData
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
            Debug.WriteLine("REQUEST : withdrawAmount");
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

                // CREDIT_CONFIRM REQUEST URL
                string DEBIT_CONFIRM_URL = Config.BASE_URL + "/transaction/confirmDebit";

                ConfirmHead DEBIT_CONFIRM_HEAD = new ConfirmHead()
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

                // ASSIGNING DATA RECIVED IN THE REQUEST
                JsonElement REQUEST_DATA = req.GetProperty("encrypted_data");

                // DECRYPTING DATA RECEIVED
                JsonElement? DECRYPTED_DATA = Crypto.DecryptRimitData(REQUEST_DATA, ENCRYPTION_KEY);

                // ERROR RESPONSE IF DECRYPTION FAILED
                if (DECRYPTED_DATA != null)
                {
                    ErrorResponse response = new ErrorResponse("depositAmount",
                       CommonCodes.RESULT_CODE_DECRYPTION_FAILED,
                       CommonCodes.STATUS_ERROR,
                       CommonCodes.RESULT_MESSAGE_E2008);

                    response.head.HTTP_CODE = CommonCodes.HTTP_CODE_BAD_REQUEST;
                    return response;
                }

                JsonElement USER = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("user");
                JsonElement TRANSACTION = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("transaction");
                JsonElement SETTLEMENT = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("settlement");

                string USER_MOBILE = USER.GetProperty("mobile").ToString();
                string USER_COUNTRY_CODE = USER.GetProperty("country_code").ToString();
                string USER_ACCOUNT_NUMBER = USER.GetProperty("account_number").ToString();
                string USER_ACCOUNT_CLASS = USER.GetProperty("account_class").ToString();
                string USER_ACCOUNT_TYPE = USER.GetProperty("account_type").ToString();
                string USER_BRANCH_CODE = USER.GetProperty("branch_code").ToString();

                string TRANSACTION_NO = TRANSACTION.GetProperty("txn_number").ToString();
                string TRANSACTION_URN = TRANSACTION.GetProperty("txn_urn").ToString();
                string TRANSACTION_TYPE = TRANSACTION.GetProperty("txn_type").ToString();
                string TRANSACTION_NATURE = TRANSACTION.GetProperty("txn_nature").ToString();
                string TRANSACTION_NOTE = TRANSACTION.GetProperty("txn_note").ToString();
                string TRANSACTION_DATE = TRANSACTION.GetProperty("txn_date").ToString();
                string TRANSACTION_TIME = TRANSACTION.GetProperty("txn_time").ToString();
                string TRANSACTION_TIMESTAMP = TRANSACTION.GetProperty("txn_ts").ToString();
                string TRANSACTION_AMOUNT = TRANSACTION.GetProperty("txn_amount").ToString();
                string TRANSACTION_SERVICE_CHARGE = TRANSACTION.GetProperty("txn_service_charge").ToString();
                string TRANSACTION_SERVICE_PROVIDER_CHARGE = TRANSACTION.GetProperty("txn_sp_charge").ToString();
                string TRANSACTION_FEE = TRANSACTION.GetProperty("txn_fee").ToString();

                string SETTLEMENT_ACCOUNT_TYPE = SETTLEMENT.GetProperty("account_type").ToString(); // no settlement is involved for CASH transaction. So 'NA' will be received.
                string SETTLEMENT_ACCOUNT_NUMBER = SETTLEMENT.GetProperty("account_number").ToString(); // will be empty for CASH transactions.

                /*  */
                /*  */
                /* VERIFY THE USER */
                /* MANAGE SCOPE FOR FAILED TRANSACTIONS (Refer - https://doc.rimit.co/transaction-credit/confirm-credit#result-code) */
                /* VERIFY THE USER ACCOUNT */
                /* VERIFY THE USER ACCOUNT BALANCE AVAILABILITY */
                /* CREDIT USER ACCOUNT WITH txn_amount */
                /*  */
                /*  */

                /*  */
                /* GENERATE A UNIQUE TRANSACTION_REF */
                string TRANSACTION_REF = "";
                /*  */

                /*  */
                /* ASSIGN LATEST ACCOUNT_BALANCE AFTER CREDITING THE TRANSACTION_AMOUNT */
                string ACCOUNT_BALANCE = "";
                /*  */

                DebitConfirmData DEBIT_CONFIRM_DATA = new DebitConfirmData()
                {
                    txn_urn = TRANSACTION_URN,
                    txn_number = TRANSACTION_NO,
                    txn_reference = TRANSACTION_REF,
                    txn_amount = TRANSACTION_AMOUNT,
                    txn_type = TRANSACTION_TYPE,
                    txn_nature = TRANSACTION_NATURE,
                    account_balance = ACCOUNT_BALANCE
                };

                /*  */
                /* EG FOR FAILED REQUEST : FIND LATEST ACCOUNT BALANCE, IF FOUND INSUFFICIENT, SEND REQUEST AS FAILED */
                bool CHECK_LATEST_BALANCE = true;

                Result DEBIT_CONFIRM_RESULT = null;
                object DEBIT_CONFIRM = null;
                if (!CHECK_LATEST_BALANCE)
                {
                    DEBIT_CONFIRM_RESULT = new Result
                    {
                        code = CommonCodes.RESULT_CODE_INSUFFICIENT_ACCOUNT_BALANCE,
                        status = CommonCodes.STATUS_FAILED,
                        message = CommonCodes.RESULT_MESSAGE_E8899,
                    };

                    DEBIT_CONFIRM = Utilities.Request.ConfirmRequest(
                        DEBIT_CONFIRM_HEAD,
                        DEBIT_CONFIRM_RESULT,
                        DEBIT_CONFIRM_DATA,
                        DEBIT_CONFIRM_URL,
                        ENCRYPTION_KEY
                    );

                    Debug.WriteLine("DEBIT_CONFIRM - CHECK_LATEST_BALANCE - RESPONSE");
                    Debug.WriteLine(DEBIT_CONFIRM.ToString());
                    return null;
                }
                /*  */

                // IF THE DEBIT AMOUNT IS SUCCESSFUL
                DEBIT_CONFIRM_RESULT = new Result
                {
                    code = CommonCodes.RESULT_CODE_HOLD,
                    status = CommonCodes.STATUS_HOLD,
                    message = CommonCodes.RESULT_MESSAGE_E1002,
                };

                DEBIT_CONFIRM = Utilities.Request.ConfirmRequest(
                    DEBIT_CONFIRM_HEAD,
                    DEBIT_CONFIRM_RESULT,
                    DEBIT_CONFIRM_DATA,
                    DEBIT_CONFIRM_URL,
                    ENCRYPTION_KEY
                );

                Debug.WriteLine("*****************");
                Debug.WriteLine("DEBIT_CONFIRM - RESPONSE");
                Debug.WriteLine(DEBIT_CONFIRM);
                Debug.WriteLine("*****************");

                /*  */
                /*  */

                /* MANAGE RECEIVED RESPONSE */
                /*  */

                /*  */
                /*  */
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
