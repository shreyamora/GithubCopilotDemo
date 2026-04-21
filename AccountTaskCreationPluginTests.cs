using System;
using Xunit;
using Moq;
using Microsoft.Xrm.Sdk;
using GithubCopilotDemo.Plugins;

namespace GithubCopilotDemo.Tests
{
    /// <summary>
    /// Unit tests for AccountTaskCreationPlugin
    /// </summary>
    public class AccountTaskCreationPluginTests
    {
        private readonly AccountTaskCreationPlugin _plugin;

        public AccountTaskCreationPluginTests()
        {
            _plugin = new AccountTaskCreationPlugin();
        }

        [Fact]
        public void Execute_WithValidAccountCreate_ShouldCreateTask()
        {
            // Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockPluginContext = new Mock<IPluginExecutionContext>();
            var mockServiceFactory = new Mock<IOrganizationServiceFactory>();
            var mockOrgService = new Mock<IOrganizationService>();
            var mockTracingService = new Mock<ITracingService>();

            // Setup context
            mockPluginContext.Setup(x => x.MessageName).Returns("Create");
            mockPluginContext.Setup(x => x.Stage).Returns(40); // Post-Operation

            var accountEntity = new Entity("account");
            accountEntity.Id = Guid.NewGuid();
            accountEntity["name"] = "Test Account";
            accountEntity["ownerid"] = new EntityReference("systemuser", Guid.NewGuid());

            mockPluginContext.Setup(x => x.InputParameters).Returns(new ParameterCollection
            {
                { "Target", accountEntity }
            });
            mockPluginContext.Setup(x => x.OutputParameters).Returns(new ParameterCollection());

            mockServiceFactory.Setup(x => x.CreateOrganizationService(It.IsAny<Guid?>()))
                .Returns(mockOrgService.Object);

            mockServiceProvider.Setup(x => x.GetService(typeof(IPluginExecutionContext)))
                .Returns(mockPluginContext.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(IOrganizationServiceFactory)))
                .Returns(mockServiceFactory.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(ITracingService)))
                .Returns(mockTracingService.Object);

            mockOrgService.Setup(x => x.Create(It.IsAny<Entity>()))
                .Returns(Guid.NewGuid());

            // Act
            _plugin.Execute(mockServiceProvider.Object);

            // Assert
            mockOrgService.Verify(x => x.Create(It.Is<Entity>(e => e.LogicalName == "task")), Times.Once);
        }

        [Fact]
        public void Execute_WithInvalidEntity_ShouldNotCreateTask()
        {
            // Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockPluginContext = new Mock<IPluginExecutionContext>();
            var mockTracingService = new Mock<ITracingService>();

            mockPluginContext.Setup(x => x.MessageName).Returns("Create");
            mockPluginContext.Setup(x => x.Stage).Returns(40);

            var contactEntity = new Entity("contact");
            contactEntity.Id = Guid.NewGuid();

            mockPluginContext.Setup(x => x.InputParameters).Returns(new ParameterCollection
            {
                { "Target", contactEntity }
            });

            mockServiceProvider.Setup(x => x.GetService(typeof(IPluginExecutionContext)))
                .Returns(mockPluginContext.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(ITracingService)))
                .Returns(mockTracingService.Object);

            // Act
            _plugin.Execute(mockServiceProvider.Object);

            // Assert
            mockTracingService.Verify(x => x.Trace(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Execute_WithEmptyAccountName_ShouldNotCreateTask()
        {
            // Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockPluginContext = new Mock<IPluginExecutionContext>();
            var mockServiceFactory = new Mock<IOrganizationServiceFactory>();
            var mockOrgService = new Mock<IOrganizationService>();
            var mockTracingService = new Mock<ITracingService>();

            mockPluginContext.Setup(x => x.MessageName).Returns("Create");
            mockPluginContext.Setup(x => x.Stage).Returns(40);

            var accountEntity = new Entity("account");
            accountEntity.Id = Guid.NewGuid();
            // Name is not set

            mockPluginContext.Setup(x => x.InputParameters).Returns(new ParameterCollection
            {
                { "Target", accountEntity }
            });

            mockServiceFactory.Setup(x => x.CreateOrganizationService(It.IsAny<Guid?>()))
                .Returns(mockOrgService.Object);

            mockServiceProvider.Setup(x => x.GetService(typeof(IPluginExecutionContext)))
                .Returns(mockPluginContext.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(IOrganizationServiceFactory)))
                .Returns(mockServiceFactory.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(ITracingService)))
                .Returns(mockTracingService.Object);

            // Act
            _plugin.Execute(mockServiceProvider.Object);

            // Assert
            mockOrgService.Verify(x => x.Create(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void Execute_WithWrongEventStage_ShouldNotProcess()
        {
            // Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockPluginContext = new Mock<IPluginExecutionContext>();
            var mockTracingService = new Mock<ITracingService>();

            mockPluginContext.Setup(x => x.MessageName).Returns("Create");
            mockPluginContext.Setup(x => x.Stage).Returns(30); // Pre-Operation (30) instead of Post-Operation (40)

            mockServiceProvider.Setup(x => x.GetService(typeof(IPluginExecutionContext)))
                .Returns(mockPluginContext.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(ITracingService)))
                .Returns(mockTracingService.Object);

            // Act
            _plugin.Execute(mockServiceProvider.Object);

            // Assert
            mockTracingService.Verify(x => x.Trace("Plugin invoked with wrong message or stage. Exiting."), Times.Once);
        }
    }
}
