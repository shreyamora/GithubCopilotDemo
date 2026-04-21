namespace GithubCopilotDemo.Plugins.Configuration
{
    /// <summary>
    /// Constants and configuration settings for Account Task Creation plugin
    /// </summary>
    public static class TaskCreationConstants
    {
        // Plugin Information
        public const string PLUGIN_NAME = "AccountTaskCreationPlugin";
        public const string PLUGIN_VERSION = "1.0.0";

        // Entity Names
        public const string ACCOUNT_ENTITY = "account";
        public const string TASK_ENTITY = "task";
        public const string SYSTEM_USER_ENTITY = "systemuser";

        // Message and Stage
        public const string CREATE_MESSAGE = "Create";
        public const int POST_OPERATION_STAGE = 40;

        // Task Configuration
        public const int TASK_PRIORITY_NORMAL = 1;
        public const int TASK_STATUS_OPEN = 1;
        public const int DAYS_UNTIL_DUE = 3;

        // Account Attributes
        public static class AccountAttributes
        {
            public const string NAME = "name";
            public const string OWNER_ID = "ownerid";
            public const string INDUSTRY = "industry";
            public const string ACCOUNT_NUMBER = "accountnumber";
            public const string REVENUE = "revenue";
            public const string CREATED_ON = "createdon";
        }

        // Task Attributes
        public static class TaskAttributes
        {
            public const string SUBJECT = "subject";
            public const string DESCRIPTION = "description";
            public const string REGARDING_OBJECT_ID = "regardingobjectid";
            public const string OWNER_ID = "ownerid";
            public const string PRIORITY_CODE = "prioritycode";
            public const string SCHEDULED_END = "scheduledend";
            public const string STATE_CODE = "statecode";
            public const string STATUS_CODE = "statuscode";
        }

        // Message Templates
        public static class MessageTemplates
        {
            public const string SUBJECT_TEMPLATE = "Follow-up: New Account - {0}";
            public const string DESCRIPTION_TEMPLATE = 
                "New account created.\n\n" +
                "Account Name: {0}\n" +
                "Account Number: {1}\n" +
                "Industry: {2}\n" +
                "Created Date: {3}";
        }

        // Logging Messages
        public static class LogMessages
        {
            public const string PLUGIN_STARTED = "AccountTaskCreationPlugin started";
            public const string PLUGIN_COMPLETED = "AccountTaskCreationPlugin completed successfully";
            public const string WRONG_MESSAGE_OR_STAGE = "Plugin invoked with wrong message or stage. Exiting.";
            public const string TARGET_NOT_FOUND = "Target parameter not found in context";
            public const string WRONG_ENTITY_TYPE = "Entity is not an account. Exiting.";
            public const string PROCESSING_ACCOUNT = "Processing account creation for ID: {0}";
            public const string ACCOUNT_NAME_EMPTY = "Account name is empty. Cannot create task.";
            public const string TASK_CREATED_SUCCESS = "Task created successfully with ID: {0}";
            public const string ERROR_FAULT_EXCEPTION = "FaultException in AccountTaskCreationPlugin: {0}";
            public const string ERROR_GENERAL_EXCEPTION = "Exception in AccountTaskCreationPlugin: {0}\n{1}";
        }

        // Feature Flags (Can be used for conditional logic)
        public static class FeatureFlags
        {
            public const bool ENABLE_TASK_CREATION = true;
            public const bool NOTIFY_ADMIN_ON_ERROR = true;
            public const bool LOG_ALL_OPERATIONS = true;
            public const bool INHERIT_OWNER_FROM_ACCOUNT = true;
        }

        // Error Messages
        public static class ErrorMessages
        {
            public const string GENERIC_ERROR = "An error occurred in AccountTaskCreationPlugin: {0}";
            public const string UNEXPECTED_ERROR = "An unexpected error occurred in AccountTaskCreationPlugin: {0}";
            public const string SERVICE_CREATION_FAILED = "Failed to create organization service";
        }
    }
}
