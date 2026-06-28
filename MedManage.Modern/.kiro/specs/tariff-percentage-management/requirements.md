# Requirements Document

## Introduction

This document specifies the requirements for the Tariff Percentage Management feature, which replaces manual SQL-based tariff percentage updates with an admin UI and automated background processing. System administrators manage tariff percentages through a dedicated interface, and the system propagates changes to the ServiceProvider_Tariff table via a background job. Case letters dynamically pull tariff percentages for the most recent two years.

## Glossary

- **System_Admin**: A user with the SystemAdmin role who is authorized to manage tariff percentages
- **Tariff_Percentage_Service**: The backend service responsible for CRUD operations on tariff percentage records and triggering propagation jobs
- **Background_Processor**: The IHostedService (TariffPercentageProcessor) that reads jobs from the channel queue and executes bulk database updates
- **Admin_UI**: The Angular frontend component that provides the tariff percentage management interface
- **API_Controller**: The ASP.NET Core controller exposing tariff percentage management endpoints
- **Propagation_Job**: A unit of work queued to the background processor that applies a tariff percentage to the ServiceProvider_Tariff table
- **TariffPercentage_Record**: A row in the Tariff.TariffPercentage table representing a managed percentage for a specific period
- **ServiceProvider_Tariff_Record**: A row in the Tariff.ServiceProvider_Tariff table representing an active tariff for a specific provider/tariff/client combination
- **Case_Letter_Generator**: The existing system component that generates case letters requiring tariff percentage data

## Requirements

### Requirement 1: Create Tariff Percentage

**User Story:** As a System Admin, I want to create new tariff percentage entries with period, dates, and percentage values, so that I can define tariff rates for upcoming periods without running manual SQL.

#### Acceptance Criteria

1. WHEN a System_Admin submits a tariff percentage with a PercentageAdded between 0.0001 and 9999.9999, a TariffPeriodName that is a 4-digit year between 2000 and 2100, a StartActiveDate, and an optional EndActiveDate that is on or after StartActiveDate, THE Tariff_Percentage_Service SHALL create a new TariffPercentage_Record with Status set to "Pending" and the authenticated user's ID recorded on the record
2. IF the submitted PercentageAdded is less than 0.0001 or greater than 9999.9999 or exceeds decimal(10,4) precision, THEN THE Tariff_Percentage_Service SHALL reject the request with a validation error and no record SHALL be created
3. IF the submitted TariffPeriodName is not a 4-digit year between 2000 and 2100, THEN THE Tariff_Percentage_Service SHALL reject the request with a validation error and no record SHALL be created
4. IF the submitted EndActiveDate is earlier than the StartActiveDate, THEN THE Tariff_Percentage_Service SHALL reject the request with a validation error and no record SHALL be created
5. IF a TariffPercentage_Record already exists for the same TariffPeriodName where the existing record's date range (StartActiveDate to EndActiveDate, treating a null EndActiveDate as open-ended) overlaps with the submitted date range, THEN THE Tariff_Percentage_Service SHALL reject the request with a 409 Conflict response and no record SHALL be created
6. IF the submitted StartActiveDate is missing, THEN THE Tariff_Percentage_Service SHALL reject the request with a validation error and no record SHALL be created

### Requirement 2: Read and List Tariff Percentages

**User Story:** As a System Admin, I want to view all existing tariff percentage entries, so that I can review current and historical tariff configurations.

#### Acceptance Criteria

1. WHEN a System_Admin requests the list of tariff percentages, THE API_Controller SHALL return all non-deleted TariffPercentage_Records with their TariffPercentageId, PercentageAdded, TariffPeriodName, StartActiveDate, EndActiveDate, Status, RecordsAffected, Notes, DateInserted, and UserID, sorted by TariffPeriodName descending then StartActiveDate descending
2. WHEN a System_Admin requests a specific tariff percentage by ID and the record exists and is not soft-deleted, THE API_Controller SHALL return the matching TariffPercentage_Record with all fields
3. IF a System_Admin requests a specific tariff percentage by ID and the record does not exist or has been soft-deleted, THEN THE API_Controller SHALL return a 404 Not Found response

### Requirement 3: Update Tariff Percentage

**User Story:** As a System Admin, I want to update an existing tariff percentage entry, so that I can correct values before propagation is triggered.

#### Acceptance Criteria

