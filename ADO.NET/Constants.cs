using System;
using System.Collections.Generic;
using System.Text;

namespace ADO.NET
{
    public static class Constants
    {
        public const string TO_CLOSE_APP = 
            "To close the application enter '" + CLOSE_APP_FLAG + "' instead of the task number.";

        public const string ENTER_TASK_NUMBER = 
            "Please enter number of contest task to run in the format 0X - 1X: ";

        public const string ENTER_EXISTING_TASK_NUMBER = 
            "Please enter number of a task that exists in the project: ";

        public const string PLEASE_ENTER_VILLAIN_ID = "Please enter villain ID: ";

        public const string CLOSE_APP_FLAG = "Close app";

        public const string SUCCESSFULLY_ADDED_TOWN = "Town {0} was added to the database.";

        public const string SUCCESSFULLY_ADDED_VILLAIN = "Villain {0} was added to the database.";

        public const string SUCCESSFULLY_ASSIGNED_MINION_TO_VILLAIN =
            "Successfully added {0} to be minion of {1}.";

        public const string TOWNS_AFFECTED = "{0} town names were affected.";

        public const string NO_TOWNS_AFFECTED = "No town names were affected.";

        public const string VILLAIN_DELETED = "{0} was deleted.";

        public const string MINIONS_RELEASED = "{0} minions were released.";

        public const string PLEASE_ENTER_COUNTRY_NAME = "Please enter country name: ";

        public const string PLEASE_ENTER_VILLAIN_INFO = "Please enter villain info: ";

        public const string PLEASE_ENTER_MINION_INFO = "Please enter minion info: ";
    }

    public static class ErrorMessagesConstants
    {
        public const string VILLAIN_DOESNT_EXIST_ERROR = "No villain with ID {0} exists in the database.";

        public const string NO_MINIONS_ERROR = "(no minions)";

        public const string NO_SUCH_VILLAIN_FOUND = "No such villain was found.";
    }
}
