# Implementation Plan: Tariff Percentage Management

## Overview

This plan implements the Tariff Percentage Management feature end-to-end: a new database entity and migration, a service layer with CRUD + propagation logic, a channel-based background processor, a secured API controller, and an Angular admin UI with job status polling. Property-based tests using FsCheck validate correctness properties defined in the design.

## Tasks

- [x] 1. Set up data layer and entity model
  - [x] 1.1 Create TariffPercentage entity and database configuration
    - Create `TariffPercentage` entity class in the domain/entities folder under the Tariff schema
    - Include all properties: TariffPercentageId, PercentageAdded (decimal(10,4)), TariffPeriodName (int), StartActiveDate (DateOnly), EndActiveDate (DateOnly?), Status (string, max 50), RecordsAffected (int?), Notes (string, max 500)
    - Inherit from `BaseEntity` to get audit fields (DateInserted, UserID, DateUpdated, UpdatedUserID, DateDeleted)
    - Add `[Table("TariffPercentage", Schema = "Tariff")]` attribute
    - Configure EF entity mapping with proper column types and constraints
    - _Requirements: 1.1, 11.1_

  - [x] 1.2 Create EF migration for Tariff.TariffPercentage table
    - Add DbSet<TariffPercentage> to the application DbContext
    - Generate EF Core migration creating the `[Tariff].[TariffPercentage]` table
    - Add index on `(TariffPeriodName, EndActiveDate)` for propagation query performance
    - Add index on `(TariffPeriodName, StartActiveDate)` for overlap detection
    - _Requirements: 1.1, 11.1_

  - [x] 1.3 Create DTOs and request/response models
    - Create `TariffPercentageDto` with all display fields
    - Create `CreateTariffPercentageDto` with DataAnnotation validation attributes ([Range], [Required], [MaxLength])
    - Create `UpdateTariffPercentageDto` with optional fields and validation
    - Create `TariffUpdateJob` internal model with JobId (Guid), TariffPercentageId, PercentageAdded, TariffPeriodName, StartActiveDate, EndActiveDate, QueuedAt
    - Create `TariffUpdateJobStatus` model with JobId, Status, RecordsAffected, ErrorMessage, StartedAt, CompletedAt
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 3.3_

- [x] 2. Implement service layer and business logic
  - [x] 2.1 Create ITariffPercentageService interface and implementation - CRUD operations
    - Define `ITariffPercentageService` interface with GetAllAsync, GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync methods
    - Implement `TariffPercentageService` with repository pattern
    - Implement CreateAsync: validate input, check for overlapping date ranges for same period, create record with Status="Pending", record authenticated user ID
    - Implement GetAllAsync: return all non-deleted records sorted by TariffPeriodName desc, StartActiveDate desc
    - Implement GetByIdAsync: return single non-deleted record or null
    - Implement UpdateAsync: validate record exists and is in "Pending" or "Failed" status, apply changes, check overlap constraints
    - Implement DeleteAsync: validate status is "Pending" or "Failed", soft-delete by setting DateDeleted
    - Register service in DI container
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 2.1, 2.2, 2.3, 3.1, 3.2, 3.3, 3.4, 3.5, 4.1, 4.2, 4.3, 9.5_

  - [x] 2.2 Write property test for input validation rejection (Property 7)
    - **Property 7: Input Validation Rejection**
    - Use FsCheck to generate arbitrary CreateTariffPercentageDto and UpdateTariffPercentageDto values with invalid data (PercentageAdded <= 0 or > 9999.9999, TariffPeriodName outside 2000-2100, EndActiveDate < StartActiveDate)
    - Assert that for all invalid inputs, CreateAsync and UpdateAsync reject the request with a validation error and no record is persisted
    - **Validates: Requirements 1.2, 1.3, 1.4, 3.3**

  - [x] 2.3 Write property test for valid state transitions (Property 8)
    - **Property 8: Valid State Transitions**
    - Use FsCheck to generate sequences of operations (create, update, delete, apply) on TariffPercentage records
    - Assert that Status transitions only through valid paths: creation → "Pending"; updates/deletes only in "Pending"/"Failed"; apply → "Processing"; success → "Completed"; failure → "Failed"
    - Assert that operations on records in invalid states are rejected
    - **Validates: Requirements 1.1, 3.1, 3.2, 4.1, 4.2, 5.1, 7.3**

  - [x] 2.4 Implement ApplyPercentageAsync and job queueing
    - Inject `Channel<TariffUpdateJob>` into TariffPercentageService
    - Implement ApplyPercentageAsync: validate record exists with Status "Pending" or "Failed", check no "Processing" job for same TariffPeriodName, set Status to "Processing", write TariffUpdateJob to channel, return TariffUpdateJobStatus with Status="Queued"
    - Reject with 409 if record is "Processing" or another job is processing for same period
    - Reject with error if record is "Completed"
    - Implement GetJobStatusAsync: look up in-memory or persisted job status by jobId
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6_

  - [x] 2.5 Write property test for idempotency guard (Property 4)
    - **Property 4: Idempotency Guard**
    - Use FsCheck to generate sequences of apply requests for the same TariffPercentage or same TariffPeriodName
    - Assert that applying a "Completed" record or applying when another job is "Processing" for the same period is always rejected
    - Assert no duplicate ServiceProvider_Tariff records can be created through repeated apply attempts
    - **Validates: Requirements 5.2, 5.3, 5.5, 11.2, 11.3**

  - [x] 2.6 Implement GetActivePercentagesForLetterAsync for case letter support
    - Query completed, non-deleted TariffPercentage records
    - Group by TariffPeriodName, take top 2 years (highest values)
    - For each year, select record with latest EndActiveDate (null = highest priority)
    - Order results by TariffPeriodName descending
    - Return empty collection if no completed records exist
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5_

  - [x] 2.7 Write property test for case letter two-year window (Property 5)
    - **Property 5: Case Letter Two-Year Window**
    - Use FsCheck to generate arbitrary collections of completed TariffPercentage records with varying years and end dates
    - Assert GetActivePercentagesForLetterAsync returns at most 2 records with distinct TariffPeriodName values representing the two most recent years
    - Assert null EndActiveDate takes highest priority per year
    - Assert results are ordered by TariffPeriodName descending
    - **Validates: Requirements 8.1, 8.2, 8.5**