1. WHEN a System_Admin submits an update to a TariffPercentage_Record that has Status "Pending" or "Failed", THE Tariff_Percentage_Service SHALL apply the changes to the updatable fields (PercentageAdded, StartActiveDate, EndActiveDate, Notes), retain the current Status, persist the changes, and return the updated record
2. IF a System_Admin submits an update to a TariffPercentage_Record that has Status "Processing" or "Completed", THEN THE Tariff_Percentage_Service SHALL reject the update with a validation error indicating the record cannot be modified in its current state
3. IF the updated values violate validation rules (PercentageAdded less than 0.0001 or greater than 9999.9999, EndActiveDate earlier than StartActiveDate), THEN THE Tariff_Percentage_Service SHALL reject the update with specific validation error messages and no changes SHALL be persisted
4. IF a System_Admin submits an update for a TariffPercentageId that does not exist or has been soft-deleted, THEN THE Tariff_Percentage_Service SHALL return a 404 Not Found response
5. IF the updated date range would overlap with another existing TariffPercentage_Record for the same TariffPeriodName, THEN THE Tariff_Percentage_Service SHALL reject the update with a 409 Conflict response

### Requirement 4: Delete Tariff Percentage

**User Story:** As a System Admin, I want to delete a tariff percentage entry that is no longer needed, so that I can keep the management table clean.

#### Acceptance Criteria

1. WHEN a System_Admin requests deletion of a TariffPercentage_Record with Status "Pending" or "Failed", THE Tariff_Percentage_Service SHALL soft-delete the record by setting DateDeleted to the current UTC timestamp and return a success response
2. IF a System_Admin requests deletion of a TariffPercentage_Record with Status "Processing" or "Completed", THEN THE Tariff_Percentage_Service SHALL reject the deletion with an error indicating the record cannot be deleted in its current state
3. IF a System_Admin requests deletion of a TariffPercentageId that does not exist or has already been soft-deleted, THEN THE Tariff_Percentage_Service SHALL return a 404 Not Found response

### Requirement 5: Apply Percentage Propagation

**User Story:** As a System Admin, I want to trigger propagation of a tariff percentage to the ServiceProvider_Tariff table, so that all provider tariff records reflect the updated percentage.

#### Acceptance Criteria

1. WHEN a System_Admin triggers apply on a TariffPercentage_Record with Status "Pending" or "Failed", THE Tariff_Percentage_Service SHALL set the record Status to "Processing", queue a Propagation_Job containing the TariffPercentageId, PercentageAdded, TariffPeriodName, StartActiveDate, and EndActiveDate to the background channel, and return a TariffUpdateJobStatus with the generated jobId and Status "Queued"
2. IF a System_Admin triggers apply on a TariffPercentage_Record with Status "Processing", or another Propagation_Job is already Processing for the same TariffPeriodName, THEN THE Tariff_Percentage_Service SHALL reject the request with a 409 Conflict indicating a propagation job is already in progress for that period
3. IF a System_Admin triggers apply on a TariffPercentage_Record with Status "Completed", THEN THE Tariff_Percentage_Service SHALL reject the request with an error indicating the percentage has already been successfully applied
4. WHEN a Propagation_Job is successfully queued, THE API_Controller SHALL return a 202 Accepted response containing the jobId that the System_Admin can use to poll the job status endpoint
5. IF a System_Admin triggers apply with a TariffPercentageId that does not reference an existing TariffPercentage_Record, THEN THE Tariff_Percentage_Service SHALL reject the request with an error indicating the record was not found
6. THE Tariff_Percentage_Service SHALL set the TariffPercentage_Record Status to "Processing" before writing the Propagation_Job to the channel, ensuring that a concurrent apply request for the same record or TariffPeriodName is rejected

### Requirement 6: Background Propagation Processing

**User Story:** As a System Admin, I want the propagation to run in the background without blocking the admin UI, so that large bulk updates do not cause request timeouts.

#### Acceptance Criteria

