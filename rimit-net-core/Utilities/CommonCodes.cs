using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RimitNetCore.Utilities
{
    public static class CommonCodes
    {
        public const int HTTP_CODE_SUCCESS = 200;
        public const int HTTP_CODE_BAD_REQUEST = 400;
        public const int HTTP_CODE_UNAUTHORIZED = 401;
        public const int HTTP_CODE_NOT_ACCEPTABLE = 406;
        public const int HTTP_CODE_CONFLICT = 409;
        public const int HTTP_CODE_SERVICE_UNAVAILABLE = 503;

        public const string STATUS_SUCCESS = "SUCCESS";
        public const string STATUS_FAILED = "FAILED";
        public const string STATUS_HOLD = "HOLD";
        public const string STATUS_REFUND = "REFUND";
        public const string STATUS_CANCEL = "CANCEL";
        public const string STATUS_PROCESSING = "PROCESSING";
        public const string STATUS_PENDING = "PENDING";
        public const string STATUS_UNDEFINED = "UNDEFINED";
        public const string STATUS_ERROR = "ERROR";

        public const int RESULT_CODE_SUCCESS = 1001;
        public const int RESULT_CODE_HOLD = 1002;
        public const int RESULT_CODE_FAILED = 9999;
        public const int RESULT_CODE_TECHNICAL_ERROR = 2001;
        public const int RESULT_CODE_ERROR_WITH_SERVICE_PROVIDER = 2002;
        public const int RESULT_CODE_SERVICE_NOT_AVAILABLE = 2003;
        public const int RESULT_CODE_AUTHENTICATION_FAILED = 2004;
        public const int RESULT_CODE_SERVER_DOWN = 2005;
        public const int RESULT_CODE_CANNOT_PROCESS_THIS_REQUEST = 2006;
        public const int RESULT_CODE_ENCRYPTION_FAILED = 2007;
        public const int RESULT_CODE_DECRYPTION_FAILED = 2008;
        public const int RESULT_CODE_HASH_VALIDATION_FAILED = 2009;
        public const int RESULT_CODE_NO_ACTIVE_ACCOUNT_FOUND = 2011;
        public const int RESULT_CODE_ACCOUNT_NOT_FOUND_FOR_USER = 2012;
        public const int RESULT_CODE_DOB_MISMATCH = 2013;
        public const int RESULT_CODE_MOBILE_NUMBER_NOT_FOUND = 2014;
        public const int RESULT_CODE_INVALID_ACCOUNT = 2021;
        public const int RESULT_CODE_ACCOUNT_NUMBER_AND_MOBILE_NUMBER_DO_NOT_MATCH = 2022;
        public const int RESULT_CODE_INSUFFICIENT_ACCOUNT_BALANCE = 8899;
        public const int RESULT_CODE_ACCOUNT_NOT_FOUND = 8898;
        public const int RESULT_CODE_ACCOUNT_IS_INACTIVE_BLOCKED_CLOSED = 8897;
        public const int RESULT_CODE_DEBIT_IS_NOT_ALLOWED = 8896;
        public const int RESULT_CODE_CREDIT_IS_NOT_ALLOWED = 8895;
        public const int RESULT_CODE_USER_DECLINED = 8894;
        public const int RESULT_CODE_TIME_EXPIRED = 8893;
        public const int RESULT_CODE_ATTEMPTS_TO_RETRY_EXCEEDED_MAXIMUM = 8892;
        public const int RESULT_CODE_SETTLEMENT_ACCOUNT_NOT_FOUND = 8891;
        public const int RESULT_CODE_ENTITY_IS_INACTIVE = 9988;
        public const int RESULT_CODE_INSUFFICIENT_VIRTUAL_ACCOUNT_BALANCE = 9989;
        public const int RESULT_CODE_TRANSACTION_DETAILS_DO_NOT_MATCH = 2091;
        public const int RESULT_CODE_TRANSACTION_REFERENCE_IS_NOT_UNIQUE = 2092;
        public const int RESULT_CODE_TRANSACTION_REFERENCE_MUST_BE_SAME = 2093;

        public const string RESULT_MESSAGE_E1001 = "The request is successfully processed";
        public const string RESULT_MESSAGE_E1002 = "The status updated as hold";
        public const string RESULT_MESSAGE_E9999 = "Processed request failed";
        public const string RESULT_MESSAGE_E2001 = "Failed due to technical error";
        public const string RESULT_MESSAGE_E2002 = "Error with Service Provider";
        public const string RESULT_MESSAGE_E2003 = "Service not available";
        public const string RESULT_MESSAGE_E2004 = "Authentication Failed";
        public const string RESULT_MESSAGE_E2005 = "Server Down";
        public const string RESULT_MESSAGE_E2006 = "Cannot process this request";
        public const string RESULT_MESSAGE_E2007 = "Encryption failed";
        public const string RESULT_MESSAGE_E2008 = "Decryption failed";
        public const string RESULT_MESSAGE_E2009 = "Hash validation failed";
        public const string RESULT_MESSAGE_E2011 = "No active account found";
        public const string RESULT_MESSAGE_E2012 = "Account not found for user";
        public const string RESULT_MESSAGE_E2013 = "DOB mismatch";
        public const string RESULT_MESSAGE_E2014 = "Mobile number not found";
        public const string RESULT_MESSAGE_E2021 = "Invalid account";
        public const string RESULT_MESSAGE_E2022 = "Account number and mobile number do not match";
        public const string RESULT_MESSAGE_E2091 = "Transaction details do not match";
        public const string RESULT_MESSAGE_E2092 = "Transaction reference is not unique";
        public const string RESULT_MESSAGE_E2093 = "The transaction reference must be the same as the HOLD";
        public const string RESULT_MESSAGE_E8899 = "Insufficient account balance";
        public const string RESULT_MESSAGE_E8898 = "Account not found";
        public const string RESULT_MESSAGE_E8897 = "Account is inactive / blocked / closed";
        public const string RESULT_MESSAGE_E8896 = "Debit is not allowed";
        public const string RESULT_MESSAGE_E8895 = "Credit is not allowed";
        public const string RESULT_MESSAGE_E8894 = "User declined";
        public const string RESULT_MESSAGE_E8893 = "Time expired";
        public const string RESULT_MESSAGE_E8892 = "Attempts to retry exceeded maximum";
        public const string RESULT_MESSAGE_E8891 = "Settlement account not found";
        public const string RESULT_MESSAGE_E9988 = "Entity is inactive";
        public const string RESULT_MESSAGE_E9989 = "Insufficient virtual account balance";
    }
}
