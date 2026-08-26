.PHONY: build test test-e2e test-a11y tf-f tf-v tf-docs

# .NET Targets
build:
	dotnet clean
	dotnet restore --locked-mode
	dotnet format
	dotnet build --no-restore

# Test Targets
test:
	dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests --no-build
	dotnet test tests/AccessingChildcareEntitlementChecker.IntegrationTests --no-build

test-e2e:
	dotnet test tests/AccessingChildcareEntitlementChecker.E2eTests --no-build

test-a11y:
	dotnet test tests/AccessingChildcareEntitlementChecker.A11yTests --no-build

# Terraform Targets
tf-f:
	terraform fmt -recursive infra/

tf-v:
	terraform -chdir=infra/terraform validate

tf-docs:
	terraform-docs -c .terraform-docs.yml --output-file ../../docs/content/reference/architecture/deployed-infrastructure.md --output-mode inject infra/terraform

# Documentation Targets
docs-c:
	cd docs && npm run clean

docs-b:
	cd docs && npm install && npm run build

docs-s:
	cd docs && npm install && npm start

# Flatten markdown documentation by folder into folder-specific notebooks
flatten-docs:
	pwsh ./scripts/flatten-docs.ps1
