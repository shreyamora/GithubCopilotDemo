# Account Task Creation - Implementation Guide

## Project Overview
This project implements automatic task creation in Dynamics 365 when a new account record is created.

## Files Included

### 1. **AccountTaskCreationPlugin.cs**
Main plugin implementation that handles the business logic.
- **Purpose**: Creates a task record linked to a newly created account
- **Entity**: Account
- **Event**: Create (Post-Operation)
- **Language**: C#

### 2. **PluginConfiguration.json**
Configuration file containing plugin registration details and task creation settings.
- Plugin registration information
- Task template and field mappings
- Error handling configuration

### 3. **PowerAutomateFlow.json**
Alternative implementation using Power Automate Cloud Flow (Low-code solution).
- Cloud flow definition
- Action and condition configurations
- Error handling setup

### 4. **AccountTaskCreationPluginTests.cs**
Unit tests for the plugin using xUnit and Moq.
- Test cases for successful task creation
- Validation tests
- Edge case handling

### 5. **DeploymentGuide.md** (this file)
Step-by-step guide for deploying the solution to Dynamics 365.

---

## Deployment Instructions

### Prerequisites
- Dynamics 365 organization access
- Plugin Registration Tool (PRT)
- Visual Studio 2019 or later
- .NET Framework 4.6.2 or higher
- Microsoft Dynamics CRM SDK

### Step 1: Build the Plugin
```bash
cd GithubCopilotDemo
dotnet build
```

### Step 2: Create Signed Assembly
```bash
# Ensure strong name key is generated
sn -k accounttask.snk

# Build release version
dotnet build -c Release
```

### Step 3: Register Plugin (Using Plugin Registration Tool)

1. Open Plugin Registration Tool
2. **Create New Connection** to your Dynamics 365 instance
3. **Register Assembly**:
   - Click "Register" → "Register New Assembly"
   - Select the compiled DLL: `GithubCopilotDemo.dll`
   - Choose: "Database" isolation mode
   - Check "Allow external connections"
   - Click "Register Selected Plugins"

4. **Register Plugin Step**:
   - Expand the registered assembly
   - Right-click plugin → "Register New Step"
   - Configure:
     - **Message**: Create
     - **Primary Entity**: account
     - **Event Pipeline Stage**: PostOperation (40)
     - **Execution Mode**: Synchronous
     - **User**: System
     - **Rank**: 1
     - **Filtering Attributes**: name, ownerid, industry

5. Click "Register New Step"

### Step 4: Test Plugin

1. In Dynamics 365, navigate to **Sales** → **Accounts**
2. Create a new account with:
   - **Name**: "Test Account"
   - **Owner**: (Select a user)
   - **Industry**: (Select an industry)
3. **Save** the account
4. Navigate to **Activities** → **Tasks**
5. Verify a new task was created with:
   - **Subject**: "Follow-up: New Account - Test Account"
   - **Regarding**: Links to the new account
   - **Owner**: Same as account owner
   - **Due Date**: 3 days from creation

### Step 5: Monitor Plugin Execution

1. Open **Settings** → **System** → **Plug-in Trace Log**
2. Review logs for the AccountTaskCreationPlugin
3. Check for any errors or warnings

---

## Configuration Details

### Task Creation Fields
| Field | Value | Description |
|-------|-------|-------------|
| Subject | Follow-up: New Account - {AccountName} | Task subject line |
| Description | New account created details | Auto-populated description |
| Regarding | Account ID | Links task to account |
| Owner | Account Owner | Inherited from account |
| Priority | Normal (1) | Task priority level |
| Due Date | CreateDate + 3 days | Due date for follow-up |
| Status | Open (1) | Task status |

### Error Handling
- **Logging**: All errors logged to Plugin Trace Log
- **Throw Exception**: Prevents account creation if task creation fails
- **Admin Notification**: Optional email notification on failure

---

## Alternative: Power Automate Deployment

If you prefer a low-code solution:

1. Go to **Power Automate** → **Cloud Flows**
2. Click **+ New** → **Automated cloud flow**
3. Name: "Create Task on Account Creation"
4. Trigger: "When a record is created" (Dynamics 365)
   - Environment: {Your Org}
   - Table: Accounts
5. Import actions from `PowerAutomateFlow.json`
6. Activate the flow

**Advantages of Power Automate**:
- No code compilation required
- Easy to modify without dev tools
- Built-in error handling
- Better for non-developers

**Advantages of Plugin**:
- Better performance
- Synchronous execution
- More control over logic
- Can prevent record creation

---

## Troubleshooting

### Issue: Task not created after account creation
**Solution**:
1. Verify plugin is registered and enabled
2. Check Plugin Trace Log for errors
3. Ensure account entity has name field populated
4. Check user permissions to create tasks

### Issue: Plugin registration fails
**Solution**:
1. Ensure assembly is signed with strong name key
2. Verify .NET Framework version compatibility
3. Check organization permissions
4. Try rebuilding the project

### Issue: Task created but fields are empty
**Solution**:
1. Verify field mappings in PluginConfiguration.json
2. Check if account required fields are populated
3. Review plugin code for null reference exceptions
4. Check Plugin Trace Log

---

## Performance Considerations

- **Execution Time**: ~200-500ms per account creation
- **Load Impact**: Minimal - runs asynchronously
- **Storage**: ~1KB per task record
- **Recommendation**: Monitor plugin performance if processing >1000 accounts/day

---

## Rollback Instructions

### To Disable Plugin:
1. Open Plugin Registration Tool
2. Select the plugin step
3. Click **Disable**

### To Unregister Plugin:
1. Select the plugin step
2. Click **Delete**
3. Select the assembly
4. Click **Delete**

---

## Support & Maintenance

- **Version**: 1.0.0
- **Last Updated**: 2026-04-17
- **Author**: Shreya Mora
- **Support Contact**: shreya.mora@company.com

---

## Related Documentation
- [Microsoft Dynamics 365 SDK Documentation](https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/)
- [Plugin Registration Tool Guide](https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/download-tools-nuget)
- [Task Entity Reference](https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/task)
- [Account Entity Reference](https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/account)
