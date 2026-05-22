# MedManage API - Postman Test Collection

Comprehensive API test suite for MedManage Member endpoints.

## Test Coverage

### CRUD Operations (8 tests)
1. **Create Member** - Creates a test member with validation
2. **Get Member by ID** - Retrieves single member
3. **Update Member** - Updates member and verifies audit trail
4. **Search Members (No Filters)** - Tests pagination
5. **Search Members (With Filters)** - Tests filtered search
6. **Delete Member** - Soft delete functionality
7. **Verify Soft Delete** - Confirms member is hidden after delete
8. **HEAD Request** - Checks member existence

### Validation Tests (3 tests)
- Missing required fields validation
- Invalid member ID handling
- Non-existent member update

### Performance Tests (2 tests)
- Large page size search performance
- Single member retrieval performance

**Total: 13 test cases with 50+ assertions**

---

## Prerequisites

### Option 1: Install Newman Globally
```powershell
npm install -g newman
```

### Option 2: Use npx (No installation required)
```powershell
# Newman will be downloaded on first run
npx newman run ...
```

### Optional: Install Newman HTML Reporter
```powershell
npm install -g newman-reporter-htmlextra
```

---

## Running Tests

### Quick Start (PowerShell)
```powershell
# From the MedManage.Modern directory
cd postman
.\run-tests.ps1
```

### Manual Execution

#### Basic Run
```powershell
newman run MedManage.postman_collection.json -e MedManage.postman_environment.json
```

#### With HTML Report
```powershell
newman run MedManage.postman_collection.json `
  -e MedManage.postman_environment.json `
  -r htmlextra `
  --reporter-htmlextra-export ./reports/test-report.html
```

#### Run Specific Folder
```powershell
# Run only CRUD operations
newman run MedManage.postman_collection.json `
  -e MedManage.postman_environment.json `
  --folder "Member CRUD Operations"
```

#### Verbose Output
```powershell
newman run MedManage.postman_collection.json `
  -e MedManage.postman_environment.json `
  --verbose
```

#### Disable SSL Verification (Development)
```powershell
newman run MedManage.postman_collection.json `
  -e MedManage.postman_environment.json `
  --insecure
```

---

## Environment Configuration

The environment file (`MedManage.postman_environment.json`) contains:

| Variable | Default Value | Description |
|----------|---------------|-------------|
| `base_url` | `https://localhost:58764` | API base URL |
| `created_member_id` | (dynamic) | Populated during test run |
| `test_member_number` | (dynamic) | Generated unique member number |
| `existing_member_id` | `16819` | Known valid member ID for tests |

### Using Different Environments

Create additional environment files for different environments:

```json
// MedManage.postman_environment.staging.json
{
  "name": "MedManage API - Staging",
  "values": [
    {
      "key": "base_url",
      "value": "https://staging-api.medmanage.com"
    }
  ]
}
```

Run with:
```powershell
newman run MedManage.postman_collection.json -e MedManage.postman_environment.staging.json
```

---

## CI/CD Integration

### GitHub Actions Example
```yaml
name: API Tests

on: [push, pull_request]

jobs:
  api-tests:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '18'
      - name: Install Newman
        run: npm install -g newman newman-reporter-htmlextra
      - name: Run API Tests
        run: |
          cd postman
          newman run MedManage.postman_collection.json -e MedManage.postman_environment.json -r htmlextra --insecure
      - name: Upload Test Results
        uses: actions/upload-artifact@v3
        if: always()
        with:
          name: test-results
          path: postman/reports/
```

### Azure DevOps Example
```yaml
steps:
- task: NodeTool@0
  inputs:
    versionSpec: '18.x'
  displayName: 'Install Node.js'

- script: npm install -g newman newman-reporter-htmlextra
  displayName: 'Install Newman'

- script: |
    cd postman
    newman run MedManage.postman_collection.json -e MedManage.postman_environment.json -r htmlextra --insecure
  displayName: 'Run API Tests'

- task: PublishTestResults@2
  inputs:
    testResultsFormat: 'JUnit'
    testResultsFiles: '**/newman-*.xml'
```

---

