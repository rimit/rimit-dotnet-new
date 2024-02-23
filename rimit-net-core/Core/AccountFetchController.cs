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
    [Route("account/fetch")]
    public class AccountFetchController : ControllerBase
    {
        private class UserData
        {
            public string mobile { get; set; }
            public string country_code { get; set; }
        }

        private class AccountData
        {
            public string account_name { get; set; }
            public string account_number { get; set; }
            public string branch_code { get; set; }
            public string branch_name { get; set; }

            public string account_type { get; set; }
            public string account_class { get; set; }
            public string account_status { get; set; }
            public string account_opening_date { get; set; }
            public string account_currency { get; set; }
            public string account_daily_limit { get; set; }

            public bool is_debit_allowed { get; set; }
            public bool is_credit_allowed { get; set; }
            public bool is_cash_debit_allowed { get; set; }
            public bool is_cash_credit_allowed { get; set; }

            public string salt { get; set; }
        }

        private class AccountData2
        {
            public string account_name { get; set; }
            public string account_number { get; set; }
            public string branch_code { get; set; }
            public string branch_name { get; set; }

            public string account_type { get; set; }
            public string account_class { get; set; }
            public string account_status { get; set; }
            public string account_opening_date { get; set; }
            public string account_currency { get; set; }
            public string account_daily_limit { get; set; }

            public bool is_debit_allowed { get; set; }
            public bool is_credit_allowed { get; set; }
            public bool is_cash_debit_allowed { get; set; }
            public bool is_cash_credit_allowed { get; set; }

            public string auth_salt { get; set; }
        }


        class AddAccountData
        {
            public UserData user { get; set; }
            public AccountData2[] accounts { get; set; }
        }

        [HttpPost]
        public GeneralResponse Post([FromBody] JsonElement req)
        {
            Debug.WriteLine("------------------");
            Debug.WriteLine("REQUEST : accountFetch");
            Debug.WriteLine("------------------");

            Debug.WriteLine(req.ToString());
            Debug.WriteLine("------------------");


            try
            {
                /* ASSIGN ENCRYPTION_KEY OF ENTITY */
                const string ENCRYPTION_KEY = "23XVJ5EITHKI13U9E0MZ9PK2VCDJYQBA";

                // ASSIGNING DATA RECIVED IN THE REQUEST
                JsonElement REQUEST_DATA = req.GetProperty("encrypted_data");

                // DECRYPTING DATA RECEIVED
                JsonElement? DECRYPTED_DATA = Crypto.DecryptRimitData(REQUEST_DATA, ENCRYPTION_KEY);

                // ERROR RESPONSE IF DECRYPTION FAILED
                if (DECRYPTED_DATA == null)
                {
                    ErrorResponse response = new ErrorResponse("accountFetch",
                    CommonCodes.RESULT_CODE_DECRYPTION_FAILED,
                    CommonCodes.STATUS_ERROR,
                    CommonCodes.RESULT_MESSAGE_E2008);

                    response.head.HTTP_CODE = CommonCodes.HTTP_CODE_BAD_REQUEST;
                    return response;
                }

                string USER_MOBILE = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("mobile").ToString();
                string USER_CC = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("country_code").ToString();
                string DOB = DECRYPTED_DATA.Value.GetProperty("content").GetProperty("data").GetProperty("dob").GetString();

                /*  */
                /*  */
                /* VERIFY THE USER */
                /* MANAGE SCOPE FOR ERRORS (Refer - https://doc.rimit.co/account/account-fetch#response-code) */
                /*  */
                /*  */

                /*  */
                /* EG FOR FAILED RESPONSE : FIND USER, IF NOT FOUND, SEND RESPONSE AS FAILED */
                bool FIND_USER = true;
                if (!FIND_USER)
                {
                    return Utilities.Response.Success("accountFetch",
                        CommonCodes.RESULT_CODE_MOBILE_NUMBER_NOT_FOUND,
                        CommonCodes.STATUS_FAILED,
                        CommonCodes.RESULT_MESSAGE_E2014,
                        CommonCodes.HTTP_CODE_SUCCESS, new object(), ENCRYPTION_KEY);
                }

                AddAccount(USER_MOBILE, USER_CC);

                // SUCCESS RESPONSE
                return Utilities.Response.Success("accountFetch",
                    CommonCodes.RESULT_CODE_SUCCESS,
                    CommonCodes.STATUS_SUCCESS,
                    CommonCodes.RESULT_MESSAGE_E1001,
                    CommonCodes.HTTP_CODE_SUCCESS, new object(), ENCRYPTION_KEY);
            }
            catch (Exception)//
            {
                return Utilities.Response.Error("accountFetch",
                    CommonCodes.RESULT_CODE_SERVICE_NOT_AVAILABLE,
                    CommonCodes.STATUS_ERROR,
                    CommonCodes.RESULT_MESSAGE_E2003,
                    CommonCodes.HTTP_CODE_SERVICE_UNAVAILABLE);
            }
        }

        private void AddAccount(string mobile, string country_code)
        {
            Debug.WriteLine("------------------");
            Debug.WriteLine("REQUEST : AddAccount");
            Debug.WriteLine("------------------");

            try
            {
                /*  */
                /* ASSIGN ENCRYPTION_KEY, API_KEY & API_ID OF ENTITY */
                const string ENCRYPTION_KEY = "23XVJ5EITHKI13U9E0MZ9PK2VCDJYQBA";
                const string AUTH_API_ID = "6360b30096066b82e9697996";
                const string AUTH_API_KEY = "b54996b0-59a8-11ed-911b-87fca52fd2dc";

                // ADD_ACCOUNT REQUEST URL
                string ADD_ACCOUNT_URL = Config.BASE_URL + "/account/add";

                ConfirmHead ADD_ACCOUNT_HEAD = new ConfirmHead()
                {
                    api = "accountAdd",
                    apiVersion = "V1",
                    timeStamp = DateTime.UtcNow.AddMinutes(330).ToString("yyyy-MM-dd hh:mm:ss tt"),
                    auth = new AuthHeader()
                    {
                        API_ID = AUTH_API_ID,
                        API_KEY = AUTH_API_KEY
                    }
                };

                /*  */
                /* ASSIGN USER DATA BASED ON REQUEST DATA ON accountFetch */
                UserData USER_DATA = new UserData()
                {
                    mobile = mobile,
                    country_code = country_code
                };
                /*  */

                /*  */
                /* READ ALL ACCOUNTS OF THE USER IN ACCOUNTS DATA */
                AccountData[] ACCOUNTS_DATA = new AccountData[]
                {
                    new AccountData() {
                        account_name= "",
                        account_number= "",
                        branch_code= "",
                        branch_name= "",

                        account_type= "",
                        account_class= "",
                        account_status= "",
                        account_opening_date= "",
                        account_currency= "",
                        account_daily_limit= "",

                        is_debit_allowed = true,
                        is_credit_allowed = true,
                        is_cash_debit_allowed = true,
                        is_cash_credit_allowed = true,

                        salt = ""
                    }
                };

                /*  */

                /*  */
                /* ASSIGN DATA RECEIVED FROM ACCOUNTS_DATA ARRAY */
                List<AccountData2> USER_ACCOUNTS = new List<AccountData2>();
                if (ACCOUNTS_DATA.Length > 0)
                {
                    for (int i = 0; i < ACCOUNTS_DATA.Length; i++)
                    {
                        AccountData2 details = new AccountData2()
                        {
                            account_name = ACCOUNTS_DATA[i].account_name,
                            account_number = ACCOUNTS_DATA[i].account_number,
                            branch_code = ACCOUNTS_DATA[i].branch_code,
                            branch_name = ACCOUNTS_DATA[i].branch_name,

                            account_type = ACCOUNTS_DATA[i].account_type,
                            account_class = ACCOUNTS_DATA[i].account_class,
                            account_status = ACCOUNTS_DATA[i].account_status,
                            account_opening_date = ACCOUNTS_DATA[i].account_opening_date,
                            account_currency = ACCOUNTS_DATA[i].account_currency,
                            account_daily_limit = ACCOUNTS_DATA[i].account_daily_limit,

                            is_debit_allowed = ACCOUNTS_DATA[i].is_debit_allowed,
                            is_credit_allowed = ACCOUNTS_DATA[i].is_credit_allowed,
                            is_cash_debit_allowed = ACCOUNTS_DATA[i].is_cash_debit_allowed,
                            is_cash_credit_allowed = ACCOUNTS_DATA[i].is_cash_credit_allowed,
                            auth_salt = ACCOUNTS_DATA[i].salt
                        };

                        USER_ACCOUNTS.Add(details);
                    }
                }
                /*  */


                /*  */

                AddAccountData ADD_ACCOUNTS_DATA = new AddAccountData()
                {
                    user = USER_DATA,
                    accounts = USER_ACCOUNTS.ToArray()
                };

                // IF THE ALL ACCOUNTS READ SUCCESSFULLY
                Result ADD_ACCOUNT_RESULT = new Result()
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,
                };

                int ADD_ACCOUNT_CONFIRM = 1;
                    Utilities.Request.ConfirmRequest(ADD_ACCOUNT_HEAD, ADD_ACCOUNT_RESULT, ADD_ACCOUNTS_DATA, ADD_ACCOUNT_URL, ENCRYPTION_KEY);


                Debug.WriteLine("*****************");
                Debug.WriteLine("ADD_ACCOUNT_CONFIRM - RESPONSE");
                Debug.WriteLine(ADD_ACCOUNT_CONFIRM);
                Debug.WriteLine("*****************");

                /*  */
                /*  */

                /* MANAGE RECEIVED RESPONSE */
                /*  */

                /*  */
                /*  */

                //---return true;
                // res.status(200).send(ADD_ACCOUNT_CONFIRM);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
