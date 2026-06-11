---
title: Accessibility Test Plan
layout: sub-navigation
sectionKey: Testing
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Testing
  key: Accessibility Test Plan
order: 2
---
The objective of this test plan is to verify that the application meets the requirements of WCAG 2.2 Level AA and provides an accessible experience for users who rely on assistive technologies, keyboard navigation, screen readers, zoom, and other accessibility accommodations.

Testing will focus on:

- Accessibility compliance with WCAG 2.2 AA
- End-to-end completion of the form journey
- Keyboard-only operation
- Screen reader usability
- Form validation and error handling
- Responsive and zoomed layouts
- Colour contrast and visual accessibility

## Scope

### In Scope

User Journey Testing:

- Start page
- Form pages
- Conditional question pages
- Review and summary pages

Accessibility Areas:

- Semantic HTML
- Keyboard accessibility
- Screen reader support
- Focus management
- Error handling
- Form labels and instructions
- Colour contrast
- Responsive design
- Zoom and reflow

### Out of Scope

- Performance testing
- Security testing
- Browser compatibility testing unrelated to accessibility
- Content quality review
- Accessibility of third-party systems not controlled by the project

## Test Approach

Accessibility testing will be performed using a combination of:

1. Automated accessibility testing using Playwright and axe-core
2. Keyboard-only testing
3. Screen reader testing
4. Visual accessibility testing
5. Manual WCAG review

This layered approach ensures coverage of both detectable technical issues and user experience issues that cannot be identified through automation alone.

## Test Environment

Browsers: Google Chrome (latest supported version)  
Screen Readers: NVDA (latest version) and/or JAWS (latest version)  
Accessibility Tools: 

- Playwright
- axe-core
- axe-playwright
- Accessibility Insights
- WAVE
- Lighthouse
- Colour Contrast Analyser

## Page Coverage Strategy

Pages will be grouped by functional pattern.

| Pattern            | Example Pages       | Coverage |
|--------------------|---------------------|----------|
| Text input forms   | Personal details    | Full     |
| Conditional forms  | Dynamic questions   | Full     |
| Review pages       | Summary page        | Full     |

Where reusable components are used across multiple pages, accessibility findings will be assessed at component level and spot-checked across implementations.

## Automated Accessibility Testing

Identify accessibility issues through automated testing integrated into the application's test suite and CI/CD pipeline.

Tools: 

- Playwright
- axe-core
- axe-playwright

### Approach

Accessibility scans will be executed as part of automated end-to-end tests using Playwright and axe.

Tests will run:

- During local development
- During pull request validation
- As part of CI/CD pipelines
- Prior to production releases

Accessibility testing shall be treated as a standard part of the automated regression suite rather than a separate activity.

### Coverage Requirements

Accessibility scans shall be executed against:

- Initial Page State: Each page loaded with no user interaction.
- Validation Error State: Pages containing validation errors.
- Completed State: Pages containing valid data.
- Conditional Content State: Pages where dynamic content is displayed following user interaction.

### Reporting

Automated test results shall capture:

- WCAG rule violated
- Severity
- Affected element
- Recommended remediation
- Build identifier

Results shall be retained as part of CI/CD artefacts.

### Limitations

Automated testing cannot reliably verify:

- Screen reader usability
- Quality of labels and instructions
- Logical reading order
- Focus management quality
- Meaningfulness of alternative text
- Overall task completion usability

Manual accessibility testing remains mandatory.

## Keyboard Accessibility Testing

Verify all functionality can be completed using only a keyboard.

### Test Method

Use:

- Tab
- Shift + Tab
- Enter
- Space
- Arrow keys
- Escape

No mouse interaction permitted.

### Test Cases

Navigation:

- Logical tab order
- No inaccessible controls
- No keyboard traps
- Skip links function correctly

Form Controls:

- Inputs are reachable
- Radio buttons operate correctly
- Checkboxes operate correctly
- Dropdowns operate correctly

Dynamic Content:

- Focus moves appropriately
- Newly displayed content is accessible

### Expected Outcome

Entire form journey can be completed using keyboard-only interaction.