## Test Results

### Console Output
Newman provides detailed console output:
```
→ Member CRUD Operations
  ✓ Create Member - Valid
    ✓ Status code is 200
    ✓ Response has success=true
    ✓ Response contains member data
    ✓ Member has audit fields populated
    ✓ DateDeleted is null for new member

┌─────────────────────────┬────────────┬────────────┐
│                         │   executed │     failed │
├─────────────────────────┼────────────┼────────────┤
│              iterations │          1 │          0 │
├─────────────────────────┼────────────┼────────────┤
│                requests │         13 │          0 │
├─────────────────────────┼────────────┼────────────┤
│            test-scripts │         13 │          0 │
├─────────────────────────┼────────────┼────────────┤
│      prerequest-scripts │         13 │          0 │
├─────────────────────────┼────────────┼────────────┤
│              assertions │         50 │          0 │
├─────────────────────────┴────────────┴────────────┤
│ total run duration: 3.2s                          │
├───────────────────────────────────────────────────┤
│ total data received: 125.5KB (approx)             │
├───────────────────────────────────────────────────┤
│ average response time: 245ms                      │
└───────────────────────────────────────────────────┘
```

### HTML Report (with htmlextra)
Beautiful HTML reports with:
- Request/Response details
- Test results with pass/fail status
- Performance metrics
- Environment variables used
- Request history

Access at: `./reports/test-report.html`

---

## Troubleshooting

### SSL Certificate Errors
```powershell
# Add --insecure flag for self-signed certificates
newman run MedManage.postman_collection.json -e MedManage.postman_environment.json --insecure
```

### API Not Running
Ensure the API is running on https://localhost:58764:
```powershell
cd ..\src\MedManage.API
dotnet run
```

### Connection Timeout
Increase timeout in collection or via CLI:
```powershell
newman run MedManage.postman_collection.json `
  -e MedManage.postman_environment.json `
  --timeout-request 10000
```

### Database State Issues
Some tests create data. Clean up test data:
```sql
DELETE FROM shared.Member WHERE MemberNumber LIKE 'TEST_%'
```

---

## Extending Tests

### Adding New Test Cases

1. Import collection into Postman UI
2. Add new request to appropriate folder
3. Add test scripts:
   ```javascript
   pm.test("Your test description", function () {
       var jsonData = pm.response.json();
       pm.expect(jsonData.success).to.eql(true);
   });
   ```
4. Export collection (overwrite existing file)

### Available Test Functions

```javascript
// Status codes
pm.response.to.have.status(200);
pm.expect(pm.response.code).to.be.oneOf([200, 204]);

// Response structure
pm.response.to.be.json;
pm.expect(jsonData).to.be.an('object');
pm.expect(jsonData.data).to.exist;

// Value assertions
pm.expect(value).to.eql(expected);
pm.expect(value).to.be.a('string');
pm.expect(value).to.be.above(10);
pm.expect(array).to.have.lengthOf(5);
pm.expect(string).to.include('substring');

// Performance
pm.expect(pm.response.responseTime).to.be.below(500);

// Set environment variables
pm.environment.set('variable_name', value);
pm.environment.get('variable_name');
```

---

## Best Practices

1. **Run before commits** - Ensure changes don't break existing functionality
2. **Use in CI/CD** - Automated testing on every build
3. **Monitor performance** - Track response times over time
4. **Update tests** - Keep tests in sync with API changes
5. **Clean test data** - Ensure tests can run multiple times
6. **Version control** - Commit collection and environment files

---

## Additional Resources

- [Newman Documentation](https://learning.postman.com/docs/running-collections/using-newman-cli/command-line-integration-with-newman/)
- [Postman Test Scripts](https://learning.postman.com/docs/writing-scripts/test-scripts/)
- [Newman Reporters](https://learning.postman.com/docs/running-collections/using-newman-cli/newman-reporters/)
- [ChaiJS Assertions](https://www.chaijs.com/api/bdd/)

---

## Support

For issues or questions:
- Check API logs in Visual Studio or dotnet console
- Verify database connectivity
- Review test assertions for failures
- Check Newman version: `newman --version`
