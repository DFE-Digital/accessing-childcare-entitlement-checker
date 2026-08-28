.PHONY: \
	build \
	test \
	test-e2e \
	test-a11y \
	playwright-i \
	analyze \
	analyse-d \
	analyse-f \
	analyse-i \
	analyse-i-summary \
	analyse-i-find \
	verify \
	tf-f \
	tf-v \
	tf-docs \
	docs-c \
	docs-b \
	docs-s \
	docs-n

# ---------------------------------------------------------------------------
# Configuration
# ---------------------------------------------------------------------------

SOLUTION := AccessingChildcareEntitlementChecker.slnx
TEST_RESULTS := ./.test-results
ANALYSIS_RESULTS := ./.analysis-results

# ---------------------------------------------------------------------------
# .NET - Build
# ---------------------------------------------------------------------------

build:
	dotnet clean
	dotnet restore --locked-mode
	dotnet format --verify-no-changes
	dotnet build --no-restore

# ---------------------------------------------------------------------------
# .NET - Static Analysis
# ---------------------------------------------------------------------------

# Fast analysis using the .NET SDK / Roslyn.
analyse-d:
	dotnet build \
		--no-restore \
		--nologo \
		--verbosity minimal

# Verify formatting without modifying files.
analyse-f:
	dotnet format \
		--verify-no-changes \
		--no-restore

# Deep InspectCode analysis.
analyse-i:
	dotnet tool run jb inspectcode $(SOLUTION) \
		--output=$(ANALYSIS_RESULTS)/inspectcode.sarif \
		--format=Sarif

analyse-i-summary:
	jq -r '[.runs[0].results[]?] | group_by(.level) | .[] | "[\(.[0].level)]", (group_by(.ruleId) | .[] | "  \(.[0].ruleId): \(length)")' $(ANALYSIS_RESULTS)/inspectcode.sarif

rule ?= $(ruleid)

analyse-i-find:
	jq -r --arg rule "$(rule)" 'if $$rule == "" then error("Please specify rule=VALUE, e.g. make analyse-i-find rule=InconsistentNaming") else . end | [.runs[0].results[]? | select(.ruleId == $$rule)] | .[] | "[\(.level)] \(.ruleId): \(.message.text) - \(.locations[0].physicalLocation.artifactLocation.uri):\(.locations[0].physicalLocation.region.startLine)"' $(ANALYSIS_RESULTS)/inspectcode.sarif

# Run all static analysis.
analyse: analyse-d analyse-f analyse-i

# ---------------------------------------------------------------------------
# Verification
# ---------------------------------------------------------------------------

# Full local verification.
verify: build analyse test

# ---------------------------------------------------------------------------
# Tests
# ---------------------------------------------------------------------------

test:
	dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests \
		--no-build \
		--results-directory $(TEST_RESULTS) \
		--logger "trx" \
		/p:CollectCoverage=true \
		/m:1

	dotnet test tests/AccessingChildcareEntitlementChecker.IntegrationTests \
		--no-build \
		--results-directory $(TEST_RESULTS) \
		--logger "trx" \
		/p:CollectCoverage=true \
		/m:1

test-e2e:
	dotnet test tests/AccessingChildcareEntitlementChecker.E2eTests \
		--no-build \
		--logger:"console;verbosity=normal"

test-a11y:
	dotnet test tests/AccessingChildcareEntitlementChecker.A11yTests \
		--no-build \
		--logger:"console;verbosity=normal"

playwright-i:
	pwsh ./.artifacts/bin/AccessingChildcareEntitlementChecker.E2eTests/debug/playwright.ps1 install --with-deps

# ---------------------------------------------------------------------------
# Terraform
# ---------------------------------------------------------------------------

tf-f:
	terraform fmt -recursive infra/

tf-v:
	terraform -chdir=infra/terraform validate

tf-docs:
	terraform-docs -c .terraform-docs.yml \
		--output-file ../../docs/content/reference/architecture/deployed-infrastructure.md \
		--output-mode inject \
		infra/terraform

# ---------------------------------------------------------------------------
# Documentation
# ---------------------------------------------------------------------------

docs-c:
	cd docs && npm run clean

docs-b:
	cd docs && npm install && npm run build

docs-s:
	cd docs && npm install && npm start

# Flatten markdown documentation by folder into folder-specific notebooks
docs-n:
	pwsh ./scripts/docs-notebook.ps1