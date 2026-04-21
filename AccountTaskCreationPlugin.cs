using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace GithubCopilotDemo.Plugins
{
    /// <summary>
    /// Plugin to automatically create a task record when an account is created in Dynamics 365
    /// Registered on: Account Entity
    /// Event: Create (Post-Operation, Synchronous)
    /// </summary>
    public class AccountTaskCreationPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Initialize services
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService orgService = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                // Step 1: Validate plugin execution context
                if (context.MessageName != "Create" || context.Stage != 40) // 40 = Post-Operation
                {
                    tracingService.Trace("Plugin invoked with wrong message or stage. Exiting.");
                    return;
                }

                tracingService.Trace("AccountTaskCreationPlugin started");

                // Step 2: Get the Account entity from the context
                if (!context.InputParameters.Contains("Target"))
                {
                    tracingService.Trace("Target parameter not found in context");
                    return;
                }

                Entity accountEntity = (Entity)context.InputParameters["Target"];

                // Step 3: Verify it's an Account entity
                if (accountEntity.LogicalName != "account")
                {
                    tracingService.Trace("Entity is not an account. Exiting.");
                    return;
                }

                tracingService.Trace($"Processing account creation for ID: {accountEntity.Id}");

                // Step 4: Extract account information
                Guid accountId = accountEntity.Id;
                string accountName = accountEntity.Contains("name") ? accountEntity["name"].ToString() : "Unknown";
                Guid accountOwnerId = Guid.Empty;
                string accountIndustry = "N/A";

                // Get owner information
                if (accountEntity.Contains("ownerid"))
                {
                    EntityReference ownerRef = (EntityReference)accountEntity["ownerid"];
                    accountOwnerId = ownerRef.Id;
                }

                // Get industry information
                if (accountEntity.Contains("industry"))
                {
                    OptionSetValue industryOption = (OptionSetValue)accountEntity["industry"];
                    accountIndustry = industryOption.Value.ToString();
                }

                tracingService.Trace($"Account Name: {accountName}, Owner ID: {accountOwnerId}");

                // Step 5: Validate required fields
                if (string.IsNullOrEmpty(accountName))
                {
                    tracingService.Trace("Account name is empty. Cannot create task.");
                    return;
                }

                // Step 6: Create a new Task entity
                Entity taskEntity = new Entity("task");

                // Step 7: Populate task fields
                taskEntity["subject"] = $"Follow-up: New Account - {accountName}";
                taskEntity["description"] = $"New account created.\n\nAccount Name: {accountName}\nIndustry: {accountIndustry}\nCreated Date: {DateTime.Now:yyyy-MM-dd}";
                
                // Link task to the account
                taskEntity["regardingobjectid"] = new EntityReference("account", accountId);

                // Set task owner (same as account owner)
                if (accountOwnerId != Guid.Empty)
                {
                    taskEntity["ownerid"] = new EntityReference("systemuser", accountOwnerId);
                }

                // Set priority to Normal
                taskEntity["prioritycode"] = new OptionSetValue(1); // 1 = Normal

                // Set due date to 3 days from now
                taskEntity["scheduledend"] = DateTime.Now.AddDays(3);

                // Step 8: Create the task record
                Guid taskId = orgService.Create(taskEntity);

                tracingService.Trace($"Task created successfully with ID: {taskId}");

                // Step 9: Optional - Log completion
                context.OutputParameters["TaskCreated"] = true;
                context.OutputParameters["TaskId"] = taskId;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                tracingService.Trace($"FaultException in AccountTaskCreationPlugin: {ex.Message}");
                throw new InvalidPluginExecutionException($"An error occurred in AccountTaskCreationPlugin: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace($"Exception in AccountTaskCreationPlugin: {ex.Message}\n{ex.StackTrace}");
                throw new InvalidPluginExecutionException($"An unexpected error occurred in AccountTaskCreationPlugin: {ex.Message}", ex);
            }
        }
    }
}