1. WHEN the Background_Processor reads a Propagation_Job from the channel, THE Background_Processor SHALL set the TariffPercentage_Record status to "Processing" before beginning any data modifications
2. WHEN processing a Propagation_Job, THE Background_Processor SHALL close all ServiceProvider_Tariff_Records where TariffPeriodName equals the job TariffPeriodName minus one and EndActiveDate is null, by setting their EndActiveDate to one day before the job StartActiveDate
3. WHEN existing records are closed, THE Background_Processor SHALL insert one new ServiceProvider_Tariff_Record for each unique (ServiceProviderId, TariffNameId, MainClientId) combination from the closed prior-period records, setting StartActiveDate to the job StartActiveDate, EndActiveDate to the job EndActiveDate, TariffPeriodName to the job TariffPeriodName, and PercentageAdded to the job PercentageAdded
4. WHEN the propagation completes successfully, THE Background_Processor SHALL update the TariffPercentage_Record status to "Completed" and set RecordsAffected to the count of inserted records
5. IF the propagation fails at any point during processing, THEN THE Background_Processor SHALL roll back the entire transaction, set the TariffPercentage_Record status to "Failed", and store the error message truncated to 500 characters in the Notes field
6. THE Background_Processor SHALL execute the close and insert operations within a single database transaction to maintain data consistency
7. WHEN no ServiceProvider_Tariff_Records exist for the prior period where TariffPeriodName equals the job TariffPeriodName minus one and EndActiveDate is null, THE Background_Processor SHALL complete the job with RecordsAffected set to zero and Status set to "Completed"

### Requirement 7: Job Status Polling

**User Story:** As a System Admin, I want to poll the status of a propagation job, so that I can monitor progress and know when the operation completes.

#### Acceptance Criteria

1. WHEN a System_Admin requests the status of a job by jobId, THE API_Controller SHALL return the current job status including Status (one of Queued, Processing, Completed, or Failed), RecordsAffected (integer or null), ErrorMessage (string or null), StartedAt (datetime or null), and CompletedAt (datetime or null)
2. IF the requested jobId does not correspond to an existing job, THEN THE API_Controller SHALL return a 404 Not Found response
3. THE TariffUpdateJobStatus SHALL only transition through the following valid paths: Queued to Processing, Processing to Completed, or Processing to Failed
4. IF the requested jobId is not a valid GUID format, THEN THE API_Controller SHALL return a 400 Bad Request response with an error message indicating the jobId format is invalid

### Requirement 8: Case Letter Percentage Retrieval

**User Story:** As a Case_Letter_Generator, I want to retrieve the active tariff percentages for the most recent two years, so that I can include accurate tariff data in generated letters.

#### Acceptance Criteria

1. WHEN the Case_Letter_Generator requests active percentages, THE Tariff_Percentage_Service SHALL return TariffPercentage_Records for the two most recent years (determined by the highest TariffPeriodName values present in the data) with Status "Completed" and where the record has not been soft-deleted
2. IF multiple completed records exist for a single year, THEN THE Tariff_Percentage_Service SHALL return only the record with the latest EndActiveDate, where a null EndActiveDate indicates currently active and takes highest priority over any non-null EndActiveDate value
3. IF only one year of completed data exists, THEN THE Tariff_Percentage_Service SHALL return only that single year record
4. IF no completed TariffPercentage_Records exist, THEN THE Tariff_Percentage_Service SHALL return an empty collection
5. THE Tariff_Percentage_Service SHALL order the returned records by TariffPeriodName descending

### Requirement 9: Authorization and Access Control

**User Story:** As a system architect, I want all tariff percentage management endpoints restricted to SystemAdmin users, so that unauthorized users cannot modify tariff configurations.

#### Acceptance Criteria

1. THE API_Controller SHALL require the SystemAdmin role for all tariff percentage management endpoints
2. WHEN a non-SystemAdmin authenticated user attempts to access any tariff percentage endpoint, THE API_Controller SHALL return a 403 Forbidden response
3. IF an unauthenticated user attempts to access any tariff percentage endpoint, THEN THE API_Controller SHALL return a 401 Unauthorized response
4. THE Admin_UI SHALL restrict navigation to the tariff percentage management route using a role guard that verifies the SystemAdmin role, redirecting unauthorized users to the default route
5. THE Tariff_Percentage_Service SHALL record the authenticated user ID on all created and updated TariffPercentage_Records for audit purposes

### Requirement 10: Admin UI Display and Interaction

**User Story:** As a System Admin, I want a dedicated admin page for managing tariff percentages with clear status feedback, so that I can manage tariffs efficiently without SQL knowledge.

#### Acceptance Criteria