- [x] 3. Checkpoint - Ensure service layer tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [x] 4. Implement background processor
  - [x] 4.1 Set up Channel<TariffUpdateJob> and register as singleton
    - Create bounded Channel<TariffUpdateJob> with appropriate capacity (e.g., 100)
    - Register ChannelWriter<TariffUpdateJob> and ChannelReader<TariffUpdateJob> in DI
    - Configure channel options (BoundedChannelFullMode.Wait)
    - _Requirements: 5.1, 6.1_

  - [x] 4.2 Implement TariffPercentageProcessor as BackgroundService
    - Create `TariffPercentageProcessor` inheriting from `BackgroundService`
    - Inject ChannelReader<TariffUpdateJob>, IServiceScopeFactory, ILogger
    - Implement ExecuteAsync: continuously read from channel, process each job in a new DI scope
    - Implement ProcessTariffUpdateJob: begin transaction, close prior period records (SET EndActiveDate = StartActiveDate - 1 day WHERE TariffPeriodName = job.Year - 1 AND EndActiveDate IS NULL), insert new records from prior period template with job's percentage and dates
    - Handle "no prior period data" case: complete with RecordsAffected = 0
    - Check for existing records before inserting (skip duplicates per requirement 11.2)
    - On success: commit transaction, update TariffPercentage status to "Completed", set RecordsAffected
    - On failure: rollback transaction, update status to "Failed", store truncated error in Notes
    - Set extended command timeout (300s) for bulk operations
    - Register as hosted service in DI
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5, 6.6, 6.7, 11.1, 11.2, 11.4_

  - [-] 4.3 Write property test for propagation completeness (Property 1)
    - **Property 1: Percentage Propagation Completeness**
    - Use FsCheck to generate arbitrary sets of ServiceProvider_Tariff records for a prior period
    - Assert that after successful propagation, the count of newly inserted records equals the count of distinct (ServiceProviderId, TariffNameId, MainClientId) combinations from the prior period
    - **Validates: Requirements 6.2, 6.3**

  - [-] 4.4 Write property test for date continuity (Property 2)
    - **Property 2: Date Continuity**
    - Use FsCheck to generate propagation jobs and prior period records
    - Assert that for every (ServiceProviderId, TariffNameId, MainClientId) combination, the EndActiveDate of the closed prior record equals job.StartActiveDate minus one day
    - Assert no date gaps or overlaps between consecutive periods
    - **Validates: Requirements 6.1, 11.1**

  - [-] 4.5 Write property test for atomicity (Property 3)
    - **Property 3: Atomicity**
    - Use FsCheck to generate propagation jobs and simulate failures at various points
    - Assert that on any failure, zero records in ServiceProvider_Tariff are modified (full rollback)
    - Assert TariffPercentage status is "Failed" with error in Notes
    - **Validates: Requirements 6.4, 6.5**

