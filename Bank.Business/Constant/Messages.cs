using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Constant
{
    public static class Messages
    {
        public static string MoneyTransferFailed = "Money transfer operation failed.";
        public static string InvalidInformation = "Invalid information.";
        public static string AddSuccessful = "Addition successful.";
        public static string DeleteSuccessful = "Deletion successful.";
        public static string UpdateSuccessful = "Update successful.";
        public static string GetByIdSuccessful = "Get by Id successful.";
        public static string GetAllSuccessful = "Get all successful.";
        public static string UpdateFailed = "Update failed!";

        public static string UserAddSuccessful = "User added successfully.";
        public static string UserAddFailed = "User addition failed.";

        public static string UserUpdateSuccessful = "User update successful.";
        public static string UserDeleteSuccessful = "User deletion successful.";
        public static string UserNotFound = "User not found.";
        public static string InvalidLogin = "Invalid login.";
        public static string LoginSuccessful = "Login successful.";

        public static string AlreadyExists = "Already exists!";

        public static string AddFailed = "Failed to add.";

        public static string RetrieveSuccessful = "Retrieval successful.";
        public static string RetrieveFailed = "Retrieval failed.";
        public static string RegistrationSuccessful = "Registration successful.";

        public static string MoneyTransferSuccessful = "Your money transfer was completed successfully.";
        public static string TransactionNotSaved = "Money transfer completed but transaction was not saved.";
        public static string TransactionLast4Success = "The last 4 transactions were brought successfully.";
    }

}