1. WHEN a System_Admin navigates to the tariff percentage management page, THE Admin_UI SHALL display a table of all tariff percentages showing period, start date, end date, percentage value, status, and records affected, sorted by TariffPeriodName descending then StartActiveDate descending
2. WHEN a System_Admin initiates creation of a new percentage, THE Admin_UI SHALL present a form with fields for PercentageAdded, TariffPeriodName, StartActiveDate, EndActiveDate, and Notes (maximum 500 characters)
3. WHEN a System_Admin clicks the Apply button for a pending or failed entry, THE Admin_UI SHALL display a confirmation dialog stating the period and percentage to be applied; IF the System_Admin confirms, THEN THE Admin_UI SHALL trigger propagation; IF the System_Admin cancels, THEN THE Admin_UI SHALL dismiss the dialog and take no further action
4. WHILE a Propagation_Job is in Processing status, THE Admin_UI SHALL poll the job status endpoint at an interval of 5 seconds and display a progress indicator on the corresponding table row until the job completes or fails
5. WHEN a Propagation_Job completes, THE Admin_UI SHALL update the table row to show the final status and records affected count
6. WHEN a Propagation_Job fails, THE Admin_UI SHALL display the error message from the job status response to the System_Admin
7. THE Admin_UI SHALL enable the Apply button only for TariffPercentage_Records with Status "Pending" or "Failed" and SHALL disable it for records with Status "Processing" or "Completed"
8. IF the System_Admin submits the creation form with values that violate validation rules (PercentageAdded less than or equal to zero, TariffPeriodName not a 4-digit year between 2000 and 2100, EndActiveDate earlier than StartActiveDate, or Notes exceeding 500 characters), THEN THE Admin_UI SHALL display field-level validation error messages and prevent form submission

### Requirement 11: Letter Template Integration and Field Mapping

**User Story:** As a Case_Letter_Generator, I want letter templates to dynamically display current and prior year tariff percentages from the management table, so that generated letters always reflect the latest approved rates without manual template edits.

#### Acceptance Criteria

1. WHEN the LetterTemplateService gathers case letter data, THE Tariff_Percentage_Service SHALL provide the two most recent years' tariff percentages as Handlebars-compatible fields: `{{TariffPercentageCurrentYear}}`, `{{TariffPercentageCurrentYearValue}}`, `{{TariffPercentagePriorYear}}`, and `{{TariffPercentagePriorYearValue}}`
2. WHEN letter data is gathered, THE LetterTemplateService SHALL populate `{{TariffPercentageCurrentYear}}` with the TariffPeriodName of the most recent completed record and `{{TariffPercentageCurrentYearValue}}` with its PercentageAdded value formatted to two decimal places
3. WHEN letter data is gathered and a prior year completed record exists, THE LetterTemplateService SHALL populate `{{TariffPercentagePriorYear}}` with the second most recent TariffPeriodName and `{{TariffPercentagePriorYearValue}}` with its PercentageAdded value formatted to two decimal places
4. IF no completed TariffPercentage_Records exist, THEN THE LetterTemplateService SHALL leave the tariff percentage Handlebars fields as empty strings so that no rendering errors occur
5. THE database letter templates (shared.LetterTemplate) SHALL be updated via a data migration to replace hardcoded tariff percentage text with Handlebars placeholders referencing the dynamic fields, specifically replacing literal strings like "NHRPL+ 233.90% for 2025" with template expressions using `{{TariffPercentagePriorYearValue}}` and `{{TariffPercentagePriorYear}}`
6. WHEN a letter template is rendered, THE LetterTemplateService SHALL produce output where the tariff percentage lines display the dynamically retrieved year and percentage values in the same format as the previously hardcoded text (e.g., "Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentagePriorYearValue}}% for {{TariffPercentagePriorYear}}")

### Requirement 12: Data Integrity and Consistency

**User Story:** As a system architect, I want the propagation to maintain referential integrity and prevent data corruption, so that the ServiceProvider_Tariff table remains consistent.

#### Acceptance Criteria

1. THE Background_Processor SHALL ensure that for each (ServiceProviderId, TariffNameId, MainClientId) combination, the EndActiveDate of the prior period record equals the StartActiveDate of the new period record minus one day, so that no date gaps or overlaps exist between consecutive periods
2. THE Background_Processor SHALL verify before inserting new ServiceProvider_Tariff_Records that no record already exists for the same (ServiceProviderId, TariffNameId, MainClientId, TariffPeriodName) combination, and SHALL skip insertion for any combination where a record already exists
3. IF a System_Admin attempts to apply a TariffPercentage_Record that has Status "Completed", THEN THE Tariff_Percentage_Service SHALL reject the request with an error indicating the percentage has already been applied
4. THE Background_Processor SHALL ensure that for each (ServiceProviderId, TariffNameId, MainClientId) combination, no two ServiceProvider_Tariff_Records have overlapping date ranges within or across periods
