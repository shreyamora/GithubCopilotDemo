using System;
using System;
using Microsoft.Xrm.Sdk;

namespace ClassLibrary1
{
    /// <summary>
    /// Dynamics 365 / Dataverse plugin that updates a numeric field on the record.
    /// Example behavior: increments the `new_numberfield` value by 1 when the plugin runs.
    /// Register this plugin on the appropriate message (Create/Update) and entity.
    /// </summary>
    public class UpdateNumberFieldPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context == null) return;

            // The plugin expects the Target to be an Entity (for Create/Update messages)
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
                return;

            var target = (Entity)context.InputParameters["Target"];

            try
            {
                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);

                const string fieldName = "new_numberfield"; // change to your schema name

                if (!target.Contains(fieldName))
                    return;

                // Handle different numeric types
                if (target[fieldName] is decimal dec)
                {
                    target[fieldName] = dec + 1m;
                }
                else if (target[fieldName] is int i)
                {
                    target[fieldName] = i + 1;
                }
                else if (target[fieldName] is long l)
                {
                    target[fieldName] = l + 1L;
                }
                else
                {
                    // not a supported numeric type
                    return;
                }

                // Update only the changed attribute
                var update = new Entity(target.LogicalName) { Id = target.Id };
                update[fieldName] = target[fieldName];
                service.Update(update);
            }
            catch (InvalidPluginExecutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("UpdateNumberFieldPlugin failed: " + ex.Message, ex);
            }
        }
    }
}
