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
    [Route("account/statement")]
    public class AccountStatementController : ControllerBase
    {
        private class AccountTransaction
        {
            public string txn_id { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public string debit_amount { get; set; }
            public string credit_amount { get; set; }
            public string balance { get; set; }
            public string description { get; set; }
        }

        private class UserAccountData
        {
            public string account_number { get; set; }
            public string branch_code { get; set; }
            public string balance_amount { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string transactions_count { get; set; }
        }

        private class AccoutStatementResultData
        {
            public UserAccountData account { get; set; }
            public AccountTransaction[] transactions { get; set; }
        }

        [HttpPost]
        public GeneralResponse Post([FromBody] JsonElement req)
        {
            Debug.WriteLine("------------------");
            Debug.WriteLine("REQUEST : accountStatement");
            Debug.WriteLine("------------------");

            Debug.WriteLine(req.ToString());
            Debug.WriteLine("------------------");

            try
            {

                List<AccountTransaction> TRANSACTION_DATA = new List<AccountTransaction>();

                /* ASSIGN ENCRYPTION_KEY OF ENTITY */
                const string ENCRYPTION_KEY = "23XVJ5EITHKI13U9E0MZ9PK2VCDJYQBA";

                // ASSIGNING DATA RECIVED IN THE REQUEST
                JsonElement REQUEST_DATA = req.GetProperty("encrypted_data");

                // DECRYPTING DATA RECEIVED
                JsonElement? DECRYPTED_DATA = Crypto.DecryptRimitData(REQUEST_DATA, ENCRYPTION_KEY);

                // ERROR RESPONSE IF DECRYPTION FAILED
                if (DECRYPTED_DATA == null)
                {
                    ErrorResponse response = new ErrorResponse("accountStatement",
                    CommonCodes.RESULT_CODE_DECRYPTION_FAILED,
                    CommonCodes.STATUS_ERROR,
                    CommonCodes.RESULT_MESSAGE_E2008);

                    response.head.HTTP_CODE = CommonCodes.HTTP_CODE_BAD_REQUEST;
                    return response;
                }

                string USER_MOBILE = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("mobile").ToString(); ;
                string USER_CC = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("mobile").ToString(); ;
                string ACC_NO = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("mobile").ToString(); ;
                string ACC_BRANCH = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("mobile").ToString(); ;

                string START_DATE = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("mobile").ToString(); ;
                string END_DATE = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("mobile").ToString(); ;

                /*  */
                /*  */
                /* VERIFY THE USER */
                /* MANAGE SCOPE FOR ERRORS (Refer - https://doc.rimit.co/account/account-statement#response-code) */
                /*  */
                /*  */

                /*  */
                /* EG FOR FAILED RESPONSE : FIND USER ACCOUNT, IF NOT FOUND, SEND RESPONSE AS FAILED */
                bool FIND_ACCOUNT = true;
                if (!FIND_ACCOUNT)
                {
                    return Utilities.Response.Success("accountStatement",
                        CommonCodes.RESULT_CODE_INVALID_ACCOUNT,
                        CommonCodes.STATUS_FAILED,
                        CommonCodes.RESULT_MESSAGE_E2021,
                        CommonCodes.HTTP_CODE_SUCCESS, new object(), ENCRYPTION_KEY);
                }
                /*  */

                /*  */
                /* FIND THE ACCOUNT BALANCE AND ASSIGN. KEEP 0 IF NO BALANCE FOUND*/
                string ACC_BALANCE = "0";
                /*  */

                /*  */
                /* FIND ALL TRANSACTIONS BETWEEN START_DATE & END_DATE IN THE RESPECTIVE ACCOUNT */
                AccountTransaction[] ACCOUNT_TRANSACTION = new AccountTransaction[] {
                    new AccountTransaction() {
                        txn_id= "",
                        date= "",
                        time= "",
                        debit_amount= "",
                        credit_amount= "",
                        balance= "",
                        description= "",
                    },
                    new AccountTransaction() {
                        txn_id= "",
                        date= "",
                        time= "",
                        debit_amount= "",
                        credit_amount= "",
                        balance= "",
                        description= "",
                    }
                };
                /*  */
                /*  */
                /* ASSIGN DATA RECEIVED FROM ACCOUNT_TRANSACTION ARRAY */
                if (ACCOUNT_TRANSACTION.Length > 0)
                {
                    for (int i = 0; i < ACCOUNT_TRANSACTION.Length; i++)
                    {
                        AccountTransaction details = new AccountTransaction()
                        {
                            txn_id = ACCOUNT_TRANSACTION[i].txn_id,
                            date = ACCOUNT_TRANSACTION[i].date,
                            time = ACCOUNT_TRANSACTION[i].time,
                            debit_amount = ACCOUNT_TRANSACTION[i].debit_amount,
                            credit_amount = ACCOUNT_TRANSACTION[i].credit_amount,
                            balance = ACCOUNT_TRANSACTION[i].balance,
                            description = ACCOUNT_TRANSACTION[i].description
                        };
                        TRANSACTION_DATA.Add(details);
                    }
                }
                /*  */

                UserAccountData USER_ACCOUNT_DATA = new UserAccountData()
                {
                    account_number = ACC_NO,
                    branch_code = ACC_BRANCH,
                    balance_amount = ACC_BALANCE,
                    start_date = START_DATE,
                    end_date = END_DATE,
                    transactions_count = ACCOUNT_TRANSACTION.Length.ToString(),
                };

                /*result = {
                    code: commonCodes.RESULT_CODE_SUCCESS,
                    status: commonCodes.STATUS_SUCCESS,
                    message: commonCodes.RESULT_MESSAGE_E1001,
                };*/

                AccoutStatementResultData data = new AccoutStatementResultData()
                {
                    account = USER_ACCOUNT_DATA,
                    transactions = TRANSACTION_DATA.ToArray(),
                };

                return Utilities.Response.Success("accountStatement",
                    CommonCodes.RESULT_CODE_SUCCESS,
                    CommonCodes.STATUS_SUCCESS,
                    CommonCodes.RESULT_MESSAGE_E1001,
                    CommonCodes.HTTP_CODE_SUCCESS,
                    data,
                    ENCRYPTION_KEY);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Utilities.Response.Error("accountStatement",
                    CommonCodes.RESULT_CODE_SERVICE_NOT_AVAILABLE,
                    CommonCodes.STATUS_ERROR,
                    CommonCodes.RESULT_MESSAGE_E2003,
                    CommonCodes.HTTP_CODE_SERVICE_UNAVAILABLE);
            }        
        }
    }
}