## Screen Reader Testing

Verify users can understand and complete the application using a screen reader.

### Test Scenarios

Page Structure:

- Page title is meaningful
- Heading hierarchy is logical
- Landmarks are present where appropriate

Form Controls:

- Labels are announced correctly
- Required fields are identified
- Instructions are announced

Validation Errors:

- Errors are announced
- Error summaries are accessible
- Focus is directed appropriately

Dynamic Updates:

- Status messages are announced
- Conditional content changes are communicated

### Expected Outcome

All critical user journeys can be completed without visual assistance.

## Error Handling Testing

Verify accessible identification and recovery from errors.

### Test Cases

Missing Required Fields:

- Error message displayed
- Error message announced
- User can identify affected field

Invalid Data:

- Error explanation provided
- Suggested correction provided where applicable

Validation Summary:

- Summary receives focus where appropriate
- Links navigate directly to fields

### Relevant WCAG Criteria

- 3.3.1 Error Identification
- 3.3.2 Labels or Instructions
- 3.3.3 Error Suggestion

## Focus Management Testing

Verify focus remains predictable and meaningful.

### Test Cases

Page Navigation:

- Focus moves to page heading or expected content

Validation Errors:

- Focus moves to error summary or first invalid field

Conditional Content:

- Focus is managed appropriately when content appears

### Relevant WCAG Criteria

- 2.4.3 Focus Order
- 2.4.7 Focus Visible
- 2.4.11 Focus Not Obscured (Minimum)

## Colour Contrast Testing

Verify sufficient visual contrast.

### Test Cases

Text Contrast:

- Normal text ≥ 4.5:1
- Large text ≥ 3:1

User Interface Components:

- Focus indicators
- Form controls
- Error indicators

Non-Colour Indicators:

- Errors are not communicated using colour alone

### Relevant WCAG Criteria

- 1.4.3 Contrast (Minimum)
- 1.4.11 Non-text Contrast
- 1.3.3 Sensory Characteristics

## Zoom and Reflow Testing

Verify usability at increased zoom levels.

### Test Cases

200% Zoom:

- All content remains visible
- No loss of functionality

400% Zoom:

- Reflow occurs correctly
- No prohibited horizontal scrolling

Responsive Layout:

- Form controls remain usable
- Error messages remain visible
- Navigation remains accessible

### Relevant WCAG Criteria

- 1.4.10 Reflow
- 1.4.4 Resize Text

## WCAG 2.2 AA Review

A manual review will be conducted against applicable WCAG 2.2 AA Success Criteria.

### Perceivable

- 1.3.1 Info and Relationships
- 1.3.5 Identify Input Purpose
- 1.4.3 Contrast (Minimum)
- 1.4.10 Reflow

### Operable

- 2.1.1 Keyboard
- 2.4.3 Focus Order
- 2.4.7 Focus Visible
- 2.4.11 Focus Not Obscured (Minimum)

### Understandable

- 3.3.1 Error Identification
- 3.3.2 Labels or Instructions
- 3.3.3 Error Suggestion

### Robust

- 4.1.2 Name, Role, Value
- 4.1.3 Status Messages

## Defect Classification

| Severity | Description                                   |
|----------|-----------------------------------------------|
| Critical | Prevents completion of a core user journey    |
| High     | Significant accessibility barrier             |
| Medium   | Accessibility issue with workaround available |
| Low      | Minor accessibility or usability issue        |

## Exit Criteria

Testing will be considered complete when:

- All page types have been assessed
- All reusable components have been assessed
- End-to-end journeys have been tested
- Automated Playwright + axe accessibility tests are passing
- Accessibility tests are executing in CI/CD
- Keyboard-only journey is successful
- Screen reader journey is successful
- WCAG 2.2 AA review is complete
- All Critical defects are resolved
- All High severity defects are resolved or formally accepted

## Deliverables

The following outputs will be produced:

- Accessibility Test Report
- WCAG 2.2 AA Compliance Matrix
- Accessibility Defect Log
- Playwright Accessibility Test Suite
- CI/CD Accessibility Test Results
- Test Evidence
- Remediation Recommendations