- [x] 5. Checkpoint - Ensure background processor tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [x] 6. Implement API controller
  - [-] 6.1 Create TariffPercentageController with CRUD endpoints
    - Create controller with `[ApiController]`, `[Route("api/admin/tariff-percentages")]`, `[Authorize(Roles = "SystemAdmin")]`
    - Implement `[HttpGet]` - GetAll: return all percentages wrapped in ApiResponse
    - Implement `[HttpGet("{id}")]` - GetById: return single percentage or 404
    - Implement `[HttpPost]` - Create: validate model, call service, return 201 Created
    - Implement `[HttpPut("{id}")]` - Update: validate, call service, return 200 or appropriate error
    - Implement `[HttpDelete("{id}")]` - Delete: call service, return 204 or appropriate error
    - Implement `[HttpPost("{id}/apply")]` - Apply: trigger propagation, return 202 Accepted with jobId
    - Implement `[HttpGet("jobs/{jobId}")]` - GetJobStatus: return job status or 404, validate jobId is valid GUID format (400 if not)
    - Implement `[HttpGet("active-for-letter")]` - GetActiveForLetter: return case letter percentages
    - Handle exceptions and return appropriate ApiResponse error wrappers
    - _Requirements: 2.1, 2.2, 2.3, 5.4, 7.1, 7.2, 7.4, 8.1, 9.1, 9.2, 9.3_

  - [x] 6.2 Write property test for authorization invariant (Property 6)
    - **Property 6: Authorization Invariant**
    - Use FsCheck to generate arbitrary HTTP requests to tariff percentage endpoints from users without SystemAdmin role
    - Assert that all such requests return 403 Forbidden
    - Assert unauthenticated requests return 401 Unauthorized
    - **Validates: Requirements 9.1, 9.2**

  - [x] 6.3 Write unit tests for controller endpoint responses
    - Test 202 Accepted response for apply endpoint
    - Test 409 Conflict for duplicate period and concurrent apply
    - Test 404 for non-existent records and jobs
    - Test 400 for invalid jobId format
    - Test successful CRUD operations return correct status codes
    - _Requirements: 2.3, 3.4, 5.2, 5.3, 7.2, 7.4_

- [x] 7. Implement Angular admin UI
  - [x] 7.1 Create Angular service and models for tariff percentage API
    - Create `TariffPercentage` interface matching the API response shape
    - Create `CreateTariffPercentageRequest` and `TariffUpdateJobStatus` interfaces
    - Create `TariffPercentageApiService` with methods: getAll(), getById(id), create(dto), update(id, dto), delete(id), apply(id), getJobStatus(jobId)
    - Use HttpClient with proper base URL (`/api/admin/tariff-percentages`)
    - Return Observable<ApiResponse<T>> for all methods
    - _Requirements: 10.1, 10.2_

  - [x] 7.2 Create tariff percentage management component with table display
    - Create standalone component `TariffPercentageManagementComponent`
    - Implement table displaying all percentages: Period, Start Date, End Date, Percentage, Status, Records Affected
    - Sort by TariffPeriodName desc, StartActiveDate desc
    - Show status badges (Pending, Processing, Completed, Failed) with appropriate styling
    - Load data on component init via the API service
    - _Requirements: 10.1, 10.5, 10.6, 10.7_

  - [x] 7.3 Implement create/edit form with client-side validation
    - Add inline form or dialog for creating new tariff percentage entries
    - Include fields: PercentageAdded (number, > 0), TariffPeriodName (4-digit year 2000-2100), StartActiveDate (date picker), EndActiveDate (optional date picker), Notes (textarea, max 500 chars)
    - Implement client-side validation with field-level error messages
    - Prevent submission if validation fails
    - Handle server-side errors (409 Conflict for overlap) and display to user
    - _Requirements: 10.2, 10.8_

  - [x] 7.4 Implement Apply button with confirmation dialog and job status polling
    - Add "Apply" button on each table row, enabled only for "Pending" or "Failed" status
    - Show confirmation dialog before triggering propagation (display period and percentage)
    - On confirm: call apply endpoint, receive jobId, start polling
    - Poll job status every 5 seconds while status is "Processing"
    - Display progress indicator on the row during processing
    - On completion: stop polling, update row with final status and RecordsAffected
    - On failure: stop polling, display error message
    - On cancel: dismiss dialog, take no action
    - _Requirements: 10.3, 10.4, 10.5, 10.6, 10.7_

  - [x] 7.5 Add routing and role guard for SystemAdmin access
    - Add route `/admin/tariff-percentages` in admin routing module
    - Apply `roleGuard` with SystemAdmin role to the route
    - Add navigation entry in admin menu for authorized users
    - Redirect unauthorized users to default route
    - _Requirements: 9.4_

