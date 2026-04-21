# Account Task Creation Project

## Overview
This project implements automatic task record creation in Microsoft Dynamics 365 whenever a new account is created. It provides two implementation approaches: a C# Plugin and a Power Automate Cloud Flow.

## Project Structure

```
GithubCopilotDemo/
├── AccountTaskCreationPlugin.cs           # Main plugin implementation
├── TaskCreationConstants.cs               # Constants and configuration
├── AccountTaskCreationPluginTests.cs      # Unit tests
├── PluginConfiguration.json               # Plugin registration config
├── PowerAutomateFlow.json                 # Alternative Power Automate flow
├── DeploymentGuide.md                     # Step-by-step deployment guide
└── README.md                              # This file
```

## Files Description

### 1. AccountTaskCreationPlugin.cs
**Purpose**: Core business logic for automatic task creation
- Listens to account creation events (Post-Operation)
- Extracts account information (name, owner, industry)
- Creates a linked task record with pre-configured fields
- Handles errors and logging

**Key Features**:
- Synchronous execution for data consistency
- Task linked to account via "Regarding" field
- Task owner inherited from account owner
- Due date set 3 days from account creation
- Comprehensive error handling with tracing

### 2. TaskCreationConstants.cs
**Purpose**: Centralized configuration and constants
- Entity attribute names
- Message templates
- Logging messages
- Error messages
- Feature flags for easy customization

**Benefits**:
- Easy maintenance - change values in one place
- Type-safe constants
- Self-documenting code
- Support for future features

### 3. AccountTaskCreationPluginTests.cs
**Purpose**: Unit tests using xUnit and Moq framework
- Test: Successful task creation
- Test: Invalid entity handling
- Test: Empty account name validation
- Test: Wrong event stage filtering

**Coverage**:
- Happy path scenarios
- Edge cases
- Validation logic
- Error conditions

### 4. PluginConfiguration.json
**Purpose**: Plugin registration and configuration metadata
- Entity and message binding
- Stage (Post-Operation)
- Task field templates
- Error handling policy

**Usage**: Referenced during plugin registration in Plugin Registration Tool

### 5. PowerAutomateFlow.json
**Purpose**: Low-code alternative implementation
- Cloud flow definition format
- Trigger on account creation
- Condition checking
- Task creation action
- Error handling

**Usage**: Import into Power Automate to create cloud flow without code

### 6. DeploymentGuide.md
**Purpose**: Comprehensive deployment documentation
- Prerequisites
- Step-by-step deployment instructions
- Configuration details
- Testing procedures
- Troubleshooting guide
- Rollback instructions

**Sections**:
- Build and compilation
- Plugin registration
- Testing scenarios
- Performance considerations
- Maintenance procedures

## Implementation Options

### Option 1: C# Plugin (Recommended for Production)
- **Pros**: 
  - Faster execution
  - Synchronous processing
  - More control over business logic
  - Prevents account creation if task fails
  
- **Cons**:
  - Requires compilation and deployment
  - Needs Visual Studio
  - More complex maintenance

**Deployment Time**: 2-4 hours including testing

### Option 2: Power Automate Cloud Flow (Low-Code)
- **Pros**:
  - No compilation needed
  - Can modify without deployment
  - Better for non-developers
  - Visual designer
  
- **Cons**:
  - Slower than plugin
  - Asynchronous execution
  - May not prevent account creation

**Deployment Time**: 30-60 minutes

## Installation Steps

### Quick Start (10 minutes)
1. Clone repository
2. Review DeploymentGuide.md
3. Choose implementation approach
4. Follow deployment instructions

### Full Installation with Testing (2-4 hours)
1. Follow all steps in DeploymentGuide.md
2. Run unit tests
3. Deploy to test environment
4. Execute test scenarios
5. Deploy to production
6. Monitor plugin execution

## Configuration

### Customize Task Fields
Edit `TaskCreationConstants.cs`:
```csharp
public const int DAYS_UNTIL_DUE = 3;  // Change due date offset
public const int TASK_PRIORITY_NORMAL = 1;  // Change priority
```

### Customize Task Subject/Description
Edit templates in `TaskCreationConstants.cs`:
```csharp
const string SUBJECT_TEMPLATE = "Follow-up: New Account - {0}";
const string DESCRIPTION_TEMPLATE = "New account created...";
```

## Testing

### Run Unit Tests
```bash
dotnet test
```

### Manual Testing in Dynamics 365
1. Create new account with name, owner, industry
2. Save account
3. Verify task created in Activities → Tasks
4. Check task is linked to account
5. Verify task owner matches account owner

## Troubleshooting

### Common Issues

**Task not created:**
- Check Plugin Trace Log
- Verify account has name field
- Ensure user has task creation permissions

**Plugin registration fails:**
- Use strong-named assembly
- Check .NET Framework compatibility
- Verify organization permissions

**Performance issues:**
- Monitor trace logs
- Check for exceptions
- Verify account creation volume

See DeploymentGuide.md for detailed troubleshooting

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-04-17 | Initial release - Basic task creation on account creation |

## Support

- **Maintainer**: Shreya Mora
- **Contact**: shreya.mora@company.com
- **Issues**: Submit via project board or contact maintainer

## License

This project is proprietary and confidential.

## Resources

- [Dynamics 365 SDK Documentation](https://docs.microsoft.com/dynamics365/developer/)
- [Plugin Development Guide](https://docs.microsoft.com/dynamics365/customer-engagement/developer/)
- [Task Entity Reference](https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/task)
- [Power Automate Documentation](https://docs.microsoft.com/en-us/power-automate/)

## Next Steps

1. Review DeploymentGuide.md
2. Choose C# Plugin or Power Automate
3. Follow deployment steps
4. Test in environment
5. Monitor production
6. Iterate based on feedback

---

**Last Updated**: 2026-04-17  
**Status**: Ready for Deployment
