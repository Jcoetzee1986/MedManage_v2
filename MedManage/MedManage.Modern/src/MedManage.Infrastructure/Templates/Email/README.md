# Email Templates

This folder contains HTML email templates used by the MedManage EmailService.

## Template Structure

Templates use a simple placeholder syntax with double curly braces: `{{PlaceholderName}}`

### Available Templates

#### 1. PasswordResetPin.html
**Purpose:** Password reset email with 6-digit PIN

**Placeholders:**
- `{{Username}}` - User's username
- `{{Pin}}` - 6-digit PIN code  
- `{{Year}}` - Current year for copyright

**Usage:**
```csharp
await emailService.SendPasswordResetPinAsync("user@example.com", "john.doe", "123456");
```

---

#### 2. Welcome.html
**Purpose:** Welcome email sent to new registered users

**Placeholders:**
- `{{Username}}` - User's username
- `{{LoginUrl}}` - Link to login page
- `{{Year}}` - Current year for copyright

**Usage:**
```csharp
await emailService.SendWelcomeEmailAsync("user@example.com", "john.doe");
```

## Editing Templates

### To modify an existing template:
1. Edit the `.html` file in this folder
2. Rebuild the project (templates are copied to output directory)
3. Restart the application

### To add a new template:
1. Create a new `.html` file in this folder
2. Use the `{{PlaceholderName}}` syntax for dynamic content
3. Update `MedManage.Infrastructure.csproj` to include the new template:
   ```xml
   <ItemGroup>
     <None Update="Templates\Email\*.html">
       <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
     </None>
   </ItemGroup>
   ```
4. Add a method to `IEmailService` interface
5. Implement the method in `EmailService` class

### Template Loading

The `EmailService` class loads templates from:
```
{AssemblyLocation}/Templates/Email/{TemplateName}.html
```

**Fallback Behavior:**
- If a template file is not found, the service uses embedded fallback templates
- A warning is logged when fallback templates are used
- This ensures emails are always sent even if template files are missing

## CSS Styling

Templates use inline CSS for maximum email client compatibility:
- **Avoid external stylesheets** - Not supported by most email clients
- **Use inline styles** - Applied directly to HTML elements
- **Use `<style>` tags in `<head>`** - Supported by most modern email clients
- **Test across clients** - Outlook, Gmail, Apple Mail have different rendering

## Test Mode

When `EmailSettings.UseTestMode = true`:
- All emails are redirected to `EmailSettings.TestEmailAddress`
- A yellow banner is added to the email showing the original recipient
- Template content remains unchanged

## Best Practices

1. **Keep it simple** - Complex layouts may not render correctly
2. **Use tables for layout** - More reliable than divs/flexbox
3. **Limit width to 600px** - Standard for email compatibility
4. **Test with real SMTP** - Rendering can differ from browser preview
5. **Use web-safe fonts** - Arial, Helvetica, Georgia, Times New Roman
6. **Optimize images** - Keep file sizes small, use absolute URLs
7. **Include plain text alternative** - For accessibility
8. **Validate HTML** - Use W3C validator before deploying

## Troubleshooting

### Template Not Found Warning
**Problem:** Log shows "Email template not found at: {path}"

**Solutions:**
- Verify template file exists in `Templates/Email/` folder
- Check file name matches exactly (case-sensitive on Linux)
- Ensure project builds successfully (templates are copied during build)
- Check output directory: `bin/Debug/net10.0/Templates/Email/`

### Placeholders Not Replaced
**Problem:** Email shows `{{Username}}` instead of actual username

**Solutions:**
- Verify placeholder name matches exactly (case-sensitive)
- Check `ReplaceTemplatePlaceholders` is called with correct dictionary
- Inspect email body before sending in debugger

### Styling Issues
**Problem:** Email looks different in different email clients

**Solutions:**
- Test with Email on Acid or Litmus
- Simplify CSS - remove advanced features
- Use inline styles instead of classes where possible
- Check Outlook-specific rendering issues

## Related Files

- **EmailService.cs** - Service that loads and processes templates
- **IEmailService.cs** - Interface defining email methods
- **EmailSettings.cs** - Configuration for SMTP and test mode
- **appsettings.json** - SMTP configuration values

## Future Enhancements

- [ ] Support for multiple languages (i18n)
- [ ] Template versioning/A-B testing
- [ ] Rich template engine (Razor, Liquid, Handlebars)
- [ ] Template preview endpoint for testing
- [ ] Email analytics/tracking
- [ ] Attachment support
- [ ] Template marketplace/library