- [x] 8. Letter template integration and field mapping
  - [x] 8.1 Add tariff percentage fields to GatherCaseLetterDataAsync
    - In `LetterTemplateService.GatherCaseLetterDataAsync`, inject or call `ITariffPercentageService.GetActivePercentagesForLetterAsync()`
    - Map results to Handlebars-compatible dictionary entries: `TariffPercentageCurrentYear` (int/string), `TariffPercentageCurrentYearValue` (formatted decimal, 2dp), `TariffPercentagePriorYear`, `TariffPercentagePriorYearValue`
    - Handle empty results gracefully (set fields to empty string so Handlebars does not error)
    - _Requirements: 11.1, 11.2, 11.3, 11.4_

  - [x] 8.2 Create data migration to update letter templates with Handlebars placeholders
    - Create a new EF Core data migration (or SQL script executed via migration)
    - Update all `shared.LetterTemplate` rows where `HtmlContent` contains hardcoded tariff percentage strings (e.g., "NHRPL+ 233.90% for 2025", "NHRPL+ 250.60% for 2026")
    - Replace hardcoded lines with Handlebars template expressions: `Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentagePriorYearValue}}% for {{TariffPercentagePriorYear}}` and `Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentageCurrentYearValue}}% for {{TariffPercentageCurrentYear}}`
    - Ensure the migration is idempotent (only replaces if the hardcoded text still exists)
    - _Requirements: 11.5_

  - [x] 8.3 Update default HTML template file for new deployments
    - Update the CaseLetter_Default.html file (or equivalent seed template) to use the new Handlebars placeholders instead of hardcoded percentages
    - Ensure both the non-medical-scheme and referral letter sections use the dynamic fields
    - _Requirements: 11.5, 11.6_

  - [x] 8.4 Write integration test for letter template field mapping
    - Create test that verifies GatherCaseLetterDataAsync includes the tariff percentage fields
    - Test with completed TariffPercentage records: verify fields contain correct year and value
    - Test with no completed records: verify fields are empty strings (no rendering error)
    - Test rendered HTML output contains the dynamically inserted percentage values in the expected format
    - _Requirements: 11.1, 11.2, 11.3, 11.4, 11.6_

- [x] 9. Checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [x] 10. Integration wiring and final verification
  - [x] 10.1 Wire all components together and verify end-to-end flow
    - Ensure DI registrations are complete: ITariffPercentageService, Channel, BackgroundService, Controller
    - Verify EF migration applies cleanly (both schema and data migrations)
    - Verify Angular module imports and lazy loading configuration
    - Ensure API route prefix and Angular proxy/base URL align
    - _Requirements: 1.1, 5.1, 6.1, 9.1_

  - [x] 10.2 Write integration tests for full propagation flow
    - Test Create → Apply → Background Process → Verify ServiceProvider_Tariff records
    - Test concurrent apply rejection returns 409
    - Test transaction rollback on simulated database failure
    - Test case letter query returns correct two-year window from real data
    - Test authorization enforcement (SystemAdmin access granted, non-admin rejected)
    - _Requirements: 5.1, 6.2, 6.3, 6.5, 8.1, 9.1, 9.2, 12.1_

  - [x] 10.3 Write property test for audit trail completeness (Property 9)
    - **Property 9: Audit Trail Completeness**
    - Use FsCheck to generate arbitrary create and update operations with authenticated users
    - Assert that for every create or update, the authenticated user ID is recorded on the resulting record
    - **Validates: Requirements 9.5**

- [x] 11. Final checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- Property tests validate universal correctness properties using FsCheck (.NET)
- Unit tests validate specific examples and edge cases
- The backend uses C# / ASP.NET Core 8 with Entity Framework Core
- The frontend uses Angular 17+ with standalone components
- Background processing uses System.Threading.Channels (no external dependencies)
- All raw SQL uses parameterized queries to prevent SQL injection
- Extended command timeout (300s) is configured for bulk propagation operations

## Task Dependency Graph

```json
{
  "waves": [
    { "id": 0, "tasks": ["1.1", "1.3"] },
    { "id": 1, "tasks": ["1.2"] },
    { "id": 2, "tasks": ["2.1", "4.1"] },
    { "id": 3, "tasks": ["2.2", "2.3", "2.4", "2.6"] },
    { "id": 4, "tasks": ["2.5", "2.7", "4.2"] },
    { "id": 5, "tasks": ["4.3", "4.4", "4.5", "6.1"] },
    { "id": 6, "tasks": ["6.2", "6.3", "7.1"] },
    { "id": 7, "tasks": ["7.2", "7.5"] },
    { "id": 8, "tasks": ["7.3", "7.4", "8.1"] },
    { "id": 9, "tasks": ["8.2", "8.3"] },
    { "id": 10, "tasks": ["8.4", "10.1"] },
    { "id": 11, "tasks": ["10.2", "10.3"] }
  ]
}
```
