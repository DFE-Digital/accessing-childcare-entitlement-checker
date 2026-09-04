.PHONY: \
	build \
	test \
	test-e2e \
	test-a11y \
	playwright-i \
	inspect \
	inspect-a \
	inspect-r \
	inspect-f \
	verify \
	tf-f \
	tf-v \
	tf-d \
	tf-i \
	tf \
	docs-c \
	docs-b \
	docs-s \
	docs-n

# ---------------------------------------------------------------------------
# Configuration
# ---------------------------------------------------------------------------

SOLUTION := Dfe.Acec.slnx
TEST_RESULTS := ./.test-results
ANALYSIS_RESULTS := ./.analysis-results

# ---------------------------------------------------------------------------
# Build
# ---------------------------------------------------------------------------

build:
	dotnet clean --nologo --verbosity minimal
	dotnet restore --locked-mode
	dotnet format --verify-no-changes --verbosity minimal
	dotnet build --no-restore --no-incremental --nologo --verbosity minimal

# ---------------------------------------------------------------------------
# Inspect
# ---------------------------------------------------------------------------

inspect-a:
	dotnet tool run jb inspectcode $(SOLUTION) \
		--output=$(ANALYSIS_RESULTS)/inspectcode.sarif \
		--format=Sarif

inspect-r:
	jq -r '[.runs[0].results[]?] | group_by(.level) | .[] | "[\(.[0].level)]", (group_by(.ruleId) | .[] | "  \(.[0].ruleId): \(length)")' $(ANALYSIS_RESULTS)/inspectcode.sarif

rule ?= $(ruleid)

inspect-f:
	jq -r --arg rule "$(rule)" 'if $$rule == "" then error("Please specify rule=VALUE, e.g. make inspect-f rule=InconsistentNaming") else . end | [.runs[0].results[]? | select(.ruleId == $$rule)] | .[] | "[\(.level)] \(.ruleId): \(.message.text) - \(.locations[0].physicalLocation.artifactLocation.uri):\(.locations[0].physicalLocation.region.startLine)"' $(ANALYSIS_RESULTS)/inspectcode.sarif

inspect: inspect-a inspect-r

# ---------------------------------------------------------------------------
# Tests
# ---------------------------------------------------------------------------

test:
	dotnet test tests/Dfe.Acec.Web.Tests.Unit \
		--no-build \
		--coverlet \
		--coverlet-output-format opencover \
		--coverlet-include "[Dfe.Acec.*]*"

	dotnet test tests/Dfe.Acec.RulesEngine.Tests.Unit \
		--no-build \
		--coverlet \
		--coverlet-output-format opencover \
		--coverlet-include "[Dfe.Acec.*]*"

	dotnet test tests/Dfe.Acec.Web.Tests.Integration \
		--no-build \
		--coverlet \
		--coverlet-output-format opencover \
		--coverlet-include "[Dfe.Acec.*]*"

test-e2e:
	dotnet test tests/Dfe.Acec.Web.Tests.E2e \
		--no-build

test-a11y:
	dotnet test tests/Dfe.Acec.Web.Tests.A11y \
		--no-build

playwright-i:
	pwsh ./.artifacts/bin/Dfe.Acec.Web.Tests.E2e/debug/playwright.ps1 install --with-deps

# ---------------------------------------------------------------------------
# Terraform
# ---------------------------------------------------------------------------

tf-i:
	terraform -chdir=infra/terraform init -input=false -backend=false

tf-f:
	terraform -chdir=infra/terraform fmt -recursive

tf-v:
	terraform -chdir=infra/terraform validate

tf-d:
	terraform-docs -c .terraform-docs.yml \
		--output-file ../../docs/content/reference/architecture/deployed-infrastructure.md \
		--output-mode inject \
		infra/terraform

tf: tf-i tf-f tf-v tf-d

# ---------------------------------------------------------------------------
# Verification
# ---------------------------------------------------------------------------

verify: build inspect test tf

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
